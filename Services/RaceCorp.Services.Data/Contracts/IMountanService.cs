namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.Mountain;
    using RaceCorp.Web.ViewModels.Town;

    public interface IMountanService
    {
        HashSet<MountainViewModel> GetMountains();

        List<T> GetAll<T>();

        IEnumerable<KeyValuePair<string, string>> GetMountainsKVP();

        MountainRidesProfileViewModel AllRides(int mountainId, int pageId, int itemsPerPage = 3);

        MountainRacesProfileViewModel AllRaces(int mountainId, int pageId, int itemsPerPage = 3);

        Task<Mountain> ProccesingData(string name);

    }
}
