namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RaceDifficulty
    {
        public int Id { get; set; }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }

        public int DifficultyId { get; set; }

        public virtual Difficulty Difficulty { get; set; }

        public int Length { get; set; }

        // TODO: add race length here
    }
}
