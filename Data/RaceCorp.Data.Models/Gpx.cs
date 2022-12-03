namespace RaceCorp.Data.Models
{
    using RaceCorp.Data.Models.BaseModels;

    public class Gpx : FileBaseModel
    {
        public string GoogleDriveId { get; set; }

        public string GoogleDriveDirectoryId { get; set; }

        public virtual Trace Trace { get; set; }

        public int TraceId { get; set; }
    }
}
