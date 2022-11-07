namespace RaceCorp.Data.Models
{
    using System;

    using RaceCorp.Data.Common.Models;

    public class ApplicationUserRace : BaseDeletableModel<int>
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }

        public int TraceId { get; set; }

        public virtual Trace Trace { get; set; }
    }
}
