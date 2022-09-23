using System;
using System.Threading.Tasks;
using RaceCorp.Web.ViewModels.DifficultyViewModels;

namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;

    public class RaceDifficultyController : BaseController
    {
        private readonly IRaceDifficultyService raceDiffService;
        private readonly IDifficultyService difficultyService;

        public RaceDifficultyController(
            IRaceDifficultyService raceDiffService,
            IDifficultyService difficultyService)
        {
            this.raceDiffService = raceDiffService;
            this.difficultyService = difficultyService;
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
            var model = this.raceDiffService.GetById<RaceDifficultyEditViewModel>(raceId, traceId);
            model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RaceDifficultyEditViewModel model)
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
            catch (Exception e)
            {
                this.ModelState.AddModelError(String.Empty, "Invalid operation");
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();

                return this.View(model);
            }

            return this.RedirectToAction(nameof(this.RaceDifficultyProfile), new { raceId = model.RaceId, traceId = model.Id });
        }
    }
}
