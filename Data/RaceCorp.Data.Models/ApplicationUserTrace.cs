namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationUserTrace
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int TraceId { get; set; }

        public virtual Trace Trace { get; set; }

        public virtual Race Race { get; set; }

        public int RaceId { get; set; }
    }
}
