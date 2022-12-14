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
    using RaceCorp.Web.ViewModels.Trace;

    using static RaceCorp.Services.Constants.Messages;

    public class TraceController : BaseController
    {
        private readonly ITraceService traceService;
        private readonly IDifficultyService difficultyService;
        private readonly IRaceService raceService;
        private readonly IGpxService gpxService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public TraceController(
            ITraceService traceService,
            IDifficultyService difficultyService,
            IRaceService raceService,
            IGpxService gpxService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.traceService = traceService;
            this.difficultyService = difficultyService;
            this.raceService = raceService;
            this.gpxService = gpxService;
            this.environment = environment;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> RaceTraceProfile(int raceId, int traceId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var model = this.traceService.GetById<RaceTraceProfileModel>(raceId, traceId);

            this.traceService.UpdateInfo(model, user);

            if (model == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.View(model);
        }

        [HttpGet]
        [Authorize]

        public IActionResult EditRaceTrace(int raceId, int traceId)
        {
            var model = this.traceService.GetById<RaceTraceEditModel>(raceId, traceId);

            if (model == null)
            {
                this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

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
                    user.Id);
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
        [DisplayName("Create Trace")]

        public IActionResult CreateRaceTrace(int raceId)
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

        public async Task<IActionResult> CreateRaceTrace(RaceTraceEditModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var user = await this.userManager
                .GetUserAsync(this.User);

            try
            {
                await this.traceService.CreateRaceTraceAsync(
               model,
               user.Id);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                return this.View(model);
            }

            this.TempData["Message"] = "Trace was successfully created!";

            return this.RedirectToAction("Profile", "Race", new { id = model.RaceId });
        }

        [DisplayName("Delete Trace")]

        public async Task<IActionResult> DeleteRaceTrace(int traceId, int raceId)
        {
            var isDeleted = await this.traceService.DeleteTraceAsync(traceId);

            if (isDeleted)
            {
                this.TempData["MessageDeleted"] = "Trace was successfully deleted!";

                return this.RedirectToAction("Profile", "Race", new { id = raceId });
            }

            return this.RedirectToAction("ErrorPage", "Home");
        }

        [DisplayName("Download Gpx")]
        public IActionResult DownloadGpx(string id)
        {
            try
            {
                var gpxFile = this.gpxService.GetGpxById(id);
                var gpxFilePath = $"{this.environment.WebRootPath}/{gpxFile.ParentFolderName}/{gpxFile.ChildFolderName}/{gpxFile.Id}.{gpxFile.Extension}";
                byte[] fileBytes = System.IO.File.ReadAllBytes(gpxFilePath);
                string fileName = $"{id}.{gpxFile.Extension}";
                return this.File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception)
            {
                return this.RedirectToAction("ErrorPage", "Home");
            }
        }
    }
}
