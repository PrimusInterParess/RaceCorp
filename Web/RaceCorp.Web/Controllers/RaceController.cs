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
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    using static RaceCorp.Services.Constants.Drive;

    using static RaceCorp.Services.Constants.Messages;

    public class RaceController : BaseController
    {
        private const int ItemsPerPage = 2;

        private readonly IFormatServices formatsList;
        private readonly IDifficultyService difficultyService;
        private readonly IRaceService raceService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public RaceController(
            IFormatServices formatsList,
            IDifficultyService difficultyService,
            IRaceService raceService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.formatsList = formatsList;
            this.difficultyService = difficultyService;
            this.raceService = raceService;
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
                    $"{this.environment.WebRootPath}/images",
                    user.Id,
                    $"{this.environment.WebRootPath}\\Gpx",
                    $"{this.environment.WebRootPath}\\Credentials\\{ServiceAccountKeyFileName}");
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
                await this.raceService.EditAsync(model, $"{this.environment.WebRootPath}/images", user.Id);
            }
            catch (Exception)
            {

                throw;
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

        [HttpGet]
        [Authorize]

        public IActionResult UploadPicture()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> UploadPicture(PictureUploadModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.raceService.SaveImageAsync(model, user.Id, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                return this.View(model);
            }

            this.TempData["Message"] = "Your picture was successfully added!";

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult UpcomingRace()
        {
            return this.View();
        }

        [HttpGet]
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
    }
}
