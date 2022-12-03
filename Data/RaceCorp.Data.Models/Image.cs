namespace RaceCorp.Data.Models
{
    using RaceCorp.Data.Models.BaseModels;

    public class Image : FileBaseModel
    {
        public string Name { get; set; }

        public string TeamId { get; set; }

        public Team Team { get; set; }
    }
}
