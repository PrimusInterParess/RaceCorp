namespace RaceCorp.Data.Models
{
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Models;

    public class Mountain : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Race> Races { get; set; } = new HashSet<Race>();

        public virtual ICollection<Ride> Rides { get; set; } = new HashSet<Ride>();
    }
}
