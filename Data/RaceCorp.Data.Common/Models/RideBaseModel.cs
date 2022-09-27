using System;

namespace RaceCorp.Data.Common.Models
{
    public abstract class RideBaseModel : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

    }
}
