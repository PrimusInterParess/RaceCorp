namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Services.ValidationAttributes;

    public class RaceEditViewModel : RaceBaseViewModel, IMapFrom<Race>, IHaveCustomMappings
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
