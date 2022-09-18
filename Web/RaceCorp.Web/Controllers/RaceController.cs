namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class RaceController : Controller
    {
        private readonly IFormatServices formatsList;
        private readonly IDifficultyService difficultyService;
        private readonly ICreateRaceService createRaceService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public RaceController(
            IFormatServices formatsList,
            IDifficultyService difficultyService,
            ICreateRaceService createRaceService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.formatsList = formatsList;
            this.difficultyService = difficultyService;
            this.createRaceService = createRaceService;
            this.environment = environment;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddRaceInputViewModel()
            {
                Formats = this.formatsList.GetFormatKVP(),
                DifficultiesKVP = this.difficultyService.GetDifficultiesKVP(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRaceInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Formats = this.formatsList.GetFormatKVP();

                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.createRaceService.CreateAsync(model, $"{this.environment.WebRootPath}/images", user.Id);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                model.Formats = this.formatsList.GetFormatKVP();
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            // TODO: Make alert message for successfully added race!
            // this.TempData["Message"];
            return this.RedirectToAction(nameof(RaceController.AllRaces));
        }

        public IActionResult RaceProfile(int raceId)
        {
            return this.View();
        }

        public IActionResult AllRaces()
        {
            return this.View();
        }
    }
}
