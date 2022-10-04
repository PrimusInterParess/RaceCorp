namespace RaceCorp.Web.ViewModels.Ride
{
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Trace;

    public class RideTraceBaseInputModel : RideBaseInputModel, IMapFrom<Trace>, IHaveCustomMappings
    {
        public TraceInputModel Trace { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Formats { get; set; } = new List<KeyValuePair<string, string>>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trace, TraceInputModel>()
              .ForMember(x => x.ControlTime, opt
                  => opt.MapFrom(x => x.ControlTime.TotalHours));
        }
    }
}
