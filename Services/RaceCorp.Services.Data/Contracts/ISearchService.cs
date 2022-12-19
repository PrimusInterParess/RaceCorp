namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.Search;
    using RaceCorp.Web.ViewModels.User;

    public interface ISearchService
    {

        List<UserAllViewModel> GetUsers(string query, string currentUserId);

        List<T> GetRaces<T>(string query);

        List<T> GetRides<T>(string query);

        List<T> GetTowns<T>(string query);

        List<T> GetTeams<T>(string query);

        List<T> GetMountains<T>(string query);
    }
}
