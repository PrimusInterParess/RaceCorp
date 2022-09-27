namespace RaceCorp.Web.ViewModels.DifficultyViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RaceCorp.Services.ValidationAttributes;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;

    public class RaceDifficultyCreateViewModel
    {
        [Display(Name = "Trace name")]
        [Required]
        [StringLength(DefaultStrMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Name { get; set; }

        public int RaceId { get; set; }

        [Required(ErrorMessage = InvalidLengthFieldErrorMessage)]
        [Display(Name = "Race length")]
        [Range(DefaultRaceMinLength, DefaultRaceMaxength, ErrorMessage = DefaultRaceLengthErrorMessage)]
        public int? Length { get; set; }

        [Required]
        public int DifficultyId { get; set; }

        [Required(ErrorMessage = InvalidControlTimeFieldErrorMessage)]
        [Display(Name = "Control time (in hours)")]
        [Range(DefaultControlTimeMinValue, DefaultControlTimeMaxValue, ErrorMessage = DefaultControlTimeErrorMessage)]
        public double? ControlTime { get; set; }

        [Required(ErrorMessage = InvalidStartDateErrorMessage)]
        [Display(Name = "Start Date")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "Track url")]
        [Required]
        [Url]
        public string TrackUrl { get; set; }

    }
}
