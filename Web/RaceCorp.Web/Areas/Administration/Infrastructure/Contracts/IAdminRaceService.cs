namespace RaceCorp.Web.Areas.Administration.Infrastructure.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.Areas.Administration.Models.Race;

    public interface IAdminRaceService
    {
        List<RaceIndexPageModel> GetNoOwnerRaces();

        List<RaceAllDashboardModel> GetAllRaces();

        Task DeleteRace(int id);

        Task UndeleteRace(int id);
    }
}
