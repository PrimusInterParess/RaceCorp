
namespace RaceCorp.Web.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;

    public class ChatHub : Hub
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ChatGroupName> chatGroupNameRepo;

        public ChatHub(
            IDeletableEntityRepository<ApplicationUser> userRepo,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ChatGroupName> chatGroupNameRepo)
        {
            this.userRepo = userRepo;
            this.userManager = userManager;
            this.chatGroupNameRepo = chatGroupNameRepo;
        }

        public override Task OnConnectedAsync()
        {
            var userName = this.Context.User.Identity.Name;

            this.Groups.AddToGroupAsync(this.Context.ConnectionId, this.Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public async Task JoinGroup(string receiverId, string authorId)
        {
            // save group name if doesnt exists

            var chatgroupNam = this.chatGroupNameRepo.All().FirstOrDefault(g => g.Name == receiverId + authorId || g.Name == authorId + receiverId);

            if (chatgroupNam != null)
            {
                chatgroupNam = new ChatGroupName
                {
                    Name = authorId + receiverId,
                };

            }

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, chatgroupNam.Name);
        }

        public async Task SendMessage(string user, string message)
        {
            await this.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            // create sendMessage

            return this.Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
