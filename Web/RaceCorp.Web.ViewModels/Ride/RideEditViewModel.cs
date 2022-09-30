namespace RaceCorp.Web.ViewModels.Ride
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Services.ValidationAttributes;
    using RaceCorp.Web.ViewModels.Trace;
    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;
    using static RaceCorp.Web.ViewModels.Constants.StringValues;

    public class RideEditVIewModel : IMapFrom<Ride>, IHaveCustomMappings
    {
        public int Id { get; set; }

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

        public traceed Trace { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Formats { get; set; } = new List<KeyValuePair<string, string>>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ride, RideEditVIewModel>()
            .ForMember(x => x.Mountain, opt
            => opt.MapFrom(x => x.Mountain.Name))
             .ForMember(x => x.Town, opt
              => opt.MapFrom(x => x.Town.Name));

            configuration.CreateMap<Trace, TraceInputModel>()
             .ForMember(x => x, opt
                    => opt.MapFrom(x => x.Difficulty.Level.ToString()))
                .ForMember(x => x.ControlTime, opt
                    => opt.MapFrom(x => x.ControlTime.TotalHours));
        }
    }
}
