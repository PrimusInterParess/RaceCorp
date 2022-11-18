namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
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
        public IActionResult Requests(string id)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null || currentUserId != id)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var userDto = this.userService.GetById<UserAllRequestsViewModel>(id);

            // find a better way to order it
            userDto.Requests.OrderBy(r => r.CreatedOn);
            return this.View(userDto);
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userDto = this.userService.GetById<UserProfileViewModel>(id);

            if (currentUserId != null && userDto != null)
            {
                userDto.IsConnected = userDto.Connections.Any(c => c.Id == currentUserId) || currentUserId == id;

                userDto.RequestedConnection = userDto.ConnectRequest.Any(r => r.RequesterId == currentUserId);

                userDto.CanMessageMe = userDto.Connections.Any(c => c.Id == currentUserId);

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

        [HttpGet]
        [Authorize]
        public IActionResult Inbox(string id)
        {
            var model = this.userService.GetByIdUserInboxViewModel(id);

            return this.View(model);
        }

        public IActionResult All()
        {
            var allUsers = this.userService.GetAllAsync<UserAllViewModel>();

            return this.View(allUsers);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SendMessage(string receiverId)
        {
            var currentUser = await this.userManager
               .GetUserAsync(this.User);

            if (currentUser == null)
            {
                this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var model = await this.userService.GetMessageModelAsync(receiverId, currentUser.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage(MessageInputModel model)
        {
            var currentUser = await this.userManager
               .GetUserAsync(this.User);

            if (currentUser == null)
            {
                this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                await this.userService.SaveMessageAsync(model, currentUser.Id);

            }
            catch (Exception)
            {
                this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.RedirectToAction("Profile", "User", new { area = string.Empty, id = model.ReceiverId });
        }
    }
}
