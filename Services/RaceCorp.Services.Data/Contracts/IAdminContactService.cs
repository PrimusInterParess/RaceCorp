namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;

    public interface IAdminContactService
    {
        Task ReceiveMessage(ContactFormModel model);
    }
}
