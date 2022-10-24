namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;

    using RaceCorp.Web.ViewModels.RaceViewModels;

    public interface IRaceService
    {
        Task CreateAsync(RaceCreateModel model, string imagePath, string userId, string gxpFileRoothPath, string pathToServiceAccountKeyFile);

        RaceAllViewModel All(int page, int itemsPerPage = 3);

        int GetCount();

        T GetById<T>(int id);

        bool ValidateId(int id);

        Task EditAsync(RaceEditViewModel model, string logoPath, string userId);

        RaceInAllViewModel GetUpcommingEvents();

        Task SaveImageAsync(PictureUploadModel model, string userId, string imagePath);

        Task<bool> DeleteAsync(int id);
    }
}
