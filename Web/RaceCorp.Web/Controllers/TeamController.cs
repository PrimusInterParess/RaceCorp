namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Request;
    using RaceCorp.Web.ViewModels.Team;

    public class TeamController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITeamService teamService;
        private readonly IJoinTeamService joinTeamService;
        private readonly ILeaveTeamService leaveTeamService;
        private readonly IWebHostEnvironment environment;

        public TeamController(
            UserManager<ApplicationUser> userManager,
            ITeamService teamService,
            IJoinTeamService joinTeamService,
            ILeaveTeamService leaveTeamService,
            IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.teamService = teamService;
            this.joinTeamService = joinTeamService;
            this.leaveTeamService = leaveTeamService;
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
                this.ModelState.AddModelError(string.Empty, e.Message);
                return this.View(inputModel);
            }

            return this.RedirectToAction("All");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string teamId, string teamOwnerId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) ||
                user.Id == teamOwnerId)
            {
                var teamDto = this.teamService.ById<TeamEditViewModel>(teamId);

                return this.View(teamDto);
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(TeamEditViewModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) ||
                (user != null && user.Id == inputModel.ApplicationUserId))
            {
                try
                {
                    await this.teamService.EditAsync(inputModel, this.environment.WebRootPath);
                    return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = inputModel.Id });
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(ArgumentException))
                    {
                        this.TempData["ErrorMessage"] = GlobalErrorMessages.InvalidRequest;

                        return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
                    }

                    this.ModelState.AddModelError(string.Empty, e.Message);
                    return this.View(inputModel);
                }
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RemoveTeamMember(string teamId, string teamOwnerId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) ||
            (user != null && user.Id == teamOwnerId))
            {
                var teamMembers = this.teamService.ById<TeamRemoveMemberModel>(teamId);
                teamMembers.CurrentUserIsOwner = teamMembers.ApplicationUserId == user.Id;

                return this.View(teamMembers);
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveTeamMember(string teamId, string memberId, string teamOwnerId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) ||
            (user != null && user.Id == teamOwnerId && memberId != teamOwnerId))
            {
                try
                {
                    await this.teamService.RemoveUserAsync(teamId, memberId);
                    this.TempData["ErrorMessage"] = GlobalConstants.RemovedTeamMember;
                    return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = teamId });
                }
                catch (Exception)
                {
                    this.TempData["ErrorMessage"] = GlobalErrorMessages.InvalidRequest;

                    return this.RedirectToAction("Profile", "Team", new { area = string.Empty });
                }
            }

            return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
        }

        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            var model = this.teamService.All<TeamAllViewModel>();

            return this.View(model);
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> Profile(string id)
        {
            var currentUser = await this.userManager
              .GetUserAsync(this.User);

            if (currentUser == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            var teamDto = this.teamService.GetProfileById(id, currentUser.Id);

            return this.View(teamDto);
        }

        public async Task<IActionResult> Join(RequestInputModel model)
        {
            try
            {
                await this.joinTeamService.RequestJoinTeamAsync(model.TargetId, model.RequesterId);

                this.TempData["Joined"] = GlobalConstants.SuccessfulRequestJoin;

                return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ArgumentException))
                {
                    this.TempData["ErrorMessage"] = e.Message;

                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                this.TempData["ErrorMessage"] = e.Message;

                return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
            }
        }

        public async Task<IActionResult> Leave(RequestInputModel model)
        {
            try
            {
                await this.leaveTeamService.LeaveTeamAsync(model);

                this.TempData["TeamLeave"] = GlobalConstants.SuccessfulTeamLeave;

                return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    this.TempData["ErrorMessage"] = e.Message;

                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Profile", "Team", new { area = string.Empty, id = model.TargetId });
            }
        }



    }
}
