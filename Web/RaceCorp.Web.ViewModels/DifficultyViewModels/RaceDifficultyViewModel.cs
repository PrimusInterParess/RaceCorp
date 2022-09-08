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
        public int Length { get; set; }

        [Required]
        public string DifficultyId { get; set; }

        [Required]
        public TimeSpan ControlTime { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [Url]
        public string TrackUrl { get; set; }
    }
}
