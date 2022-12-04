namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;

    public class ConnectionController : BaseController
    {
        private readonly IConnectUserService connectUserService;
        private readonly IDisconnectUserService disconnectUserService;

        public ConnectionController(
            IConnectUserService connectUserService,
            IDisconnectUserService disconnectUserService)
        {
            this.connectUserService = connectUserService;
            this.disconnectUserService = disconnectUserService;
        }

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
    }
}
