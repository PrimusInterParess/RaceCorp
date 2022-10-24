namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Ride;

    public interface IRideService
    {
        Task CreateAsync(RideCreateViewModel model, string gxpFileRoothPath, string userId, string pathToServiceAccountKeyFile);

        Task EditAsync(RideEditVIewModel model);

        RideAllViewModel All(int page, int itemsPerPage = 3);

        T GetById<T>(int id);

        Task<bool> DeleteAsync(int id);
    }
}
