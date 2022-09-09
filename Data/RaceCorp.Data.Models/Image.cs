namespace RaceCorp.Data.Models
{
    using RaceCorp.Data.Common.Models;

    public class Image : BaseDeletableModel<int>
    {
        public string ImagePath { get; set; }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }
    }
}
