namespace RaceCorp.Web.Api
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.Controllers;
    using RaceCorp.Web.ViewModels.Message;

    [ApiController]
    public class MessageController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IMessageService messageService;
        private readonly IUserService userService;

        public MessageController(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IMessageService messageService,
            IUserService userService)
        {
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.messageService = messageService;
            this.userService = userService;
        }

        [HttpGet]
        [Route("api/message/messages")]
        public async Task<IActionResult> Messages(string authorId, string interlocutorId)
        {
            var currentUser = await this.userManager
                    .GetUserAsync(this.User);

            var interlocutorEmail = this.userService.GetUserEmail(interlocutorId);

            if (interlocutorEmail == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            if (currentUser == null || currentUser.Id != authorId)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var messages = await this.messageService.GetMessages<MessageInListViewModel>(authorId, interlocutorId);

            if (messages.Count == 0)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var authorEmail = currentUser.Email;

            return this.Json(new
            {
                authorEmail = authorEmail,
                interlocutorEmail = interlocutorEmail,
                messages = messages,
            });
        }
    }
}
