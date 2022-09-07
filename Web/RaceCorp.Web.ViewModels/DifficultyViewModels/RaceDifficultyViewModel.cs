namespace RaceCorp.Web.ViewModels.DifficultyViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;

    public class RaceDifficultyViewModel
    {
        [Required]
        [MinLength(DefaultRaceMinLength, ErrorMessage = DefaultRaceLengthErrorMessage)]
        public int Length { get; set; }

        [Required]
        [Range(DefaultControlTimeMinValue, DefaultControlTimeMaxValue, ErrorMessage = DefaultControlTimeErrorMessage)]
        public TimeSpan ControlTime { get; set; }

        [Required]
        [Url]
        public string TrackUrl { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public int DifficultyId { get; set; }

        public IEnumerable<DifficultyViewModel> Difficulties { get; set; } = new List<DifficultyViewModel>();
    }
}
