namespace RaceCorp.Web.Hubs
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Message;

    public class ChatHub : Hub
    {
        private readonly IGroupNameProvider groupNameProvider;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IMessageService messageService;

        public ChatHub(
            IGroupNameProvider groupNameProvider,
            IDeletableEntityRepository<Data.Models.Message> messageRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IUserService userService,
            IMessageService messageService)
        {
            this.groupNameProvider = groupNameProvider;
            this.userRepo = userRepo;
            this.messageService = messageService;
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

        public async Task SendMessageToGroup(string receiverId, string message)
        {
            var senderId = this.Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var groupName = this.groupNameProvider.GetGroupName(receiverId, senderId);

            var receiver = this.userRepo.AllAsNoTracking().FirstOrDefault(u => u.Id == receiverId);
            var sender = this.userRepo.AllAsNoTracking().FirstOrDefault(u => u.Id == senderId);

            var messageInputModel = new MessageInputModel
            {
                Content = message,
                ReceiverId = receiverId,
                ReceiverFirstName = receiver.FirstName,
                ReceiverLastName = receiver.LastName,
                ReceiverProfilePicurePath = receiver.ProfilePicturePath,
            };

            if (senderId != receiverId)
            {
                var task = this.messageService.SaveMessageAsync(messageInputModel, senderId);

                var messageDb = task.Result;

                var result = new
                {
                    SenderId = senderId,
                    Content = message,
                    ReceiverId = receiverId,
                    SenderProfilePicurePath = sender.ProfilePicturePath,
                    CreatedOn = messageDb.CreatedOn.ToString(GlobalConstants.DateMessageFormat),
                };

                await this.Clients.Group(groupName).SendAsync("ReceiveMessage", result);
            }
        }
    }
}
