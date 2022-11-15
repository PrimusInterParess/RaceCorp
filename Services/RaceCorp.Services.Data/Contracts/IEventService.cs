namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.EventRegister;

    public interface IEventService
    {
        Task<bool> RegisterUserEvent(EventRegisterModel eventModel);

        Task<bool> Unregister(EventRegisterModel eventModel);

        Task ProccesRequest(RequestInputModel inputModel);

        Task ProccesApproval(ApproveRequestModel inputModel);
    }
}
