namespace RaceCorp.Web.ViewModels.Ride
{
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.User;

    public class RideProfileVIewModel : IMapFrom<Ride>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserProfilePicturePath { get; set; }

        public string ApplicationUserFirstName { get; set; }

        public string ApplicationUserLastName { get; set; }

        public string TraceName { get; set; }

        public string Difficulty { get; set; }

        public int Length { get; set; }

        public int DifficultyId { get; set; }

        public double ControlTime { get; set; }

        public string StartTime { get; set; }

        public string Description { get; set; }

        public string TraceMapUrl { get; set; }

        public bool IsOwner { get; set; }

        public bool IsRegistered { get; set; }

        public bool HasPassed { get; set; }

        public string TraceGpxId { get; set; }

        public ICollection<UserEventRegisteredModelRide> RegisteredUsers { get; set; } = new List<UserEventRegisteredModelRide>();

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
                   => opt.MapFrom(x => x.Trace.StartTime.ToString(GlobalConstants.DateStringFormat)))
                .ForMember(x => x.Length, opt
                   => opt.MapFrom(x => x.Trace.Length));
        }
    }
}
