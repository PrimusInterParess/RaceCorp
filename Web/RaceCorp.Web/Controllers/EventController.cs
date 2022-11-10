namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;
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

                return this.RedirectToAction("ErrorPage", "Home");
            }
            catch (Exception)
            {
                return this.RedirectToAction("ErrorPage", "Home");
            }
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
    }
}
