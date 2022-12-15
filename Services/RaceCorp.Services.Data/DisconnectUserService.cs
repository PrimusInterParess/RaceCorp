namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Request;

    public class DisconnectUserService : IDisconnectUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Connection> connectionRepo;
        private readonly IDeletableEntityRepository<Conversation> conversationRepo;

        public DisconnectUserService(
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Connection> connectionRepo,
            IDeletableEntityRepository<Conversation> conversationRepo)
        {
            this.userRepo = userRepo;
            this.connectionRepo = connectionRepo;
            this.conversationRepo = conversationRepo;
        }

        public async Task DisconnectUserAsync(RequestInputModel inputModel)
        {
            var requester = this.userRepo
               .All()
               .Include(u => u.Connections)
               .Include(u => u.Requests)
               .Include(c => c.Conversations)
               .FirstOrDefault(u => u.Id == inputModel.RequesterId);

            if (requester == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var targetUser = this.userRepo
                .All()
                .Include(u => u.Connections)
                .Include(u => u.Requests)
                .Include(c => c.Conversations)
                .FirstOrDefault(u => u.Id == inputModel.TargetId);

            if (targetUser == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var requesterConnectRequest = requester.Requests
                .FirstOrDefault(
                r => r.RequesterId == targetUser.Id &&
                r.Type == GlobalConstants.RequestTypeConnectUser);

            if (requesterConnectRequest != null)
            {
                requester.Requests.Remove(requesterConnectRequest);
            }

            var targetUserConnectRequest = targetUser.Requests
               .FirstOrDefault(
                r => r.RequesterId == requester.Id &&
               r.Type == GlobalConstants.RequestTypeConnectUser);

            if (targetUserConnectRequest != null)
            {
                targetUser.Requests.Remove(targetUserConnectRequest);
            }

            var requesterConnection = requester.Connections
                .FirstOrDefault(
                c => c.Id == requester.Id + targetUser.Id ||
                c.Id == targetUser.Id + requester.Id);

            if (requesterConnection != null)
            {

                this.connectionRepo.Delete(requesterConnection);
            }

            var targetUserConnection = targetUser.Connections
              .FirstOrDefault(
              c => c.Id == requester.Id + targetUser.Id ||
              c.Id == targetUser.Id + requester.Id);

            if (targetUserConnection != null)
            {
                this.connectionRepo.Delete(targetUserConnection);
            }

            var requesterConversation = requester.Conversations
                .FirstOrDefault(
                c => c.Id == requester.Id + targetUser.Id ||
                c.Id == targetUser.Id + requester.Id);

            if (requesterConversation != null)
            {
                this.conversationRepo.Delete(requesterConversation);
            }

            var targetUserConversation = targetUser.Conversations
                .FirstOrDefault(
                c => c.Id == requester.Id + targetUser.Id ||
                c.Id == targetUser.Id + requester.Id);

            if (targetUserConversation != null)
            {
                this.conversationRepo.Delete(targetUserConversation);
            }

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
        }
    }
}
