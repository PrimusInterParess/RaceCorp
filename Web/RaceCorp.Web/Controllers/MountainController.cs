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
    using RaceCorp.Web.ViewModels.Mountain;
    using RaceCorp.Web.ViewModels.Town;

    using static RaceCorp.Services.Constants.Common;

    public class MountainController : BaseController
    {
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMountanService mountanService;

        public MountainController(
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            IMountanService mountanService)
        {
            this.environment = environment;
            this.userManager = userManager;
            this.mountanService = mountanService;
        }

        public IActionResult ById(int id)
        {
            this.ViewData["id"] = $"Mountain id: {id}";
            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult UploadPicture()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult All()
        {
            var model = new MountainListViewModel()
            {
                Mountains = this.mountanService.GetAll<MountainRacesRidesViewModel>(),
            };

            return this.View(model);
        }

        public IActionResult ProfileRides(int modelId, int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var rides = this.mountanService.AllRides(modelId, id);
            return this.View(rides);
        }

        public IActionResult ProfileRaces(int modelId, int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var races = this.mountanService.AllRaces(modelId, id);
            return this.View(races);
        }
    }
}
