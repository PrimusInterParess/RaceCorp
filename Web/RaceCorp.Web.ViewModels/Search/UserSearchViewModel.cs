namespace RaceCorp.Web.ViewModels.Search
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserSearchViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
