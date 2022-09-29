namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Ride;

    public interface IRideService
    {
        Task CreateAsync(RideCreateViewModel model, string userId);

        RideAllViewModel All(int page, int itemsPerPage = 3);

    }
}
