namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserConnectionsViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
    }
}
