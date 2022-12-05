namespace RaceCorp.Web.Areas.Administration.Infrastructure.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.Areas.Administration.Models.Race;
    using RaceCorp.Web.Areas.Administration.Models.Ride;

    public interface IAdminRideService
    {
        List<RideIndexPageModel> GetNoOwnerRides();

        List<RideAllDashboardModel> GetAllRides();

        Task DeleteRide(int id);

        Task UndeleteRide(int id);
    }
}
