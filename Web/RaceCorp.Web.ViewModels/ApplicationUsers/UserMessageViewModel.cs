namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using System.Collections.Generic;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserMessageViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
