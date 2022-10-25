using System;

namespace RaceCorp.Data.Models
{
    public class ApplicationUserRace
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }
    }
}
