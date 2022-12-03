namespace RaceCorp.Data.Models
{
    using System.Collections.Generic;

    using RaceCorp.Data.Models.BaseModels;

    public class Race : RideBaseModel
    {
        public string LogoPath { get; set; }

        public string LogoId { get; set; }

        public virtual Logo Logo { get; set; }

        public ICollection<Trace> Traces { get; set; } = new HashSet<Trace>();

        public virtual ICollection<ApplicationUserRace> RegisteredUsers { get; set; } = new HashSet<ApplicationUserRace>();

        // public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
    }
}
