namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;

    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.Mountain;

    public interface IMountanService
    {
        HashSet<MountainViewModel> GetMountains();

        List<T> GetAll<T>();

        IEnumerable<KeyValuePair<string, string>> GetMountainsKVP();

        Task SaveImageAsync(PictureUploadModel model, string userId, string imagePath);
    }
}
