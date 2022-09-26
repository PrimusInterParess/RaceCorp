namespace RaceCorp.Data.Common.Models
{
    using System;

    using RaceCorp.Data.Common.Models;

    public class BaseRide : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

    }
}
