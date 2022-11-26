namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;

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

            this.searchService.GetSearchResults(inputModel);

            return this.RedirectToAction("/");

        }
    }
}
