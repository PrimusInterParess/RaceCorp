namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.Ride;
    using RaceCorp.Web.ViewModels.Town;

    public interface ITownService
    {
        IEnumerable<KeyValuePair<string, string>> GetTownsKVP();

        List<T> GetAll<T>();

        Task Create(TownCreateViewModel model);

        TownRidesProfileViewModel AllRides(int townId, int pageId, int itemsPerPage = 3);

        TownRacesProfileViewModel AllRaces(int townId, int pageId, int itemsPerPage = 3);
    }
}
