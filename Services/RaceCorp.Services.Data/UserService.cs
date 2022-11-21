namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.ApplicationUsers;
    using RaceCorp.Web.ViewModels.Common;

    public class UserService : IUserService
    {
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IFileService fileService;
        private readonly IDeletableEntityRepository<Request> requestRepo;
        private readonly IDeletableEntityRepository<Conversation> conversationRepo;

        public UserService(
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Town> townRepo,
            IFileService fileService,
            IDeletableEntityRepository<Request> requestRepo,
            IDeletableEntityRepository<Conversation> conversationRepo)
        {
            this.userStore = userStore;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.townRepo = townRepo;
            this.fileService = fileService;
            this.requestRepo = requestRepo;
            this.conversationRepo = conversationRepo;
        }

        public async Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath)
        {
            var user = this.userRepo.All().Include(u => u.Images).Include(u => u.Town).FirstOrDefault(u => u.Id == inputModel.Id);

            if (inputModel.UserProfilePicture != null)
            {
                var image = await this.fileService
                    .ProccessingImageData(
                    inputModel.UserProfilePicture,
                    inputModel.Id,
                    roothPath,
                    inputModel.FirstName + inputModel.LastName + GlobalConstants.ProfilePicterPostFix);

                user.ProfilePicturePath = $"\\{image.ParentFolderName}\\{image.ChildFolderName}\\{image.Id}.{image.Extension}";
                image.Name = GlobalConstants.ProfilePictire;
                user.Images.Add(image);
            }

            var townDb = this.townRepo.All().FirstOrDefault(t => t.Name == inputModel.Town);

            if (townDb == null)
            {
                townDb = new Town
                {
                    Name = inputModel.Town,
                };

                await this.townRepo.AddAsync(townDb);
                user.Town = townDb;
            }

            user.FacoBookLink = inputModel.FacoBookLink;
            user.LinkedInLink = inputModel.LinkedInLink;
            user.TwitterLink = inputModel.TwitterLink;
            user.GitHubLink = inputModel.GitHubLink;

            user.About = inputModel.About;

            if (user.FirstName != inputModel.FirstName ||
               user.LastName != inputModel.LastName)
            {
                user.FirstName = inputModel.FirstName;
                user.LastName = inputModel.LastName;

                await this.UpdateClaim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}", user);
            }

            if (user.Gender != inputModel.Gender)
            {
                user.Gender = inputModel.Gender;
                await this.UpdateClaim(ClaimTypes.Gender, inputModel.Gender.ToString(), user);
            }

            if (user.Country != inputModel.Country)
            {
                user.Country = inputModel.Country;
                await this.UpdateClaim(ClaimTypes.Country, inputModel.Gender.ToString(), user);
            }

            if (user.DateOfBirth != inputModel.DateOfBirth)
            {
                user.DateOfBirth = inputModel.DateOfBirth;
                await this.UpdateClaim(ClaimTypes.DateOfBirth, inputModel.Gender.ToString(), user);
            }

            await this.userManager.UpdateAsync(user);
            await this.signInManager.RefreshSignInAsync(user);
            await this.userRepo.SaveChangesAsync();

            return true;
        }

        public List<T> GetAllAsync<T>()
        {
            return this.userRepo.AllAsNoTracking().To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            return this.userRepo
                .All()
                .Where(u => u.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public UserInboxViewModel GetByIdUserInboxViewModel(string id)
        {
            var userDb = this.userRepo
                .AllAsNoTracking()
                .Include(u => u.Conversations)
                .FirstOrDefault(u => u.Id == id);

            return new UserInboxViewModel
            {
                Id = id,
                ProfilePicturePath = userDb.ProfilePicturePath,
                Conversations = userDb.Conversations.Select(c => new UserConversationViewModel
                {
                    Id = c.Id,
                    AuthorId = c.AuthorId,
                    InterlocutorId = c.InterlocutorId,
                    Email = c.UserEmail,
                    LastMessageContent = c.LastMessageContent,
                    UserFirstName = c.UserFirstName,
                    UserLastName = c.UserLastName,
                    UserProfilePicturePath = c.UserProfilePicturePath,
                }).ToList(),
            };
        }

        public MessageInputModel GetMessageModelAsync(string receiverId, string senderId)
        {
            var sender = this.userRepo.All().Include(u => u.Conversations).FirstOrDefault(u => u.Id == senderId);
            var receiver = this.userRepo.All().Include(u => u.Conversations).FirstOrDefault(u => u.Id == receiverId);

            return new MessageInputModel
            {
                ReceiverProfilePicurePath = receiver.ProfilePicturePath,
                ReceiverFirstName = receiver.FirstName,
                ReceiverId = receiver.Id,
                ReceiverLastName = receiver.LastName,
            };
        }

        public List<T> GetRequest<T>(string userId)
        {
            return this.requestRepo
                .AllAsNoTracking()
                .Include(r => r.ApplicationUser)
                .Where(r => r.ApplicationUserId == userId)
                .OrderBy(r => r.IsApproved)
                .To<T>()
                .ToList();
        }

        public async Task SaveMessageAsync(MessageInputModel model, string senderId)
        {
            var receiver = this.userRepo.All().Include(u => u.Conversations).FirstOrDefault(u => u.Id == model.ReceiverId);
            var sender = this.userRepo.All().Include(u => u.Conversations).FirstOrDefault(u => u.Id == senderId);
            if (sender == null || receiver == null)
            {
                throw new NullReferenceException();
            }

            //validate sender and receiver
            if (sender.Conversations.Any(c => c.Id == sender.Id + receiver.Id || c.Id == receiver.Id + senderId) == false)
            {
                var conversationSender = new Conversation
                {
                    Id = receiver.Id + sender.Id,
                    CreatedOn = DateTime.UtcNow,
                    AuthorId = sender.Id,
                    InterlocutorId = receiver.Id,
                };

                var conversationReceiver = new Conversation
                {
                    Id = sender.Id + receiver.Id,
                    CreatedOn = DateTime.UtcNow,
                    AuthorId = receiver.Id,
                    InterlocutorId = sender.Id,

                };

                receiver.Conversations.Add(conversationReceiver);
                sender.Conversations.Add(conversationSender);
            }

            var message = new Message
            {
                CreatedOn = DateTime.UtcNow,
                Sender = sender,
                Content = model.Content,
                Receiver = receiver,
            };

            var receiverConvrs = receiver.Conversations.FirstOrDefault(c => c.Id == sender.Id + receiver.Id || c.Id == receiver.Id + senderId);
            var senderConvrs = sender.Conversations.FirstOrDefault(c => c.Id == sender.Id + receiver.Id || c.Id == receiver.Id + senderId);

            this.UpdateConversation(receiverConvrs, message, sender);
            this.UpdateConversation(senderConvrs, message, receiver);

            receiver.InboxMessages.Add(message);
            sender.SentMessages.Add(message);

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
        }

        private void UpdateConversation(Conversation conversation, Message message, ApplicationUser user)
        {
            conversation.LastMessageContent = message.Content;
            conversation.LastMessageDate = message.CreatedOn.ToString("g");
            conversation.UserProfilePicturePath = user.ProfilePicturePath;
            conversation.ModifiedOn = message.CreatedOn;
            conversation.UserEmail = user.Email;
            conversation.UserFirstName = user.FirstName;
            conversation.UserLastName = user.LastName;
        }

        private async Task UpdateClaim(string claimType, string value, ApplicationUser user)
        {
            var claim = this.userManager.GetClaimsAsync(user).Result.FirstOrDefault(c => c.Type == claimType);

            if (claim == null)
            {
                await this.userManager.AddClaimAsync(user, new Claim(claimType, value));
            }
            else
            {
                user.Claims.Where(c => c.ClaimType == claimType).FirstOrDefault().ClaimValue = value;
            }
        }


        public bool RequestedConnection(string currentUserId, string targetUserId)
        {
            return this.userRepo
                .AllAsNoTracking()
                .Include(u => u.Requests)
                .FirstOrDefault(u => u.Id == currentUserId)
                .Requests
                .Any(r => r.RequesterId == targetUserId);
        }
    }
}
