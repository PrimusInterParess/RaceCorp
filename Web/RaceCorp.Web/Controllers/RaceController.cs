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

    public class RaceController : Controller
    {
        private const int ItemsPerPage = 12;

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
            var model = new RaceCreateInputViewModel()
            {
                Formats = this.formatsList.GetFormatKVP(),
                DifficultiesKVP = this.difficultyService.GetDifficultiesKVP(),
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(RaceCreateInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Formats = this.formatsList.GetFormatKVP();
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.raceService.CreateAsync(model, $"{this.environment.WebRootPath}/images", user.Id);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                model.Formats = this.formatsList.GetFormatKVP();
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            // TODO: Make alert message for successfully added race!
            this.TempData["Message"] = "Your race was successfully created!";
            return this.RedirectToAction(nameof(RaceController.All));
        }


        public IActionResult Profile(int id)
        {
            var model = this.raceService.GetRaceById(id);

            if (model == null)
            {
                return this.RedirectToAction(nameof(RaceController.All));
            }

            return this.View(model);
        }


        public IActionResult TraceProfile(int traceId)
        {
            return this.View();
        }

        public IActionResult All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var races = new RaceAllViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                RacesCount = this.raceService.GetCount(),
                Races = this.raceService.All(id, ItemsPerPage),
            };

            return this.View(races);
        }
    }
}
