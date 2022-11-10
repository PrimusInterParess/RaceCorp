namespace RaceCorp.Web.ViewModels.Team
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class TeamAllViewModel : IMapFrom<Team>
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string LogoImagePath { get; set; }
    }
}
