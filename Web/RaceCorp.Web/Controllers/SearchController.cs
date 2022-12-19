namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Data.Models.Enums;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Search;

    public class SearchController : BaseController
    {
        private readonly ISearchService searchService;
        private readonly UserManager<ApplicationUser> userManager;

        public SearchController(
            ISearchService searchService,
            UserManager<ApplicationUser> userManager)
        {
            this.searchService = searchService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Search(SearchInputModel inputModel)
        {
            if (string.IsNullOrEmpty(inputModel.QueryInput) == false ||
                string.IsNullOrWhiteSpace(inputModel.QueryInput) == false)
            {
                var action = ((SearchCategory)Enum.Parse(typeof(SearchCategory), inputModel.Area)).ToString();

                return this.RedirectToAction($"{action}Search", new { input = inputModel.QueryInput });
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserSearch(string input)
        {
            var currentUser = await this.userManager
            .GetUserAsync(this.User);

            if (currentUser == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var result = this.searchService.GetUsers(input, currentUser.Id);

            if (result.Count == 0)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.View(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult RaceSearch(string input)
        {
            var result = this.searchService.GetRaces<RaceSearchViewModel>(input);

            if (result.Count == 0)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.View(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult RideSearch(string input)
        {
            var result = this.searchService.GetRides<RideSearchViewModel>(input);

            if (result.Count == 0)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.View(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult TownSearch(string input)
        {
            var result = this.searchService.GetTowns<TownSearchViewModel>(input);

            if (result.Count == 0)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.View(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult MountainSearch(string input)
        {
            var result = this.searchService.GetMountains<MountainSearchViewModel>(input);

            if (result.Count == 0)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.View(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult TeamSearch(string input)
        {
            var result = this.searchService.GetTeams<TeamSearchViewModel>(input);

            if (result.Count == 0)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.View(result);
        }
    }
}
