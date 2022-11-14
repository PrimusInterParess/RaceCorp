namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Team;

    public class TeamController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITeamService teamService;
        private readonly IWebHostEnvironment environment;

        public TeamController(
            UserManager<ApplicationUser> userManager,
            ITeamService teamService,
            IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.teamService = teamService;
            this.environment = environment;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await this.userManager
                .GetUserAsync(this.User);

            var model = new TeamCreateBaseModel
            {
                CreatorId = user.Id,
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create(TeamCreateBaseModel inputModel)
        {
            var user = await this.userManager
               .GetUserAsync(this.User);

            if (user == null || user.Id != inputModel.CreatorId)
            {
                this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                await this.teamService.CreateAsync(inputModel, this.environment.WebRootPath);
            }
            catch (System.Exception e)
            {
                this.TempData["AlreadyHaveTeam"] = e.Message;
            }

            return this.RedirectToAction("All");

        }

        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            var model = this.teamService.All<TeamAllViewModel>();

            return this.View(model);
        }

        public IActionResult Profile(string id)
        {
            var model = this.teamService.ById<TeamProfileViewModel>(id);

            if (model == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            return this.View(model);
        }

      
    }
}
