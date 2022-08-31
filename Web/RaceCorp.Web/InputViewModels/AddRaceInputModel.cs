namespace RaceCorp.Web.InputViewModels
{
    using System;

    public class AddRaceInputModel
    {
        public string Name { get; set; }

        public double Length { get; set; }

        public string Mountain { get; set; }

        public string Town { get; set; }

        public TimeSpan ControlTime { get; set; }

        public DateTime Date { get; set; }

        public string TrackUrl { get; set; }

        public string Description { get; set; }

        public string Difficulty { get; set; }

        public string Format { get; set; }

        // TODO: input for image uploads
    }
}
