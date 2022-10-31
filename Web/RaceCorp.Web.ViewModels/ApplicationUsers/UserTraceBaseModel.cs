namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserTraceBaseModel : IMapTo<ApplicationUserTrace>, IHaveCustomMappings
    {
        public string ApplicationUserId { get; set; }

        public int TraceId { get; set; }

        public int RaceId { get; set; }

        public string RaceName { get; set; }

        public string TraceName { get; set; }

        public string TraceStartTime { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUserTrace, UserTraceBaseModel>()
                .ForMember(x => x.RaceName, opt
                       => opt.MapFrom(x => x.Race.Name))
                 .ForMember(x => x.TraceName, opt
                       => opt.MapFrom(x => x.Trace.Name))
                  .ForMember(x => x.TraceStartTime, opt
                       => opt.MapFrom(x => x.Trace.StartTime.ToString("f")));
        }
    }
}
