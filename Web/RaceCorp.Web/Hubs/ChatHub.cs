﻿
namespace RaceCorp.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, this.Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await this.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {

            return this.Clients.User(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}