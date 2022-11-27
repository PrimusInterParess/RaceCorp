namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.Search;

    public interface ISearchService
    {

        List<T> GetUsers<T>(string query);

        List<T> GetRaces<T>(string query);

        List<T> GetRides<T>(string query);

        List<T> GetTowns<T>(string query);

        List<T> GetTeams<T>(string query);

        List<T> GetMountains<T>(string query);
    }
}
