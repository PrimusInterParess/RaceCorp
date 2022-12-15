namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Message;

    public class MessageController : BaseController
    {
        private readonly IMessageService messageService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public MessageController(
            IMessageService messageService,
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.messageService = messageService;
            this.userManager = userManager;
            this.userService = userService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Inbox(string id)
        {
            try
            {
                var model = this.messageService.GetByIdUserInboxViewModel(id);
                return this.View(model);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SendMessage(string receiverId)
        {
            var currentUser = await this.userManager
               .GetUserAsync(this.User);

            if (currentUser == null)
            {
                this.TempData["ErrorMessage"] = GlobalErrorMessages.UnauthorizedRequest;

                this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var model = this.messageService.GetMessageModelAsync(receiverId, currentUser.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage(MessageInputModel model)
        {
            var currentUser = await this.userManager
               .GetUserAsync(this.User);

            if (currentUser == null)
            {
                this.TempData["ErrorMessage"] = GlobalErrorMessages.UnauthorizedRequest;

                this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                await this.messageService.SaveMessageAsync(model, currentUser.Id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.ReceiverId });
        }
    }
}
