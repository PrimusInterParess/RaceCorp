﻿namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.ApplicationUsers;
    using RaceCorp.Web.ViewModels.Common;

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

        [HttpGet]
        public IActionResult Profile(string id)
        {
            var userDto =  this.userService.GetById<UserProfileViewModel>(id);
            if (userDto != null)
            {
                return this.View(userDto);
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
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
    }
}
