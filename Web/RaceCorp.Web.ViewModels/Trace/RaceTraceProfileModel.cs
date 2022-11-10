namespace RaceCorp.Web.ViewModels.Trace
{
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.ApplicationUsers;
    using RaceCorp.Web.ViewModels.EventRegister;

    public class RaceTraceProfileModel : IMapFrom<Trace>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RaceName { get; set; }

        public int RaceId { get; set; }

        public string Difficulty { get; set; }

        public int? Length { get; set; }

        public int DifficultyId { get; set; }

        public double? ControlTime { get; set; }

        public string StartTime { get; set; }

        public string GpxGoogleDriveId { get; set; }

        public string GpxId { get; set; }

        public string GpxPath { get; set; }

        public ICollection<UserEventRegisteredModelTrace> RegisteredUsers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trace, RaceTraceProfileModel>()
                .ForMember(x => x.Difficulty, opt
                   => opt.MapFrom(x => x.Difficulty.Level.ToString()))
                .ForMember(x => x.ControlTime, opt
                   => opt.MapFrom(x => x.ControlTime.TotalHours));
        }
    }
}
