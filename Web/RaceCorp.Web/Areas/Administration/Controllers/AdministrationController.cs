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
    using RaceCorp.Web.Areas.Administration.Models.Message;
    using RaceCorp.Web.Controllers;

    using static RaceCorp.Services.Constants.Common;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdministrationController : BaseController
    {
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminFileService adminFileService;
        private readonly IAdminService adminService;

        public AdministrationController(
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            IAdminFileService adminFileService,
            IAdminService adminService)
        {
            this.environment = environment;
            this.userManager = userManager;
            this.adminFileService = adminFileService;
            this.adminService = adminService;
        }

        [HttpGet]
        public IActionResult UploadPicture()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPicture(PictureUploadModel inputModel)
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

        [HttpGet]
        public IActionResult Messages()
        {
            var model = this.adminService.GetMessages();

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Message(int id)
        {
            try
            {
                var model = this.adminService.GetMessage(id);
                return this.View(model);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                return this.RedirectToAction("Index", "Dashboard", new { area = "Administration" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Message(MessageProfileModel inputModel)
        {
            try
            {
                await this.adminService.SaveReply(inputModel);

                return this.RedirectToAction("Messages", "Administration", new { area = "Administration" });

            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                return this.RedirectToAction("Index", "Dashboard", new { area = "Administration" });
            }
        }
    }
}
