namespace RaceCorp.Web.Areas.Administration.Infrastructure.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.Areas.Administration.Models.User;

    public interface IAdminUserService
    {
        List<UserAllDashboardModel> GetAllUsers();

        Task DeleteUser(string id);

        Task UndeleteUser(string id);
    }
}
