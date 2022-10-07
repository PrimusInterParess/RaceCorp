namespace RaceCorp.Web.ViewModels.Mountain
{
    using System.Collections.Generic;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Race;
    using RaceCorp.Web.ViewModels.Ride;

    public class MountainRacesRidesViewModel : MountainViewModel, IMapFrom<Mountain>
    {
        public IEnumerable<RaceIdNameViewModel> Races { get; set; }

        public IEnumerable<RideIdNameViewModel> Rides { get; set; }
    }
}
