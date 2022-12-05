﻿namespace RaceCorp.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels;
    using RaceCorp.Web.ViewModels.User;

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;
        private readonly IUserService userService;

        public HomeController(IHomeService homeService, IUserService userService)
        {
            this.homeService = homeService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexViewModel = this.homeService.GetIndexModel();

            return this.View(indexViewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AllUsers()
        {
            var allUsers = this.userService.GetAllAsync<UserAllViewModel>();

            return this.View(allUsers);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorPage()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
