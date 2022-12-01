namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.Town;

    using static RaceCorp.Services.Constants.Common;

    public class TownController : BaseController
    {
        private readonly ITownService townService;

        public TownController(
            ITownService townService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.townService = townService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        public IActionResult ProfileRides(int modelId, int pageId = 1)
        {
            if (pageId <= 0)
            {
                return this.NotFound();
            }

            var rides = this.townService.AllRides(modelId, pageId);
            return this.View(rides);
        }

        public IActionResult ProfileRaces(int modelId, int pageId = 1)
        {
            if (pageId <= 0)
            {
                return this.NotFound();
            }

            var races = this.townService.AllRaces(modelId, pageId);
            return this.View(races);
        }

        [HttpGet]
        public IActionResult All()
        {
            var model = new TownListViewModel()
            {
                Towns = this.townService.GetAll<TownRacesRidesViewModel>(),
            };

            return this.View(model);
        }
    }
}
