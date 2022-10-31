namespace RaceCorp.Web.Areas.Identity.Pages.Account.Infrastructure.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using RaceCorp.Data.Models;
    using RaceCorp.Web.Areas.Identity.Pages.Account;

    public interface IRegisterService
    {
        Task AssignUserToRole(string roleName, ApplicationUser user);

        Task<ApplicationRole> ValidateRole(string roleId);

        Task ProccesingData(RegisterModel.InputModel inputModel, ApplicationUser user);
    }
}
