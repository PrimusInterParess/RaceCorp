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
        [Range(DefaultRaceMinLength, 200)]
        public double Length { get; set; }

        [Required]
        [StringLength(DefaultStrMaxValue,MinimumLength = DefaultStrMinValue,ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Difficulty { get; set; }

        [Required]
        public int ControlTime { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [Url]
        public string TrackUrl { get; set; }
    }
}
