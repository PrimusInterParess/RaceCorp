namespace RaceCorp.Web.ViewModels.Search
{
    using System.Collections.Generic;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Race;
    using RaceCorp.Web.ViewModels.Ride;

    public class MountainSearchViewModel : IMapFrom<Mountain>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<RaceIdNameViewModel> Races { get; set; }

        public IEnumerable<RideIdNameViewModel> Rides { get; set; }
    }
}
