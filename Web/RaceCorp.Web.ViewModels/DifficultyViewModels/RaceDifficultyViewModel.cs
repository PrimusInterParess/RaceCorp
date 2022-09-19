namespace RaceCorp.Web.ViewModels.DifficultyViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RaceCorp.Services.ValidationAttributes;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;

    public class RaceDifficultyViewModel
    {
        [Display(Name = "Trace name")]
        [Required]
        [StringLength(DefaultStrMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Name { get; set; }

        [Display(Name = "Race length")]
        [Range(DefaultRaceMinLength, DefaultRaceMaxength, ErrorMessage = DefaultRaceLengthErrorMessage)]
        public int Length { get; set; }

        [Required]
        public string DifficultyId { get; set; }

        [Display(Name = "Control time (in hours)")]
        [Range(DefaultControlTimeMinValue, DefaultControlTimeMaxValue, ErrorMessage = DefaultControlTimeErrorMessage)]
        public double ControlTime { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage ="Start date is Required!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Display(Name = "Track url")]
        [Required]
        [Url]
        public string TrackUrl { get; set; }
    }
}
