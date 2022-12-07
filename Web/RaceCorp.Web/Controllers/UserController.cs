namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.User;

    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(
            IUserService userService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.environment = environment;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var currentUser = await this.userManager
               .GetUserAsync(this.User);

            if (currentUser == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                var userDto = this.userService.GetProfileModelById(id, currentUser.Id);
                return this.View(userDto);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userManager
               .GetUserAsync(this.User);

            if (user.Id != id)
            {
                this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
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

            if (this.ModelState.IsValid == false)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.userService.EditAsync(inputModel, this.environment.WebRootPath);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                return this.View(inputModel);
            }

            return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = inputModel.Id });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Requests(string id)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null ||
                currentUserId != id)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                var userDto = this.userService.GetRequestsModel(id);
                return this.View(userDto);
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }
        }
    }
}
