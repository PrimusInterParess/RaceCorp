namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Models;

    public class Ride : RideBaseModel
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }

        public int FormatId { get; set; }

        public virtual Format Format { get; set; }

        public int TraceId { get; set; }

        public Trace Trace { get; set; }
    }
}
