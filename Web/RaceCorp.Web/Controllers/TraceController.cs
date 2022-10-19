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
        private readonly IRaceTraceService raceDiffService;
        private readonly IDifficultyService difficultyService;
        private readonly IRaceService raceService;

        public TraceController(
            IRaceTraceService raceDiffService,
            IDifficultyService difficultyService,
            IRaceService raceService)
        {
            this.raceDiffService = raceDiffService;
            this.difficultyService = difficultyService;
            this.raceService = raceService;
        }

        public IActionResult RaceTraceProfile(int raceId, int traceId)
        {
            var model = this.raceDiffService.GetRaceDifficultyProfileViewModel(raceId, traceId);

            this.ViewData["id"] = $"Race id ={raceId}. Trace id = {traceId}";
            return this.View(model);
        }

        [HttpGet]
        [Authorize]

        public IActionResult EditRaceTrace(int raceId, int traceId)
        {
            var model = this.raceDiffService.GetById<RaceTraceEditModel>(raceId, traceId);
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
                await this.raceDiffService.EditAsync(model);
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

            await this.raceDiffService.CreateAsync(model);

            return this.RedirectToAction(nameof(RaceController.Profile), new { id = model.RaceId });
        }

        private string DriveUploadWithConversion(string filePath)
        {
            try
            {
                /* Load pre-authorized user credentials from the environment.
                 TODO(developer) - See https://developers.google.com/identity for
                 guides on implementing OAuth2 for your application. */
                GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                    .CreateScoped(DriveService.Scope.Drive);

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Drive API Snippets"
                });

                // Upload file My Report on drive.
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = "My Report",
                    MimeType = "application/vnd.google-apps.spreadsheet"
                };
                FilesResource.CreateMediaUpload request;
                // Create a new drive.
                using (var stream = new FileStream(filePath,
                           FileMode.Open))
                {
                    // Create a new file, with metadata and stream.
                    request = service.Files.Create(
                        fileMetadata, stream, "text/csv");
                    request.Fields = "id";
                    request.Upload();
                }

                var file = request.ResponseBody;
                // Prints the uploaded file id.
                Console.WriteLine("File ID: " + file.Id);
                return file.Id;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is FileNotFoundException)
                {
                    Console.WriteLine("File not found");
                }
                else
                {
                    throw;
                }
            }

            return null;
        }
    }
}
