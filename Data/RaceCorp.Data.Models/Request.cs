namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;

    public class Request : BaseDeletableModel<int>
    {
        public string RequesterId { get; set; }

        public virtual ApplicationUser Requester { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        // change applicationUser to targetUser
        public string TargetUserId { get; set; }

        public virtual ApplicationUser TargetUser { get; set; }

        public bool IsApproved { get; set; }
    }
}
