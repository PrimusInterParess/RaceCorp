namespace RaceCorp.Web.InputViewModels
{
    using System;

    public class AddRaceInputModel
    {
        public string Name { get; set; }

        public double Length { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public string TrackUrl { get; set; }
    }
}
