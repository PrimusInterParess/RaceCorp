namespace RaceCorp.Data.Models
{
    using System;

    using RaceCorp.Data.Common.Models;

    public class ApplicationUserRide : BaseDeletableModel<int>
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int RideId { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
