namespace RaceCorp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.Areas.Administration.Models;
    using RaceCorp.Web.Controllers;

    using static RaceCorp.Services.Constants.Common;

    // [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileService fileService;

        public AdministrationController(
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            IFileService fileService)
        {
            this.environment = environment;
            this.userManager = userManager;
            this.fileService = fileService;
        }

        [HttpGet]
        public IActionResult UploadPicture(string type)
        {
            var model = new PictureUploadModel()
            {
                Type = type,
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPictureAsync(PictureUploadModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);

                // return this.View(model);
            }

            this.TempData["Message"] = "Your picture was successfully added!";

            return this.RedirectToAction("Index", "Home");

            // return this.View();
        }
    }
}
