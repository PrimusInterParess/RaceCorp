namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using RaceCorp.Data.Common.Models;

    public class Team : BaseDeletableModel<string>
    {
        public Team() => this.Id = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Description { get; set; }

        public string LogoImagePath { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<ApplicationUser> TeamMembers { get; set; } = new HashSet<ApplicationUser>();

        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
    }
}
