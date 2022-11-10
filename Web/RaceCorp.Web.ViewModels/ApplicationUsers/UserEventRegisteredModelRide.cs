namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserEventRegisteredModelRide : IMapTo<ApplicationUserRide>, IHaveCustomMappings
    {
        public string ApplicationUserId { get; set; }

        public string ApplicationUserFirstName { get; set; }

        public string ApplicationUserLastName { get; set; }

        public string MemberInTeam { get; set; }

        public string MemberInTeamId { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUserRide, UserEventRegisteredModelRide>()
                .ForMember(x => x.ApplicationUserFirstName, opt
                       => opt.MapFrom(x => x.ApplicationUser.FirstName))
                .ForMember(x => x.ApplicationUserLastName, opt
                       => opt.MapFrom(x => x.ApplicationUser.LastName))
                .ForMember(x => x.CreatedOn, opt
                       => opt.MapFrom(x => x.CreatedOn.ToString(GlobalConstants.DateStringFormat)));

        }
    }
}
