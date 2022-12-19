namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.User;

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;
        private readonly IUserService userService;
        private readonly IAdminContactService adminContactService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IHomeService homeService,
            IUserService userService,
            IAdminContactService adminContactService,
            UserManager<ApplicationUser> userManager)
        {
            this.homeService = homeService;
            this.userService = userService;
            this.adminContactService = adminContactService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexViewModel = this.homeService.GetIndexModel();

            return this.View(indexViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AllUsers()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (currentUser == null)
            {
                return this.Redirect("/Identity/Account/Login");
            }

            var allUsers = this.userService.GetAllAsyncHomePage(currentUser.Id);

            return this.View(allUsers);
        }

        public IActionResult Privacy()
        {
            this.ViewData["Privacy"] = GlobalConstants.PrivacyPage;

            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            try
            {
                await this.adminContactService.ReceiveMessage(model);

                this.TempData["AdminContact"] = GlobalConstants.AdminMessageSend;

                return this.RedirectToAction("Index", "Home", new { area = string.Empty });
            }
            catch (Exception)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorPage()
        {
            return this.View();
        }
    }
}
