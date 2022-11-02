namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.ApplicationUsers;
    using System.Threading.Tasks;

    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;
        private readonly IWebHostEnvironment environment;

        public UserController(
            IUserService userService,
            IFileService fileService,
            IWebHostEnvironment environment)
        {
            this.userService = userService;
            this.fileService = fileService;
            this.environment = environment;
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {
            var userDto = this.userService.GetById<UserProfileViewModel>(id);
            return this.View(userDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(ApplicationUserProfilePictureUploadModel model)
        {

            if (model.ProfilePicture != null)
            {
                await this.userService.SaveProfileImage(model.ProfilePicture, model.UserId, this.environment.WebRootPath);

                return this.RedirectToAction("Profile", "User", new { id = model.UserId, area = "" });
            }

            return this.RedirectToAction("/");
        }
    }
}
