namespace RaceCorp.Web.ViewModels.User
{
    using System.Collections.Generic;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Connection;

    public class UserAllConnectionsViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public ICollection<UserConnectionInAllViewModel> Connections { get; set; }
    }
}
