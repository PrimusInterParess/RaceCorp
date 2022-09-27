namespace RaceCorp.Web.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    using static RaceCorp.Services.Constants.Messages;

    public class RaceDifficultyController : BaseController
    {
        private readonly IRaceDifficultyService raceDiffService;
        private readonly IDifficultyService difficultyService;
        private readonly IRaceService raceService;

        public RaceDifficultyController(
            IRaceDifficultyService raceDiffService,
            IDifficultyService difficultyService,
            IRaceService raceService)
        {
            this.raceDiffService = raceDiffService;
            this.difficultyService = difficultyService;
            this.raceService = raceService;
        }

        public IActionResult RaceDifficultyProfile(int raceId, int traceId)
        {
            var model = this.raceDiffService.GetRaceDifficultyProfileViewModel(raceId, traceId);

            this.ViewData["id"] = $"Race id ={raceId}. Trace id = {traceId}";
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Edit(int raceId, int traceId)
        {
            var model = this.raceDiffService.GetById<RaceDifficultyInputViewModel>(raceId, traceId);
            model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RaceDifficultyInputViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            try
            {
                await this.raceDiffService.EditAsync(model);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(String.Empty, IvalidOperation);
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();

                return this.View(model);
            }

            return this.RedirectToAction(nameof(this.RaceDifficultyProfile), new { raceId = model.RaceId, traceId = model.Id });
        }

        [HttpGet]
        public IActionResult Create(int raceId)
        {
            var isRaceIdValid = this.raceService.ValidateId(raceId);

            if (isRaceIdValid == false)
            {
                this.TempData["Message"] = IvalidOperation;
                return this.RedirectToAction(nameof(RaceController.All), nameof(RaceController));
            }

            var model = new RaceDifficultyInputViewModel()
            {
                RaceId = raceId,
            };

            model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RaceDifficultyInputViewModel model)
        {

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.raceDiffService.CreateAsync(model);

            return this.RedirectToAction(nameof(RaceController.Profile), nameof(RaceController), new { id = model.RaceId });
        }
    }
}
