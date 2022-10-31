namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.ApplicationUsers;

    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {
            var userDto = this.userService.GetById<UserProfileViewModel>(id);
            return this.View(userDto);
        }
    }
}
