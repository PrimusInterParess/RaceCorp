namespace RaceCorp.Web.ViewModels.User
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserEventRegisteredModelRace : IMapTo<ApplicationUserRace>, IHaveCustomMappings
    {
        public string ApplicationUserId { get; set; }

        public string ApplicationUserFirstName { get; set; }

        public string ApplicationUserLastName { get; set; }

        public string ApplicationUserMemberInTeamName { get; set; }

        public string TownName { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUserRace, UserEventRegisteredModelRace>()
                .ForMember(x => x.ApplicationUserFirstName, opt
                       => opt.MapFrom(x => x.ApplicationUser.FirstName))
                .ForMember(x => x.TownName, opt
                       => opt.MapFrom(x => x.ApplicationUser.Town.Name))
                .ForMember(x => x.ApplicationUserLastName, opt
                       => opt.MapFrom(x => x.ApplicationUser.LastName))
                .ForMember(x => x.CreatedOn, opt
                       => opt.MapFrom(x => x.CreatedOn.ToString(GlobalConstants.DateStringFormat)));
        }
    }
}
