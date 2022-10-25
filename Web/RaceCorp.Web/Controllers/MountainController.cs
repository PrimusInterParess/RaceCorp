namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.Mountain;
    using RaceCorp.Web.ViewModels.Town;

    using static RaceCorp.Services.Constants.Common;

    public class MountainController : BaseController
    {
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMountanService mountanService;
        private readonly IImageService imageService;

        public MountainController(
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            IMountanService mountanService,
            IImageService imageService)
        {
            this.environment = environment;
            this.userManager = userManager;
            this.mountanService = mountanService;
            this.imageService = imageService;
        }

        public IActionResult ById(int id)
        {
            this.ViewData["id"] = $"Mountain id: {id}";
            return this.View();
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
                await this.imageService.SaveImageAsync(model, user.Id, $"{this.environment.WebRootPath}/images", MountainFolderName, MountainImageName);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                return this.View(model);
            }

            this.TempData["Message"] = "Your picture was successfully added!";

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult All()
        {
            var model = new MountainListViewModel()
            {
                Mountains = this.mountanService.GetAll<MountainRacesRidesViewModel>(),
            };

            return this.View(model);
        }

        public IActionResult ProfileRides(int mountainId, int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var rides = this.mountanService.AllRides(mountainId, id);
            return this.View(rides);
        }

        public IActionResult ProfileRaces(int mountainId, int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var races = this.mountanService.AllRaces(mountainId, id);
            return this.View(races);
        }
    }
}
