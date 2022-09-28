namespace RaceCorp.Data.Common.Models
{
    using System;

    using RaceCorp.Data.Models;

    public abstract class RideBaseModel : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }

        public int FormatId { get; set; }

        public virtual Format Format { get; set; }
    }
}
