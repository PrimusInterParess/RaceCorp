namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;
    using RaceCorp.Web.ViewModels.Request;

    public interface ILeaveTeamService
    {
        Task LeaveTeamAsync(RequestInputModel inputModel);
    }
}
