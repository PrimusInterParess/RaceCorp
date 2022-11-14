namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserAllViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
