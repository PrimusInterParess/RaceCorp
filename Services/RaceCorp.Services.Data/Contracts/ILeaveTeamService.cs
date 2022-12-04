namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;

    public interface ILeaveTeamService
    {
        Task LeaveTeamAsync(RequestInputModel inputModel);
    }
}
