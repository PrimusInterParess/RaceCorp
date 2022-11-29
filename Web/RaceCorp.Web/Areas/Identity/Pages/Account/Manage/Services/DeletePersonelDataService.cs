namespace RaceCorp.Web.Areas.Identity.Pages.Account.Manage.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Manage.Services.Contracts;

    public class DeletePersonelDataService : IDeletePersonelDataService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Conversation> conversationRepo;
        private readonly IDeletableEntityRepository<Connection> connectionRepo;
        private readonly IDeletableEntityRepository<Message> messageRepo;
        private readonly IDeletableEntityRepository<Request> requestRepo;

        public DeletePersonelDataService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Conversation> conversationRepo,
            IDeletableEntityRepository<Connection> connectionRepo,
            IDeletableEntityRepository<Message> messageRepo,
            IDeletableEntityRepository<Request> requestRepo)
        {
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.conversationRepo = conversationRepo;
            this.connectionRepo = connectionRepo;
            this.messageRepo = messageRepo;
            this.requestRepo = requestRepo;
        }

        public async Task DeleteUser(string userId)
        {
            var user = this.userRepo
                .All()
                .Include(u => u.Logins)
                .Include(u => u.Connections)
                .Include(u => u.Conversations)
                .Include(u => u.InboxMessages)
                .Include(u => u.SentMessages)
                .FirstOrDefault(u => u.Id == userId);

            await this.ReasignUserFromRoles(user);
            await this.DeleteClaimsFromUser(user);
            await this.DeleteUserExternalLogins(user);
            this.DeleteUserConnections(user);
            this.DeleteConversations(user);
            this.DeleteMessages(user);
            this.DeleteRequests(user);
        }

        private void DeleteRequests(ApplicationUser user)
        {
            var requests = this.requestRepo
                .AllWithDeleted()
                .Where(r => r.TargetUserId == user.Id || r.RequesterId == user.Id).ToList();

            foreach (var request in requests)
            {
                this.requestRepo.HardDelete(request);
            }
        }

        private void DeleteMessages(ApplicationUser user)
        {
            var inboxMessages = user.InboxMessages;
            var sendMessages = user.SentMessages;

            foreach (var message in inboxMessages)
            {
                this.messageRepo.HardDelete(message);
            }

            foreach (var message in sendMessages)
            {
                this.messageRepo.HardDelete(message);
            }
        }

        private void DeleteConversations(ApplicationUser user)
        {
            var conversations = user.Conversations;
            var interluctorConversations = this.conversationRepo.AllWithDeleted().Where(c => c.InterlocutorId == user.Id).ToList();

            foreach (var conversation in conversations)
            {
                this.conversationRepo.HardDelete(conversation);
            }

            foreach (var interluctorConversation in interluctorConversations)
            {
                this.conversationRepo.HardDelete(interluctorConversation);
            }
        }

        private void DeleteUserConnections(ApplicationUser user)
        {
            var connections = user.Connections;

            var interluctorConnections = this.connectionRepo.AllWithDeleted().Where(c => c.InterlocutorId == user.Id).ToList();

            foreach (var connection in connections)
            {
                this.connectionRepo.HardDelete(connection);
            }

            foreach (var interluctorConnection in interluctorConnections)
            {
                this.connectionRepo.HardDelete(interluctorConnection);
            }
        }

        private async Task DeleteUserExternalLogins(ApplicationUser user)
        {
            var userLogins = user.Logins;

            foreach (var login in userLogins)
            {
                await this.userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
            }
        }

        private async Task ReasignUserFromRoles(ApplicationUser user)
        {
            var userRoles = await this.userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                await this.userManager.RemoveFromRoleAsync(user, role);
            }
        }

        private async Task DeleteClaimsFromUser(ApplicationUser user)
        {
            var userClaims = await this.userManager.GetClaimsAsync(user);

            foreach (var claim in userClaims)
            {
                await this.userManager.RemoveClaimAsync(user, claim);
            }
        }
    }
}
