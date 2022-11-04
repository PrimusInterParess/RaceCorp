namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Ride;

    public interface IRideService
    {
        Task CreateAsync(RideCreateViewModel model, string roothPath, string userId);

        Task EditAsync(RideEditVIewModel model, string roothPath, string userId);

        RideAllViewModel All(int page, int itemsPerPage = 3);

        RideAllViewModel GetUpcomingRides(int page, int itemsPerPage = 3);

        Task<bool> Unregister(int id, string userId);

        T GetById<T>(int id);

        Task<bool> DeleteAsync(int id);

        Task<bool> RegisterUserToRide(int id, string userId);
    }
}
