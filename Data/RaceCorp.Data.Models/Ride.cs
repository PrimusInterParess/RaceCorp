namespace RaceCorp.Data.Models
{
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Models;

    public class Ride : BaseRide
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }

        public int FormatId { get; set; }

        public virtual Format Format { get; set; }

        public ICollection<RideDifficulty> Traces { get; set; } = new HashSet<RideDifficulty>();
    }
}
