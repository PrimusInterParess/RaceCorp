namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.EventRegister;

    public interface IUnregisterUserRaceService
    {
        Task UnregisterUserRace(EventRegisterModel eventModel);
    }
}
