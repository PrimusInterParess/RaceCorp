namespace RaceCorp.Web.ViewModels.Trace
{
    public class RaceTraceProfileModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RaceName { get; set; }

        public int RaceId { get; set; }

        public string Difficulty { get; set; }

        public string LogoPath { get; set; }

        public int? Length { get; set; }

        public int DifficultyId { get; set; }

        public double? ControlTime { get; set; }

        public string StartTime { get; set; }

        public string GoogleDriveId { get; set; }

        public string GpxId { get; set; }
    }
}
