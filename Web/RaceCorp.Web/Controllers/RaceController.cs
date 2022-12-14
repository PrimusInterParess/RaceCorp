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
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class RaceController : BaseController
    {
        private readonly IFormatServices formatsList;
        private readonly IDifficultyService difficultyService;
        private readonly IRaceService raceService;
        private readonly IApprovalService eventService;
        private readonly IGpxService gpxService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public RaceController(
            IFormatServices formatsList,
            IDifficultyService difficultyService,
            IRaceService raceService,
            IApprovalService eventService,
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

            if (user == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                await this.raceService.CreateAsync(
                    model,
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

        public async Task<IActionResult> Profile(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var model = this.raceService.GetById<RaceProfileViewModel>(id);

            if (model == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            this.raceService.UpdateInfo(model, user);


            return this.View(model);
        }

        [HttpGet]
        [Authorize]

        public IActionResult Edit(int id)
        {
            var model = this.raceService.GetById<RaceEditViewModel>(id);

            if (model == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

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
                    user.Id);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                model.Formats = this.formatsList.GetFormatKVP();
                return this.View(model);
            }

            this.TempData["Message"] = "Your race was successfully edited!";
            return this.RedirectToAction(nameof(RaceController.Profile), new { id = model.Id });
        }

        public IActionResult All(int pageId = 1)
        {
            if (pageId <= 0)
            {
                return this.NotFound();
            }

            var races = this.raceService.All(pageId);
            return this.View(races);
        }

        public IActionResult UpcomingRaces(int pageId = 1)
        {
            if (pageId <= 0)
            {
                return this.NotFound();
            }

            var races = this.raceService.GetUpcomingRaces(pageId);
            return this.View(races);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.raceService.DeleteAsync(id);

            if (isDeleted)
            {
                this.TempData["MessageDeleted"] = "Your race was successfully deleted!";

                return this.RedirectToAction("All", "Race");
            }

            return this.RedirectToAction("ErrorPage", "Home");
        }
    }
}
