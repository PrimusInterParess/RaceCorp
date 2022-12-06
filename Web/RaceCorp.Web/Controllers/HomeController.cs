namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.User;

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;
        private readonly IUserService userService;
        private readonly IAdminContactService adminContactService;

        public HomeController(
            IHomeService homeService,
            IUserService userService,
            IAdminContactService adminContactService)
        {
            this.homeService = homeService;
            this.userService = userService;
            this.adminContactService = adminContactService;
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
