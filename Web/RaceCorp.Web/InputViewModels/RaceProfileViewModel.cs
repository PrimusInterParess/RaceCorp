using System;

namespace RaceCorp.Web.InputViewModels
{
    public class RaceProfileViewModel
    {
        public string Name { get; set; }

        public double Length { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public string TrackUrl { get; set; }
    }
}
