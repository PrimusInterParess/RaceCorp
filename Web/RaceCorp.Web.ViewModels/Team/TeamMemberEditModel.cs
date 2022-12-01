namespace RaceCorp.Web.ViewModels.Team
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class TeamMemberEditModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }



        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
