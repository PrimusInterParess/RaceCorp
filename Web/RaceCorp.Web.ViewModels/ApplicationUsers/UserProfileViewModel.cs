namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using System.Collections;
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserProfileViewModel : UserBaseModel, IMapTo<ApplicationUser>,IHaveCustomMappings
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TownName { get; set; }

        public string ContryName { get; set; }

        public string PictureId { get; set; }

        public ICollection<UserRaceBaseModel> Races { get; set; } = new List<UserRaceBaseModel>();

        public ICollection<UserRideBaseModel> Rides { get; set; } = new List<UserRideBaseModel>();
    }
}
