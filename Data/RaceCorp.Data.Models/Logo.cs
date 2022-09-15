namespace RaceCorp.Data.Models
{
    using RaceCorp.Data.Common.Models;

    public class Logo : BaseDeletableModel<int>
    {
        public string Path { get; set; }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }
    }
}
