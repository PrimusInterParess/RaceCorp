﻿namespace RaceCorp.Web.ViewModels.Race
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Services.ValidationAttributes;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;
    using static RaceCorp.Web.ViewModels.Constants.StringValues;

    public abstract class RaceBaseInputModel
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

        public IFormFile RaceLogo { get; set; }

        [StringLength(DefaultDescriptionMaxValue, MinimumLength = DefaultDescriptionMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Description { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Formats { get; set; } = new List<KeyValuePair<string, string>>();
    }
}