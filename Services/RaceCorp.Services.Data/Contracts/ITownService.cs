namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.HomeViewModels;

    public interface ITownService
    {
        HashSet<TownViewModel> GetTowns();

        IEnumerable<KeyValuePair<string, string>> GetTownsKVP();

        Task SaveImage(PictureUploadModel model, string userId, string imagePath);
    }
}
