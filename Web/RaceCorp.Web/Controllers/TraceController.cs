namespace RaceCorp.Web.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Threading.Tasks;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Trace;

    using static RaceCorp.Services.Constants.Messages;

    public class TraceController : BaseController
    {
        private readonly ITraceService traceService;
        private readonly IDifficultyService difficultyService;
        private readonly IRaceService raceService;

        public TraceController(
            ITraceService traceService,
            IDifficultyService difficultyService,
            IRaceService raceService)
        {
            this.traceService = traceService;
            this.difficultyService = difficultyService;
            this.raceService = raceService;
        }

        public IActionResult RaceTraceProfile(int raceId, int traceId)
        {
            var model = this.traceService.GetRaceDifficultyProfileViewModel(raceId, traceId);

            this.ViewData["id"] = $"Race id ={raceId}. Trace id = {traceId}";
            return this.View(model);
        }

        [HttpGet]
        [Authorize]

        public IActionResult EditRaceTrace(int raceId, int traceId)
        {
            var model = this.traceService.GetById<RaceTraceEditModel>(raceId, traceId);
            model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> EditRaceTrace(RaceTraceEditModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            try
            {
                await this.traceService.EditAsync(model);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(String.Empty, IvalidOperationMessage);
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();

                return this.View(model);
            }

            return this.RedirectToAction(nameof(this.RaceTraceProfile), new { raceId = model.RaceId, traceId = model.Id });
        }

        [HttpGet]
        [Authorize]

        public IActionResult Create(int raceId)
        {
            var isRaceIdValid = this.raceService.ValidateId(raceId);

            if (isRaceIdValid == false)
            {
                this.TempData["Message"] = IvalidOperationMessage;
                return this.RedirectToAction(nameof(RaceController.All), nameof(RaceController));
            }

            var model = new RaceTraceEditModel()
            {
                RaceId = raceId,
            };

            model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create(RaceTraceEditModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.traceService.CreateAsync(model);

            return this.RedirectToAction(nameof(RaceController.Profile), new { id = model.RaceId });
        }
    }
}
