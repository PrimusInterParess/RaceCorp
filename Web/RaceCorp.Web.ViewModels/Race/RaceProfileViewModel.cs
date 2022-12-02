namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Trace;
    using RaceCorp.Web.ViewModels.User;

    public class RaceProfileViewModel : RaceViewModel, IMapFrom<Race>, IHaveCustomMappings
    {
        public string Date { get; set; }

        public string ApplicationUserId { get; set; }

        public bool IsOwner { get; set; }

        public bool IsRegistered { get; set; }

        public bool HasPassed { get; set; }

        public string ApplicationUserProfilePicturePath { get; set; }

        public string ApplicationUserFirstName { get; set; }

        public string ApplicationUserLastName { get; set; }

        public List<TraceInRaceProfileViewModel> Traces { get; set; }

        public ICollection<UserEventRegisteredModelRace> RegisteredUsers { get; set; } = new List<UserEventRegisteredModelRace>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Race, RaceProfileViewModel>()
                .ForMember(x => x.LogoPath, opt
                 => opt.MapFrom(x => x.LogoPath))
                .ForMember(x => x.Town, opt
                       => opt.MapFrom(x => x.Town.Name))
                .ForMember(x => x.Mountain, opt
                    => opt.MapFrom(x => x.Mountain.Name))
                .ForMember(x => x.Date, opt
                   => opt.MapFrom(x => x.Date.ToString(GlobalConstants.DateStringLongFormat)));

            configuration.CreateMap<Trace, TraceInRaceProfileViewModel>()
                .ForMember(x => x.DifficultyName, opt
                   => opt.MapFrom(x => x.Difficulty.Level.ToString()))
                .ForMember(x => x.ControlTime, opt
                   => opt.MapFrom(x => x.ControlTime.TotalHours))
                .ForMember(x => x.StartTime, opt
                   => opt.MapFrom(x => x.StartTime.ToString(GlobalConstants.DateStringFormat)));
        }
    }
}
