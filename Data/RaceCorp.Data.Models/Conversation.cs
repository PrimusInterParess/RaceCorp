namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Models;
    using RaceCorp.Data.Common.Repositories;

    public class Conversation : BaseDeletableModel<string>
    {
        public string UserAId { get; set; }

        public virtual ApplicationUser UserA { get; set; }

        public string UserBId { get; set; }

        public virtual ApplicationUser UserB { get; set; }

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
