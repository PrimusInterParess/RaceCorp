namespace RaceCorp.Web.ViewModels.Trace
{
    using System.Collections.Generic;

    using AutoMapper;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RaceTraceEditModel : TraceInputModel, IMapFrom<Trace>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int RaceId { get; set; }

        public string DifficultyName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trace, RaceTraceEditModel>()
                .ForMember(x => x.DifficultyName, opt
                    => opt.MapFrom(x => x.Difficulty.Level.ToString()))
                .ForMember(x => x.ControlTime, opt
                    => opt.MapFrom(x => x.ControlTime.TotalHours));
        }
    }
}
