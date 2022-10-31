namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using System.Collections;
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserProfileViewModel : IMapTo<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TownName { get; set; }

        public string Country { get; set; }

        public string PictureId { get; set; }

       
        public List<UserRideBaseModel> Rides { get; set; } = new List<UserRideBaseModel>();

        public List<UserTraceBaseModel> Traces { get; set; } = new List<UserTraceBaseModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserProfileViewModel>();

        }
    }
}
