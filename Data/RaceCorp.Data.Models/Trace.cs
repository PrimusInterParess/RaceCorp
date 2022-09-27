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

        public TimeSpan ControlTime { get; set; }

        public string TrackUrl { get; set; }

        public DateTime StartTime { get; set; }

        public int DifficultyId { get; set; }

        public Difficulty Difficulty { get; set; }

        public int? RideId { get; set; }

        public Ride Ride { get; set; }

        public int? RaceId { get; set; }

        public Race Race { get; set; }
    }
}
