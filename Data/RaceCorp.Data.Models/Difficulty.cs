namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;
    using RaceCorp.Data.Models.Enums;

    public class Difficulty : BaseDeletableModel<int>
    {
        public DifficultyLevel Level { get; set; }

        public virtual ICollection<RideDifficulty> Races { get; set; } = new HashSet<RideDifficulty>();
    }
}
