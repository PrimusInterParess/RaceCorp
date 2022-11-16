namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Models;
    using RaceCorp.Data.Common.Repositories;

    public class Conversation : BaseDeletableModel<string>
    {
        public string Name { get; set; }

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
