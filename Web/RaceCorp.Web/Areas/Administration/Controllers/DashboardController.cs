namespace RaceCorp.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;
    using RaceCorp.Web.Controllers;

    using System.Threading.Tasks;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]

    public class DashboardController : BaseController
    {
        private readonly IAdminRideService adminRideService;
        private readonly IAdminUserService adminUserService;
        private readonly IAdminRaceService adminRaceService;
        private readonly IAdminService adminService;

        public DashboardController(
            IAdminService adminService,
            IAdminRaceService adminRaceService,
            IAdminRideService adminRideService,
            IAdminUserService adminUserService)
        {
            this.adminService = adminService;
            this.adminRaceService = adminRaceService;
            this.adminRideService = adminRideService;
            this.adminUserService = adminUserService;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            var model = this.adminService.GetIndexModel();
            return this.View(model);
        }

        [Authorize]

        public IActionResult AllRaces()
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            var model = this.adminRaceService.GetAllRaces();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteRace(int id)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            await this.adminRaceService.DeleteRace(id);
            return this.RedirectToAction("AllRaces", "Dashboard", new { area = "Administration" });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UndeleteRace(int id)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            await this.adminRaceService.UndeleteRace(id);
            return this.RedirectToAction("AllRaces", "Dashboard", new { area = "Administration" });
        }

        [Authorize]

        public IActionResult AllRides()
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            var model = this.adminRideService.GetAllRides();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteRide(int id)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            await this.adminRideService.DeleteRide(id);
            return this.RedirectToAction("AllRides", "Dashboard", new { area = "Administration" });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UndeleteRide(int id)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            await this.adminRideService.UndeleteRide(id);
            return this.RedirectToAction("AllRides", "Dashboard", new { area = "Administration" });
        }

        [Authorize]
        public IActionResult AllUsers()
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            var model = this.adminUserService.GetAllUsers();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            await this.adminUserService.DeleteUser(id);
            return this.RedirectToAction("AllUsers", "Dashboard", new { area = "Administration" });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UndeleteUser(string id)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) == false)
            {
                return this.Unauthorized();
            }

            await this.adminUserService.UndeleteUser(id);
            return this.RedirectToAction("AllUsers", "Dashboard", new { area = "Administration" });
        }
    }
}
