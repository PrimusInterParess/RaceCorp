namespace RaceCorp.Data.Models
{
    using System;

    public class ApplicationUserRace
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }

        public string TraceName { get; set; }

        public virtual Trace Trace { get; set; }
    }
}
