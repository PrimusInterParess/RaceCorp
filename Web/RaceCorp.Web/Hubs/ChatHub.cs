
namespace RaceCorp.Web.Hubs
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using NuGet.Protocol.Plugins;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;

    public class ChatHub : Hub
    {
        private readonly IGroupNameProvider groupNameProvider;

        public ChatHub(IGroupNameProvider groupNameProvider)
        {
            this.groupNameProvider = groupNameProvider;
        }

        public override Task OnConnectedAsync()
        {
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, this.Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return base.OnConnectedAsync();
        }

        public async Task JoinGroup(string receiverId)
        {
            var groupName = this.groupNameProvider.GetGroupName(receiverId, this.Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string user, string message)
        {
            await this.Clients.User(user).SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            var groupName = this.groupNameProvider.GetGroupName(receiver, sender);
            return this.Clients.Group(groupName).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
