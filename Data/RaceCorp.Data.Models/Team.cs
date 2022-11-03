namespace RaceCorp.Data.Models
{
    using System.Collections;
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Models;

    public class Team : BaseDeletableModel<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<ApplicationUser> TeamMembers { get; set; } = new HashSet<ApplicationUser>();
    }
}
