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

        public async Task AddAsync(string currentUserId, string targetUserId)
        {
            var userDb = this.userRepo.All().Include(u => u.Connections).FirstOrDefault(u => u.Id == currentUserId);

            var targetUserDb = this.userRepo.All().Include(u => u.Connections).FirstOrDefault(u => u.Id == targetUserId);

            if (userDb != null && targetUserDb != null)
            {
                if (userDb.Connections.Any(c => c.Id == targetUserDb.Id))
                {
                    throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
                }

                userDb.Connections.Add(targetUserDb);
                targetUserDb.Connections.Add(userDb);

                await this.userRepo.SaveChangesAsync();
            }
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
            return this.userRepo.All().Where(u => u.Id == id).To<T>().FirstOrDefault();
        }

        public UserInboxViewModel GetByIdUserInboxViewModel(string id)
        {
            var userDb = this.userRepo
                .AllAsNoTracking()
                .Include(u => u.Conversations).ThenInclude(c => c.Messages.OrderByDescending(m => m.CreatedOn))
                .Include(u => u.Conversations).ThenInclude(c => c.UserA)
                .Include(u => u.Conversations).ThenInclude(c => c.UserB)
                .FirstOrDefault(u => u.Id == id);

            return new UserInboxViewModel
            {
                Id = id,
                ProfilePicturePath = userDb.ProfilePicturePath,
                Conversations = userDb.Conversations.Select(c => new UserConversationViewModel
                {
                    Id = c.Id,
                    LastMessageContent = c.Messages.LastOrDefault().Content,
                    UserFirstName = c.UserA.Id != id ? c.UserA.FirstName : c.UserB.FirstName,
                    UserLastName = c.UserA.Id != id ? c.UserA.LastName : c.UserB.LastName,
                    UserProfilePicturePath = c.UserA.Id != id ? c.UserA.ProfilePicturePath : c.UserB.ProfilePicturePath,
                }).ToList(),
            };
        }

        public async Task<MessageInputModel> GetMessageModelAsync(string receiverId, string senderId)
        {
            var sender = this.userRepo.All().Include(u => u.Conversations).FirstOrDefault(u => u.Id == senderId);
            var receiver = this.userRepo.All().Include(u => u.Conversations).FirstOrDefault(u => u.Id == receiverId);

            var conversation = this.conversationRepo.All().FirstOrDefault(c => c.Id == sender.Id + receiver.Id || c.Id == receiver.Id + sender.Id);

            if (conversation == null)
            {
                conversation = new Conversation()
                {
                    Id = sender.Id + receiver.Id,
                    CreatedOn = DateTime.UtcNow,
                };

                conversation.UserB = sender;
                conversation.UserA = receiver;

                sender.Conversations.Add(conversation);
                receiver.Conversations.Add(conversation);

                await this.conversationRepo.AddAsync(conversation);
            }

            try
            {
                await this.conversationRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

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
            var conversation = this.conversationRepo.All().FirstOrDefault(c => c.Id == model.ReceiverId + senderId || c.Id == senderId + model.ReceiverId);
            var receiver = this.userRepo.All().Include(u => u.Conversations).FirstOrDefault(u => u.Id == model.ReceiverId);

            var sender = this.userRepo.All().Include(u => u.Conversations).FirstOrDefault(u => u.Id == senderId);

            if (receiver == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

            if (conversation == null)
            {
                conversation = new Conversation()
                {
                    Id = sender.Id + receiver.Id,
                    CreatedOn = DateTime.UtcNow,
                };


                sender.Conversations.Add(conversation);
                receiver.Conversations.Add(conversation);

                conversation.UserA = sender;
                conversation.UserB = receiver;

                await this.conversationRepo.AddAsync(conversation);

            }

            var message = new Message
            {
                Conversation = conversation,
                CreatedOn = DateTime.UtcNow,
                Sender = sender,
                Content = model.Content,
                Receiver = receiver,
            };

            conversation.Messages.Add(message);

            try
            {
                await this.conversationRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
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
    }
}
