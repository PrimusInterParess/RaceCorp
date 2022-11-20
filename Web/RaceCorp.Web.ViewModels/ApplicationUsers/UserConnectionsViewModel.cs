namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserConnectionsViewModel : IMapFrom<Connection>
    {
        public string Id { get; set; }
    }
}
