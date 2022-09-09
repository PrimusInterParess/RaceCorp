namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class RaceController : Controller
    {
        private readonly IFormatServices formatsList;
        private readonly IDifficultyService difficultyService;
        private readonly ICreateRaceService createRaceService;

        public RaceController(IFormatServices formatsList, IDifficultyService difficultyService, ICreateRaceService createRaceService)
        {
            this.formatsList = formatsList;
            this.difficultyService = difficultyService;
            this.createRaceService = createRaceService;
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

            await this.createRaceService.CreateAsync(model);

            // TODO:Redirect to ProfilePage;
            return this.RedirectToAction(nameof(this.RaceProfile));
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
