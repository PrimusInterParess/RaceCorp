namespace RaceCorp.Web.Controllers
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.EventRegister;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    using static RaceCorp.Services.Constants.Common;

    public class RaceController : BaseController
    {
        private const int ItemsPerPage = 2;

        private readonly IFormatServices formatsList;
        private readonly IDifficultyService difficultyService;
        private readonly IRaceService raceService;
        private readonly IEventService eventService;
        private readonly IGpxService gpxService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public RaceController(
            IFormatServices formatsList,
            IDifficultyService difficultyService,
            IRaceService raceService,
            IEventService eventService,
            IGpxService gpxService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.formatsList = formatsList;
            this.difficultyService = difficultyService;
            this.raceService = raceService;
            this.eventService = eventService;
            this.gpxService = gpxService;
            this.environment = environment;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new RaceCreateModel()
            {
                Date = DateTime.UtcNow,
                Formats = this.formatsList.GetFormatKVP(),
                DifficultiesKVP = this.difficultyService.GetDifficultiesKVP(),
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(RaceCreateModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Date = DateTime.UtcNow;
                model.Formats = this.formatsList.GetFormatKVP();
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.raceService.CreateAsync(
                    model,
                    this.environment.WebRootPath,
                    user.Id);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                model.Formats = this.formatsList.GetFormatKVP();
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            this.TempData["Message"] = "Your race was successfully created!";
            return this.RedirectToAction(nameof(RaceController.All));
        }

        public IActionResult Profile(int id)
        {
            var model = this.raceService.GetById<RaceProfileViewModel>(id);

            if (model == null)
            {
                return this.RedirectToAction(nameof(RaceController.All));
            }

            return this.View(model);
        }

        [HttpGet]
        [Authorize]

        public IActionResult Edit(int id)
        {
            var model = this.raceService.GetById<RaceEditViewModel>(id);
            model.Formats = this.formatsList.GetFormatKVP();

            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Edit(RaceEditViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                model.Formats = this.formatsList.GetFormatKVP();
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.raceService.EditAsync(
                    model,
                    this.environment.WebRootPath,
                    user.Id);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(" ", e.Message);
                model.Formats = this.formatsList.GetFormatKVP();
                return this.View(model);
            }

            this.TempData["Message"] = "Your race was successfully edited!";

            return this.RedirectToAction(nameof(RaceController.Profile), new { id = model.Id });
        }

        public IActionResult All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var races = this.raceService.All(id);
            return this.View(races);
        }

        public IActionResult UpcomingRaces(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var races = this.raceService.GetUpcomingRaces(id);
            return this.View(races);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.raceService.DeleteAsync(id);

            if (isDeleted)
            {
                return this.RedirectToAction("All", "Race");
            }

            return this.RedirectToAction("ErrorPage", "Home");
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

                    return this.RedirectToAction("Profile", new { id = eventModel.Id });
                }

                return this.RedirectToAction("ErrorPage", "Home");
            }
            catch (Exception e)
            {
                this.TempData["CannotParticipate"] = e.Message;

                return this.RedirectToAction("Profile", new { id = eventModel.Id });
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

                return this.RedirectToAction("Profile", new { id = eventModel.Id });
            }

            return this.RedirectToAction("ErrorPage", "Home");
        }
    }
}
