namespace RaceCorp.Data.Models
{
    public class Image : FileBaseModel
    {
        public string Name { get; set; }

        public string TeamId { get; set; }

        public Team Team { get; set; }
    }
}
