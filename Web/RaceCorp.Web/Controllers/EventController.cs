namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.EventRegister;

    public class EventController : BaseController
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register(EventRegisterModel eventModel)
        {
            try
            {
                await this.eventService.RegisterUserEvent(eventModel);

                this.TempData["Registered"] = GlobalConstants.RegisteredMessage;

                return this.RedirectToAction("Profile", eventModel.EventType, new { id = eventModel.Id });
            }
            catch (Exception e)
            {
                this.TempData["CannotParticipate"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    this.TempData["ErrorMessage"] = e.Message;

                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", eventModel.EventType, new { id = eventModel.Id });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Unregister(EventRegisterModel eventModel)
        {
            try
            {
                await this.eventService.Unregister(eventModel);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", eventModel.EventType, new { id = eventModel.Id });
            }

            this.TempData["Unregistered"] = GlobalConstants.UregisteredMessage;

            return this.RedirectToAction("Profile", eventModel.EventType, new { id = eventModel.Id });
        }

        [HttpPost]
        [Authorize]
        public new async Task<IActionResult> Request(RequestInputModel model)
        {
            try
            {
                await this.eventService.ProccesRequest(model);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                if (model.Type == GlobalConstants.RequestTypeTeamJoin)
                {
                    return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
                }
                else
                {
                    return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.TargetId });
                }
            }

            if (model.Type == GlobalConstants.RequestTypeTeamJoin)
            {
                this.TempData["Joined"] = GlobalConstants.SuccessfulRequestJoin;

                return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
            }
            else if (model.Type == GlobalConstants.RequestTypeConnectUser)
            {
                this.TempData["Connect"] = GlobalConstants.SuccessfulRequestConnect;
                return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.TargetId });
            }
            else if (model.Type == GlobalConstants.RequestTypeTeamLeave)
            {
                this.TempData["TeamLeave"] = GlobalConstants.SuccessfulTeamLeave;

                return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
            }
            else if (model.Type == GlobalConstants.RequestTypeDisconnectUser)
            {
                this.TempData["Disconnect"] = GlobalConstants.SuccessfulDisconnect;

                return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.TargetId });
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ApproveRequest(ApproveRequestModel model)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                await this.eventService.ProccesApproval(model);
                return this.RedirectToAction("Requests", "User", new { area = string.Empty, id = currentUserId });
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Requests", "User", new { area = string.Empty, id = currentUserId });
            }
        }
    }
}
