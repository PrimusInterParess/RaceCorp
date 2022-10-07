namespace RaceCorp.Web.ViewModels.Race
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RaceIdNameViewModel : IMapFrom<Race>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
