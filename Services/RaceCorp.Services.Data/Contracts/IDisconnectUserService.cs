namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;
    using RaceCorp.Web.ViewModels.Request;

    public interface IDisconnectUserService
    {
        Task DisconnectUserAsync(RequestInputModel inputModel);
    }
}
