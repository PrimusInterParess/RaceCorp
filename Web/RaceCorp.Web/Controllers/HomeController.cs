namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.Google;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels;
    using RaceCorp.Web.ViewModels.CommonViewModels;

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexViewModel = this.homeService.GetCategories();

            return this.View(indexViewModel);
        }

        // [HttpPost]
        //  public IActionResult Index(IndexViewModel model)
        //  {
        //    var homeAllViewModel = this.homeService
        //        .GetAll(
        //        model.TownId,
        //        model.MountainId,
        //        model.FormatId,
        //        model.DifficultyId);

        // return this.RedirectToAction(nameof(HomeController.All), nameof(HomeController), new { model= homeAllViewModel });
        //  }
        [HttpGet]
        public IActionResult All(HomeAllViewModel model)
        {
            return this.View(model);
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


        public IActionResult ErrorPage()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
