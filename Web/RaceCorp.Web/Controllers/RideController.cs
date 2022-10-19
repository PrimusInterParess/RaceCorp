namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;

    using RaceCorp.Web.ViewModels.Ride;
    using RaceCorp.Web.ViewModels.Trace;

    using static RaceCorp.Services.Constants.Messages;

    public class RideController : BaseController
    {
        private readonly IRideService rideService;
        private readonly IDifficultyService difficultyService;
        private readonly IFormatServices formatServices;
        private readonly UserManager<ApplicationUser> userManager;

        public RideController(
            IRideService rideService,
            IDifficultyService difficultyService,
            IFormatServices formatServices,
            UserManager<ApplicationUser> userManager)
        {
            this.rideService = rideService;
            this.difficultyService = difficultyService;
            this.formatServices = formatServices;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]

        public IActionResult Create()
        {
            var model = new RideCreateViewModel()
            {
                Date = DateTime.UtcNow,
                Formats = this.formatServices.GetFormatKVP(),
                Trace = new TraceInputModel()
                {
                    DifficultiesKVP = this.difficultyService.GetDifficultiesKVP(),
                },
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create(RideCreateViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                model.Date = DateTime.UtcNow;
                model.Formats = this.formatServices.GetFormatKVP();
                model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            try
            {
                await this.rideService.CreateAsync(model, user.Id);
            }
            catch (System.Exception)
            {
                this.ModelState.AddModelError(string.Empty, IvalidOperationMessage);
                model.Formats = this.formatServices.GetFormatKVP();
                model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            return this.RedirectToAction(nameof(RideController.All));
        }

        public IActionResult Profile(int id)
        {
            var model = this.rideService.GetById<RideProfileVIewModel>(id);

            return this.View(model);
        }

        public IActionResult All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var rides = this.rideService.All(id);
            return this.View(rides);
        }

        [HttpGet]
        [Authorize]

        public IActionResult Edit(int id)
        {
            var model = this.rideService.GetById<RideEditVIewModel>(id);
            model.Formats = this.formatServices.GetFormatKVP();
            model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Edit(RideEditVIewModel model)
        {

            try
            {
                await this.rideService.EditAsync(model);
            }
            catch (Exception e)
            {

                this.ModelState.AddModelError(string.Empty, e.Message);
                model.Formats = this.formatServices.GetFormatKVP();
                model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            return this.RedirectToAction(nameof(RideController.Profile), new { id = model.Id });
        }
    }
}
