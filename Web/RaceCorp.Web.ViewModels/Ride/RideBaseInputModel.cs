namespace RaceCorp.Web.ViewModels.Ride
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Services.ValidationAttributes;
    using RaceCorp.Web.ViewModels.Trace;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;
    using static RaceCorp.Web.ViewModels.Constants.StringValues;

    public class RideBaseInputModel
    {
        [Required]
        [Display(Name = DisplayName)]
        [StringLength(DefaultStrMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        [StringLength(DefaultStrMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Town { get; set; }

        [Required]
        [StringLength(DefaultStrMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Mountain { get; set; }

        [Required]
        [ValidateDate(ErrorMessage = InvalidDateErrorMessage)]
        public DateTime Date { get; set; }

        [StringLength(DefaultFormatMaxValue, MinimumLength = DefaultFormatMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]

        [Display(Name = DisplayNameFormat)]
        public string FormatId { get; set; }

        [StringLength(DefaultDescriptionMaxValue, MinimumLength = DefaultDescriptionMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Description { get; set; }
    }
}
