namespace RaceCorp.Web.Areas.Identity.Pages.Account.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Infrastructure.Contracts;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Models;

    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ITownService townService;

        public RegisterService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ITownService townService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.townService = townService;
        }

        public async Task<IdentityResult> AssignUserToRole(string roleId, ApplicationUser user)
        {
            var applicationRole = await this.roleManager.FindByIdAsync(roleId);

            if (applicationRole != null)
            {
                try
                {
                    var result = await this.userManager.AddToRoleAsync(user, applicationRole.Name);

                    return result;
                }
                catch (System.Exception)
                {
                }
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Failed to register user to role" });
        }

        public async Task ProccesingData(RegisterModel.InputModel inputModel, ApplicationUser user)
        {
            var townDb = await this.townService.ReturnTown(inputModel.Town);

            try
            {
                user.FirstName = inputModel.FirstName;
                user.LastName = inputModel.LastName;
                user.Country = inputModel.Country;
                user.Town = townDb;
                user.Gender = Enum.Parse(typeof(Gender), inputModel.Gender.ToString()).ToString();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Invalid input data!");
            }

        }
    }
}
