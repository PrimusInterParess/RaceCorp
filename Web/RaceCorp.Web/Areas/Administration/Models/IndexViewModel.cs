namespace RaceCorp.Web.Areas.Administration.Models
{
    using System.Collections.Generic;

    using RaceCorp.Web.Areas.Administration.Models.Race;
    using RaceCorp.Web.Areas.Administration.Models.Ride;

    public class DashboardIndexViewModel
    {
        public ICollection<RaceIndexPageModel> NoOwnerRaces { get; set; }

        public ICollection<RideIndexPageModel> NoOwnerRides { get; set; }
    }
}
