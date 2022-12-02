namespace RaceCorp.Web.ViewModels.Ride
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RideInAllViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TraceMapUrl { get; set; }

        public string TownName { get; set; }

        public string MountainName { get; set; }

        public string TraceStartTime { get; set; }
    }
}
