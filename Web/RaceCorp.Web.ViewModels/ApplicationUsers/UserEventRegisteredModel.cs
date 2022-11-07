namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserEventRegisteredModel : IMapTo<ApplicationUserRide>, IHaveCustomMappings
    {
        public string ApplicationUserId { get; set; }

        public string ApplicationUserFirstName { get; set; }

        public string ApplicationUserLastName { get; set; }

        public string ApplicationUserGender { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUserRide, UserEventRegisteredModel>().ForMember(x => x.ApplicationUserFirstName, opt
                       => opt.MapFrom(x => x.ApplicationUser.FirstName))
                .ForMember(x => x.ApplicationUserLastName, opt
                       => opt.MapFrom(x => x.ApplicationUser.LastName))
                .ForMember(x => x.ApplicationUserGender, opt
                       => opt.MapFrom(x => x.ApplicationUser.Gender));

            configuration.CreateMap<ApplicationUserRace, UserEventRegisteredModel>().ForMember(x => x.ApplicationUserFirstName, opt
                       => opt.MapFrom(x => x.ApplicationUser.FirstName))
                .ForMember(x => x.ApplicationUserLastName, opt
                       => opt.MapFrom(x => x.ApplicationUser.LastName))
                .ForMember(x => x.ApplicationUserGender, opt
                       => opt.MapFrom(x => x.ApplicationUser.Gender));
        }
    }
}
