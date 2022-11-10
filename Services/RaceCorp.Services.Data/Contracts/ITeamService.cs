namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Team;

    public interface ITeamService
    {
        Task CreateAsync(TeamCreateBaseModel inputMode, string roothPath);

        List<T> All<T>();

        T ById<T>(string id);

        bool RequestJoin(string teamId, string userId);
    }
}
