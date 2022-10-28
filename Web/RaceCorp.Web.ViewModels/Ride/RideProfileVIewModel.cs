namespace RaceCorp.Web.ViewModels.Ride
{
    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.ApplicationUsers;
    using System.Collections.Generic;

    public class RideProfileVIewModel : IMapFrom<Ride>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TraceName { get; set; }

        public string Difficulty { get; set; }

        public int Length { get; set; }

        public int DifficultyId { get; set; }

        public double ControlTime { get; set; }

        public string StartTime { get; set; }

        public string TraceGpxGoogleDriveId { get; set; }

        public string TraceGpxId { get; set; }

        public ICollection<UserRideRegisteredModel> RegisteredUsers { get; set; } = new List<UserRideRegisteredModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ride, RideProfileVIewModel>()
                .ForMember(x => x.TraceName, opt
                   => opt.MapFrom(x => x.Trace.Name))
                .ForMember(x => x.Difficulty, opt
                   => opt.MapFrom(x => x.Trace.Difficulty.Level.ToString()))
                .ForMember(x => x.ControlTime, opt
                   => opt.MapFrom(x => x.Trace.ControlTime.TotalHours))
                .ForMember(x => x.StartTime, opt
                   => opt.MapFrom(x => x.Trace.StartTime.ToString("HH:MM")))
                .ForMember(x => x.Length, opt
                   => opt.MapFrom(x => x.Trace.Length));
        }
    }
}
