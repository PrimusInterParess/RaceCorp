namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Location> Locations { get; set; } = new HashSet<Location>();
    }
}
