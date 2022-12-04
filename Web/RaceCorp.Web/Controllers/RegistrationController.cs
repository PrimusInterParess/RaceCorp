namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Services.Data;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.EventRegister;

    public class RegistrationController : BaseController
    {
        private readonly IRegisterUserRideService registerUserRideService;
        private readonly IRegisterUserRaceService registerUserRaceService;
        private readonly IUnregisterUserRideService unregisterUserRideService;
        private readonly IUnregisterUserRaceService unregisterUserRaceService;

        public RegistrationController(
            IRegisterUserRideService registerUserRideService,
            IRegisterUserRaceService registerUserRaceService,
            IUnregisterUserRideService unregisterUserRideService,
            IUnregisterUserRaceService unregisterUserRaceService)
        {
            this.registerUserRideService = registerUserRideService;
            this.registerUserRaceService = registerUserRaceService;
            this.unregisterUserRideService = unregisterUserRideService;
            this.unregisterUserRaceService = unregisterUserRaceService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterRide(EventRegisterModel eventModel)
        {
            try
            {
                await this.registerUserRideService.RegisterUserRide(eventModel);

                this.TempData["Registered"] = GlobalConstants.RegisteredMessage;

                return this.RedirectToAction("Profile", "Ride", new { id = eventModel.Id });
            }
            catch (Exception e)
            {
                this.TempData["CannotParticipate"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    this.TempData["ErrorMessage"] = e.Message;

                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", "Ride", new { id = eventModel.Id });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterRace(EventRegisterModel eventModel)
        {
            try
            {
                await this.registerUserRaceService.RegisterUserRace(eventModel);

                this.TempData["Registered"] = GlobalConstants.RegisteredMessage;

                return this.RedirectToAction("Profile", "Race", new { id = eventModel.Id });
            }
            catch (Exception e)
            {
                this.TempData["CannotParticipate"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    this.TempData["ErrorMessage"] = e.Message;

                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", "Race", new { id = eventModel.Id });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnregisterRide(EventRegisterModel eventModel)
        {
            try
            {
                await this.unregisterUserRideService.UnregisterUserRide(eventModel);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", "Ride", new { id = eventModel.Id });
            }

            this.TempData["Unregistered"] = GlobalConstants.UregisteredMessage;

            return this.RedirectToAction("Profile", "Ride", new { id = eventModel.Id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnregisterRace(EventRegisterModel eventModel)
        {
            try
            {
                await this.unregisterUserRaceService.UnregisterUserRace(eventModel);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", "Race", new { id = eventModel.Id });
            }

            this.TempData["Unregistered"] = GlobalConstants.UregisteredMessage;

            return this.RedirectToAction("Profile", "Race", new { id = eventModel.Id });
        }
    }
}
