namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.EventRegister;

    public interface IEventService
    {
        Task<bool> RegisterUserEvent(EventRegisterModel eventModel);

        Task<bool> Unregister(EventRegisterModel eventModel);
    }
}

