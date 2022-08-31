namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;

    public class Race : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string TrackUrl { get; set; }

        public TimeSpan ControlTime { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();

        public virtual ICollection<RaceDifficulty> Difficulties { get; set; } = new HashSet<RaceDifficulty>();

        public virtual ICollection<RaceFormat> Formats { get; set; } = new HashSet<RaceFormat>();
    }
}
