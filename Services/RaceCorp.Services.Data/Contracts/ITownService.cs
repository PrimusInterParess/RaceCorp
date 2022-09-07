namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.DifficultyViewModels;
    using RaceCorp.Web.ViewModels.HomeViewModels;

    public interface ITownService
    {
        HashSet<TownViewModel> GetTowns();

        IEnumerable<KeyValuePair<string, string>> GetTownsKVP();
    }
}
