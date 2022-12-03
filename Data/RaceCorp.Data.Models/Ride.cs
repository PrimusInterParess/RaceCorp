namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;

    using RaceCorp.Data.Models.BaseModels;

    public class Ride : RideBaseModel
    {
        public int TraceId { get; set; }

        public virtual Trace Trace { get; set; }

        public virtual ICollection<ApplicationUserRide> RegisteredUsers { get; set; } = new HashSet<ApplicationUserRide>();
    }
}
