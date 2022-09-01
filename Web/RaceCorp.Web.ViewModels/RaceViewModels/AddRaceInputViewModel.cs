namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;

    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    public class AddRaceInputViewModel
    {
        public string Name { get; set; }

        public double Length { get; set; }

        public string Town { get; set; }

        public string Mountain { get; set; }

        public TimeSpan ControlTime { get; set; }

        public DateTime Date { get; set; }

        public string TrackUrl { get; set; }

        public string Description { get; set; }

        public ICollection<DifficultyViewModel> Difficulties { get; set; }

        public string Format { get; set; }

        // TODO: input for image uploads
    }
}
