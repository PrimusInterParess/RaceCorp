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
    using RaceCorp.Web.ViewModels.Request;
    using RaceCorp.Web.ViewModels.User;

    public class ConnectionController : BaseController
    {
        private readonly IConnectUserService connectUserService;
        private readonly IDisconnectUserService disconnectUserService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public ConnectionController(
            IConnectUserService connectUserService,
            IDisconnectUserService disconnectUserService,
            IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            this.connectUserService = connectUserService;
            this.disconnectUserService = disconnectUserService;
            this.userService = userService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Connect(RequestInputModel model)
        {
            try
            {
                await this.connectUserService.RequestConnectUserAsync(model.RequesterId, model.TargetId);

                this.TempData["Connect"] = GlobalConstants.SuccessfulRequestConnect;

                return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.TargetId });
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    this.TempData["ErrorMessage"] = e.Message;

                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.TargetId });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Diconnect(RequestInputModel model)
        {
            try
            {
                await this.disconnectUserService.DisconnectUserAsync(model);

                this.TempData["Disconnect"] = GlobalConstants.SuccessfulDisconnect;

                return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.TargetId });
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    this.TempData["ErrorMessage"] = e.Message;

                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.TargetId });
            }
        }

        [Authorize]
        public async Task<IActionResult> All(string userId)
        {
            var currentUser = await this.userManager
               .GetUserAsync(this.User);

            if (currentUser == null || currentUser.Id != userId)
            {
                this.TempData["ErrorMessage"] = GlobalErrorMessages.InvalidRequest;
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                var model = this.userService.GetById<UserAllConnectionsViewModel>(userId);
                return this.View(model);
            }
            catch (Exception)
            {
                this.TempData["ErrorMessage"] = GlobalErrorMessages.UnauthorizedRequest;
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }
        }
    }
}
