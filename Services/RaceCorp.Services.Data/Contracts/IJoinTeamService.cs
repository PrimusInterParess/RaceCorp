using System.Threading.Tasks;

namespace RaceCorp.Services.Data.Contracts
{
    public interface IJoinTeamService
    {
        Task RequestJoinTeamAsync(string teamId, string requesterId);
    }
}
