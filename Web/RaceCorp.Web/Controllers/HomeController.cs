namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using RaceCorp.Services.Data;
    using RaceCorp.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly IGetIndexPageCategoriesList getCattegoryListService;

        public HomeController(IGetIndexPageCategoriesList getCattegoryListService)
        {
            this.getCattegoryListService = getCattegoryListService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexViewModel = this.getCattegoryListService.GetCategories();

            return this.View(indexViewModel);
        }

        [HttpPost]
        public IActionResult Index(string inputData)
        {
            var indexViewModel = this.getCattegoryListService.GetCategories();

            return this.View(indexViewModel);
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
    }
}
