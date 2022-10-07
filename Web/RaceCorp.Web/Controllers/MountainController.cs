namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using RaceCorp.Web.ViewModels.Town;
    using RaceCorp.Web.ViewModels.Mountain;

    public class MountainController : BaseController
    {

        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMountanService mountanService;

        public MountainController(
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            IMountanService mountanService)
        {
            this.environment = environment;
            this.userManager = userManager;
            this.mountanService = mountanService;
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
                await this.mountanService.SaveImageAsync(model, user.Id, $"{this.environment.WebRootPath}/images");
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
    }
}
