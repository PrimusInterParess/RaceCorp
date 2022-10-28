namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserRideRegisteredModel : IMapTo<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUserRide, UserRideRegisteredModel>().ForMember(x => x.FullName, opt
                   => opt.MapFrom(x => x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName));
        }
    }
}
