﻿namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Ride;
    using RaceCorp.Web.ViewModels.Trace;

    using static RaceCorp.Services.Constants.Drive;
    using static RaceCorp.Services.Constants.Messages;

    public class RideController : BaseController
    {
        private readonly IRideService rideService;
        private readonly IDifficultyService difficultyService;
        private readonly IFormatServices formatServices;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public RideController(
            IRideService rideService,
            IDifficultyService difficultyService,
            IFormatServices formatServices,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.rideService = rideService;
            this.difficultyService = difficultyService;
            this.formatServices = formatServices;
            this.environment = environment;
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

            var user = await this.userManager
                .GetUserAsync(this.User);

            try
            {
                await this.rideService
                    .CreateAsync(
                    model,
                    $"{this.environment.WebRootPath}\\Gpx",
                    user.Id,
                    $"{this.environment.WebRootPath}\\Credentials\\{ServiceAccountKeyFileName}");
            }
            catch (System.Exception)
            {
                this.ModelState.AddModelError(string.Empty, IvalidOperationMessage);
                model.Formats = this.formatServices.GetFormatKVP();
                model.Trace.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            this.TempData["Message"] = "Your ride was successfully created!";

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

            this.TempData["Message"] = "Your ride was successfully edited!";

            return this.RedirectToAction(nameof(RideController.Profile), new { id = model.Id });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.rideService.DeleteAsync(id);

            this.TempData["MessageDeleted"] = "Your ride was successfully deleted!";

            return this.RedirectToAction("All", "Ride");
        }
    }
}
