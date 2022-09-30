namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Services.ValidationAttributes;
    using RaceCorp.Web.ViewModels.Race;
    using RaceCorp.Web.ViewModels.Ride;

    public class RaceEditViewModel : RaceBaseInputModel, IMapFrom<Race>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Race, RaceEditViewModel>()
                .ForMember(x => x.Mountain, opt
                       => opt.MapFrom(x => x.Mountain.Name))
                .ForMember(x => x.Town, opt
                    => opt.MapFrom(x => x.Town.Name));
        }
    }
}
