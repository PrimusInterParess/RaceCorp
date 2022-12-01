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

        TeamProfileViewModel GetProfileById(string id, string currentUserId);

        Task EditAsync(TeamEditViewModel inputModel, string roothPath);

        Task RemoveUserAsync(string teamId, string memberId);

        List<T> GetTeamMembers<T>(string teamId);
    }
}
