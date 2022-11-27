namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models.Enums;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Search;
    using System;

    public class SearchController : BaseController
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {

            this.searchService = searchService;
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

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult UserSearch(string input)
        {
            var result = this.searchService.GetUsers<UserSearchViewModel>(input);

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
