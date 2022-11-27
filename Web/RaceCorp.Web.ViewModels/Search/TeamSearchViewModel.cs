namespace RaceCorp.Web.ViewModels.Search
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class TeamSearchViewModel : IMapFrom<Team>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
