namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Message;
    using RaceCorp.Web.ViewModels.User;

    public class MessageService : IMessageService
    {
        private readonly IDeletableEntityRepository<Message> messageRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;

        public MessageService(
            IDeletableEntityRepository<Message> messageRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo)
        {
            this.messageRepo = messageRepo;
            this.userRepo = userRepo;
        }

        public async Task<List<T>> GetMessages<T>(string userId, string interlocutorId)
        {
            var user = this.userRepo

                  .All()
                  .Include(u => u.InboxMessages)
                  .FirstOrDefault(u => u.Id == userId);

            foreach (var message in user.InboxMessages)
            {
                message.IsRead = true;
            }

            await this.userRepo.SaveChangesAsync();

            return this.messageRepo
                .AllAsNoTracking()
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .Where(m =>
                (m.RevceiverId == userId && m.SenderId == interlocutorId) ||
                (m.RevceiverId == interlocutorId && m.SenderId == userId))
                .OrderBy(m => m.CreatedOn)
                .To<T>()
                .ToList();
        }

        public UserInboxViewModel GetByIdUserInboxViewModel(string id)
        {
            var userDb = this.userRepo
                .AllAsNoTracking()
                .Include(u => u.InboxMessages)
                .Include(u => u.Conversations)
                .FirstOrDefault(u => u.Id == id);

            if (userDb == null)
            {
                throw new UnauthorizedAccessException(GlobalErrorMessages.UnauthorizedRequest);
            }

            return new UserInboxViewModel
            {
                Id = id,
                ProfilePicturePath = userDb.ProfilePicturePath,
                Conversations = userDb.Conversations.OrderByDescending(c => c.LastMessageDate).Select(c => new UserConversationViewModel
                {
                    Id = c.Id,
                    AuthorId = c.ApplicationUserId,
                    InterlocutorId = c.InterlocutorId,
                    Email = c.UserEmail,
                    LastMessageContent = c.LastMessageContent,
                    UserFirstName = c.UserFirstName,
                    UserLastName = c.UserLastName,
                    UserProfilePicturePath = c.UserProfilePicturePath,
                    LastMessageDate = c.LastMessageDate.ToString(GlobalConstants.DateStringFormat),
                    UnreadMessages = userDb.InboxMessages.Where(m => m.SenderId == c.InterlocutorId && m.IsRead == false).ToList().Count(),
                }).ToList(),
            };
        }

        public MessageInputModel GetMessageModelAsync(string receiverId, string senderId)
        {
            var sender = this.userRepo
                .All()
                .Include(u => u.Conversations)
                .FirstOrDefault(u => u.Id == senderId);

            var receiver = this.userRepo
                .All()
                .Include(u => u.Conversations)
                .FirstOrDefault(u => u.Id == receiverId);

            return new MessageInputModel
            {
                ReceiverProfilePicurePath = receiver.ProfilePicturePath,
                ReceiverFirstName = receiver.FirstName,
                ReceiverId = receiver.Id,
                ReceiverLastName = receiver.LastName,
            };
        }

        public async Task<Message> SaveMessageAsync(MessageInputModel model, string senderId)
        {
            var receiver = this.userRepo
                .All()
                .Include(u => u.Conversations)
                .FirstOrDefault(u => u.Id == model.ReceiverId);

            var sender = this.userRepo
                .All()
                .Include(u => u.Conversations)
                .FirstOrDefault(u => u.Id == senderId);

            if (sender == null || receiver == null)
            {
                throw new NullReferenceException();
            }

            // validate sender and receiver
            if (sender.Conversations.Any(c => c.Id == sender.Id + receiver.Id || c.Id == receiver.Id + senderId) == false)
            {
                var conversationSender = new Conversation
                {
                    Id = receiver.Id + sender.Id,
                    CreatedOn = DateTime.UtcNow,
                    ApplicationUserId = sender.Id,
                    InterlocutorId = receiver.Id,
                };

                var conversationReceiver = new Conversation
                {
                    Id = sender.Id + receiver.Id,
                    CreatedOn = DateTime.UtcNow,
                    ApplicationUserId = receiver.Id,
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

            var receiverConvrs = receiver.Conversations
                .FirstOrDefault(c => c.Id == sender.Id + receiver.Id || c.Id == receiver.Id + senderId);

            var senderConvrs = sender.Conversations
                .FirstOrDefault(c => c.Id == sender.Id + receiver.Id || c.Id == receiver.Id + senderId);

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

            return message;
        }

        private void UpdateConversation(Conversation conversation, Message message, ApplicationUser user)
        {
            conversation.LastMessageContent = message.Content;
            conversation.LastMessageDate = message.CreatedOn;
            conversation.UserProfilePicturePath = user.ProfilePicturePath;
            conversation.ModifiedOn = message.CreatedOn;
            conversation.UserEmail = user.Email;
            conversation.UserFirstName = user.FirstName;
            conversation.UserLastName = user.LastName;
        }
    }
}
