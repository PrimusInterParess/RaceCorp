namespace RaceCorp.Web.ViewModels.Trace
{
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.User;

    public class RaceTraceProfileModel : IMapFrom<Trace>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RaceName { get; set; }

        public string RaceApplicationUserId { get; set; }

        public int RaceId { get; set; }

        public string Difficulty { get; set; }

        public int? Length { get; set; }

        public int DifficultyId { get; set; }

        public double? ControlTime { get; set; }

        public string StartTime { get; set; }

        public string MapUrl { get; set; }

        public string GpxId { get; set; }

        public string GpxPath { get; set; }

        public bool IsOwner { get; set; }

        public bool IsRegistered { get; set; }

        public bool HasPassed { get; set; }

        public string TraceRaceApplicationUserId { get; set; }

        public ICollection<UserEventRegisteredModelTrace> RegisteredUsers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trace, RaceTraceProfileModel>()
                .ForMember(x => x.Difficulty, opt
                   => opt.MapFrom(x => x.Difficulty.Level.ToString()))
                .ForMember(x => x.ControlTime, opt
                   => opt.MapFrom(x => x.ControlTime.TotalHours))
                 .ForMember(x => x.StartTime, opt
                   => opt.MapFrom(x => x.StartTime.ToString(GlobalConstants.DateStringFormat)));
        }
    }
}
