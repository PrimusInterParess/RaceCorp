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
                var registering = await this.eventService.RegisterUserEvent(eventModel);

                if (registering)
                {
                    this.TempData["Registered"] = "Your are now registered!";

                    return this.RedirectToAction("Profile", eventModel.EventType, new { id = eventModel.Id });
                }

            }
            catch (Exception e)
            {
                this.TempData["CannotParticipate"] = e.Message;
            }

            return this.RedirectToAction("ErrorPage", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Unregister(EventRegisterModel eventModel)
        {
            var isRemoved = await this.eventService.Unregister(eventModel);

            if (isRemoved)
            {
                this.TempData["Unregistered"] = "Your are unregistered!!";

                return this.RedirectToAction("Profile", eventModel.EventType, new { id = eventModel.Id });
            }

            return this.RedirectToAction("ErrorPage", "Home");
        }

        public IActionResult Messages(string id)
        {
            return this.View();
        }

        public async Task<IActionResult> Request(RequestInputModel model)
        {
            bool hasPassed = false;

            try
            {
                hasPassed = await this.eventService.JoinTeamAsync(model.TargetId, model.RequesterId);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;
                return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
            }

            if (hasPassed)
            {
                this.TempData["Joined"] = GlobalConstants.SuccessfulRequest;

                return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> ProcessRequest(int requestId, string userId)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null || currentUserId != userId)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var isApproved = await this.eventService.ProcessRequestAsync(requestId, userId);

            return this.RedirectToAction("Requests", "User", new { area = string.Empty, id = userId });
        }
    }
}
