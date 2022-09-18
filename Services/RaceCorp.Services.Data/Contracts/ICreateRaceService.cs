namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.RaceViewModels;

    public interface ICreateRaceService
    {
        Task CreateAsync(AddRaceInputViewModel model, string imagePath, string userId);
    }
}
