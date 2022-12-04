namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.EventRegister;

    public interface IApprovalService
    {
        Task ProccesApproval(ApproveRequestModel inputModel);
    }
}
