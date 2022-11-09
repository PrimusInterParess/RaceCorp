namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserProfileViewModel : IMapTo<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TownName { get; set; }

        public string Country { get; set; }

        public string About { get; set; }

        public string ProfilePicturePath { get; set; }

        public string LinkedInLink { get; set; }

        public string FacoBookLink { get; set; }

        public string GitHubLink { get; set; }

        public string TwitterLink { get; set; }

        public virtual ICollection<CreatedRideBaseModel> CreatedRides { get; set; } = new HashSet<CreatedRideBaseModel>();

        public virtual ICollection<CreatedRaceBaseModel> CreatedRaces { get; set; } = new HashSet<CreatedRaceBaseModel>();

        public List<UserRideBaseModel> Rides { get; set; } = new List<UserRideBaseModel>();

        public List<UserTraceBaseModel> Traces { get; set; } = new List<UserTraceBaseModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            // the last one to close the door!
            configuration.CreateMap<ApplicationUser, UserProfileViewModel>().ForMember(x => x.ProfilePicturePath, opt
                       => opt.MapFrom(x => x.ProfilePicturePath == null ?
                       "\\Images\\default\\Murgash.jpg" : x.ProfilePicturePath));
        }
    }
}
