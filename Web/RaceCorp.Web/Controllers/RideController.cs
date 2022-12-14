namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Ride;
    using RaceCorp.Web.ViewModels.Trace;

    using static RaceCorp.Services.Constants.Messages;

    public class RideController : BaseController
    {
        private readonly IRideService rideService;
        private readonly IApprovalService eventService;
        private readonly IDifficultyService difficultyService;
        private readonly IFormatServices formatServices;
        private readonly IGpxService gpxService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public RideController(
            IRideService rideService,
            IApprovalService eventService,
            IDifficultyService difficultyService,
            IFormatServices formatServices,
            IGpxService gpxService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.rideService = rideService;
            this.eventService = eventService;
            this.difficultyService = difficultyService;
            this.formatServices = formatServices;
            this.gpxService = gpxService;
            this.environment = environment;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]

        public IActionResult Create()
        {
            var model = new RideCreateViewModel()
            {
                Date = DateTime.UtcNow,
                Formats = this.formatServices.GetFormatKVP(),
                Trace = new TraceInputModel()
                {
                    DifficultiesKVP = this.difficultyService.GetDifficultiesKVP(),
                },
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create(RideCreateViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                model.Date = DateTime.UtcNow;
                model.Formats = this.formatServices.GetFormatKVP();
                model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            var user = await this.userManager
                .GetUserAsync(this.User);

            try
            {
                await this.rideService
                    .CreateAsync(
                    model,
                    user.Id);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                model.Formats = this.formatServices.GetFormatKVP();
                model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            this.TempData["Message"] = "Your ride was successfully created!";

            return this.RedirectToAction(nameof(RideController.All));
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var model = this.rideService.GetById<RideProfileVIewModel>(id);

            if (model == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var user = await this.userManager.GetUserAsync(this.User);
            this.rideService.UpdateInfo(model, user);

            return this.View(model);
        }

        public IActionResult All(int pageId = 1)
        {
            if (pageId <= 0)
            {
                return this.NotFound();
            }

            var rides = this.rideService.All(pageId);
            return this.View(rides);
        }

        [HttpGet]
        [Authorize]

        public IActionResult Edit(int id)
        {
            var model = this.rideService.GetById<RideEditVIewModel>(id);

            if (model == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            model.Formats = this.formatServices.GetFormatKVP();
            model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Edit(RideEditVIewModel model)
        {
            var user = await this.userManager
                    .GetUserAsync(this.User);

            try
            {
                await this.rideService.EditAsync(
                    model,
                    user.Id);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                model.Formats = this.formatServices.GetFormatKVP();
                model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            this.TempData["Message"] = "Your ride was successfully edited!";

            return this.RedirectToAction(nameof(RideController.Profile), new { id = model.Id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.rideService.DeleteAsync(id);

            if (isDeleted)
            {
                this.TempData["MessageDeleted"] = "Your ride was successfully deleted!";

                return this.RedirectToAction("All", "Ride");
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
        }

        public IActionResult UpcomingRides(int pageId = 1)
        {
            if (pageId <= 0)
            {
                return this.NotFound();
            }

            var rides = this.rideService.GetUpcomingRides(pageId);
            return this.View(rides);
        }
    }
}
