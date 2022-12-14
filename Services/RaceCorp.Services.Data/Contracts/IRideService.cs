namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.Ride;

    public interface IRideService
    {
        Task CreateAsync(RideCreateViewModel model, string userId);

        Task EditAsync(RideEditVIewModel model, string userId);

        RideAllViewModel All(int page, int itemsPerPage = 3);

        RideAllViewModel GetUpcomingRides(int page, int itemsPerPage = 3);

        T GetById<T>(int id);

        Task<bool> DeleteAsync(int id);

        void UpdateInfo(RideProfileVIewModel rideModel,ApplicationUser user);
    }
}
