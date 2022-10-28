namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.Town;

    public interface ITownService
    {
        IEnumerable<KeyValuePair<string, string>> GetTownsKVP();

        List<T> GetAll<T>();

        TownRidesProfileViewModel AllRides(int townId, int pageId, int itemsPerPage = 3);

        TownRacesProfileViewModel AllRaces(int townId, int pageId, int itemsPerPage = 3);

        Task<Town> ProccesingData(string name);
    }
}
