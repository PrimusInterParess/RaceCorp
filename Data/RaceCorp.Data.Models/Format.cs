namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;

    public class Format : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Race> Races { get; set; } = new HashSet<Race>();

        public virtual ICollection<Ride> Rides { get; set; } = new HashSet<Ride>();
    }
}
