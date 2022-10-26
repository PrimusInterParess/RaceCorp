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
    using RaceCorp.Web.ViewModels.Trace;

    using static RaceCorp.Services.Constants.Drive;
    using static RaceCorp.Services.Constants.Messages;

    public class TraceController : BaseController
    {
        private readonly ITraceService traceService;
        private readonly IDifficultyService difficultyService;
        private readonly IRaceService raceService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public TraceController(
            ITraceService traceService,
            IDifficultyService difficultyService,
            IRaceService raceService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.traceService = traceService;
            this.difficultyService = difficultyService;
            this.raceService = raceService;
            this.environment = environment;
            this.userManager = userManager;
        }

        public IActionResult RaceTraceProfile(int raceId, int traceId)
        {
            try
            {
                var model = this.traceService
                    .GetRaceTraceProfileViewModel(raceId, traceId);

                return this.View(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Profile", "Race", new { id = raceId });
            }
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
                var user = await this.userManager
                .GetUserAsync(this.User);

                await this.traceService
                    .EditAsync(
                    model,
                    $"{this.environment.WebRootPath}\\Gpx",
                    user.Id,
                    $"{this.environment.WebRootPath}\\Credentials\\{ServiceAccountKeyFileName}");
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);

                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();

                return this.View(model);
            }

            this.TempData["Message"] = "Your trace was successfully edited!";

            return this.RedirectToAction("RaceTraceProfile", new { raceId = model.RaceId, traceId = model.Id });
        }

        [HttpGet]
        [Authorize]

        public IActionResult Create(int raceId)
        {
            var isRaceIdValid = this.raceService.ValidateId(raceId);

            if (isRaceIdValid == false)
            {
                this.TempData["Message"] = IvalidOperationMessage;
                return this.RedirectToAction("All", "Race", nameof(RaceController));
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

            var user = await this.userManager
                .GetUserAsync(this.User);

            await this.traceService.CreateRaceTraceAsync(
                model,
                $"{this.environment.WebRootPath}\\Gpx",
                user.Id,
                $"{this.environment.WebRootPath}\\Credentials\\{ServiceAccountKeyFileName}");

            this.TempData["Message"] = "Trace was successfully created!";

            return this.RedirectToAction("Profile", "Race", new { id = model.RaceId });
        }

        public async Task<IActionResult> DeleteRaceTrace(int traceId, int raceId)
        {
            var isDeleted = await this.traceService.DeleteTraceAsync(traceId);

            if (isDeleted)
            {
                this.TempData["MessageDeleted"] = "Trace was successfully deleted!";

                return this.RedirectToAction("Race", "Profile", new { id = raceId });
            }

            return this.RedirectToAction("ErrorPage", "Home");
        }
    }
}
