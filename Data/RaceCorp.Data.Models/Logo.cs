namespace RaceCorp.Data.Models
{
    using System;

    using RaceCorp.Data.Common.Models;

    public class Logo : FileBaseModel
    {
        public int? RaceId { get; set; }

        public virtual Race Race { get; set; }
    }
}
