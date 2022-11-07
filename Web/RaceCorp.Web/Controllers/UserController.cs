namespace RaceCorp.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.ApplicationUsers;

    using RaceCorp.Web.ViewModels.Common;

    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(
            IUserService userService,
            IFileService fileService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.fileService = fileService;
            this.environment = environment;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {
            var userDto = this.userService.GetById<UserProfileViewModel>(id);
            return this.View(userDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateTeam()
        {
            var user = await this.userManager
                .GetUserAsync(this.User);

            var model = new TeamCreateBaseModel
            {
                CreatorId = user.Id,
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public IActionResult CreateTeam(TeamCreateBaseModel inputModel)
        {
            return this.View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userManager
               .GetUserAsync(this.User);

            if (user.Id != id)
            {
                this.RedirectToAction("ErrorPage", "Home", new { area = " " });
            }

            var userEditModel = this.userService.GetById<UserEditViewModel>(id);

            return this.View(userEditModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(UserEditViewModel inputModel)
        {
            var user = await this.userManager
               .GetUserAsync(this.User);

            await this.userService.EditAsync(inputModel, this.environment.WebRootPath, this.User);
            return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = inputModel.Id });
        }
    }
}
