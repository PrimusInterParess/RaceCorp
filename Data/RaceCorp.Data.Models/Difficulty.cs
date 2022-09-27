namespace RaceCorp.Data.Models
{
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Models;
    using RaceCorp.Data.Models.Enums;

    public class Difficulty : BaseDeletableModel<int>
    {
        public DifficultyLevel Level { get; set; }

        public ICollection<Trace> Traces { get; set; }
    }
}
