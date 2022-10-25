using System;

namespace RaceCorp.Data.Models
{
    public class ApplicationUserRide
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int RideId { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
