namespace RaceCorp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;
    using RaceCorp.Web.Areas.Administration.Models;
    using RaceCorp.Web.Controllers;

    using static RaceCorp.Services.Constants.Common;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdministrationController : BaseController
    {
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminFileService adminFileService;

        public AdministrationController(
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            IAdminFileService adminFileService)
        {
            this.environment = environment;
            this.userManager = userManager;
            this.adminFileService = adminFileService;
        }

        [HttpGet]
        public IActionResult UploadPicture()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPictureAsync(PictureUploadModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.adminFileService.ProccessingImageData(inputModel.Picture, inputModel.Type, user.Id, this.environment.WebRootPath, SystemImageFolderName);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);

                return this.View();
            }

            this.TempData["Message"] = "Your picture was successfully added!";

            return this.RedirectToAction("Index", "Home", new { area = " " });

            // return this.View();
        }
    }
}
