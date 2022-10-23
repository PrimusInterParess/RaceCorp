namespace RaceCorp.Web.ViewModels.Trace
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;

    public class TraceInputModel
    {
        [Display(Name = "Trace name")]
        [Required]
        [StringLength(DefaultStrMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Name { get; set; }

        // todo: change race-to trace
        [Required(ErrorMessage = InvalidLengthFieldErrorMessage)]
        [Display(Name = "Trace length")]
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

        public IFormFile GpxFile { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DifficultiesKVP { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
