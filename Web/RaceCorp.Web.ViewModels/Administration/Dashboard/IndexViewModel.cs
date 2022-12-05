namespace RaceCorp.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    using RaceCorp.Data.Models;

    public class IndexViewModel
    {
        public ICollection<Race> NoOwnerRaces { get; set; }

        public ICollection<Ride> NoOwnerRides { get; set; }
    }
}
