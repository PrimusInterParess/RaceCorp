namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.RaceViewModels;

    public interface IRaceService
    {
        Task CreateAsync(RaceCreateInputViewModel model, string imagePath, string userId);

        List<RaceViewModel> All(int page, int itemsPerPage = 3);

        int GetCount();

        RaceProfileViewModel GetRaceById(int id);
    }
}
