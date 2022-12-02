namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;
    using RaceCorp.Data.Models.Enums;

    public class Trace : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public int Length { get; set; }

        public string MapUrl { get; set; }

        public TimeSpan ControlTime { get; set; }

        public string GpxPath { get; set; }

        public virtual Gpx Gpx { get; set; }

        public string GpxId { get; set; }

        public DateTime StartTime { get; set; }

        public int DifficultyId { get; set; }

        public Difficulty Difficulty { get; set; }

        public int? RideId { get; set; }

        public virtual Ride Ride { get; set; }

        public int? RaceId { get; set; }

        public virtual Race Race { get; set; }

        public virtual ICollection<ApplicationUserTrace> RegisteredUsers { get; set; } = new HashSet<ApplicationUserTrace>();
    }
}
