namespace RaceCorp.Data.Models
{
    using RaceCorp.Data.Common.Models;

    public class ApplicationUserTrace : BaseDeletableModel<int>
    {
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int TraceId { get; set; }

        public virtual Trace Trace { get; set; }

        public virtual Race Race { get; set; }

        public int RaceId { get; set; }
    }
}
