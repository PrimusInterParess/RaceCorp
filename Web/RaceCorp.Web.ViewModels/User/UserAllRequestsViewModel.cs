namespace RaceCorp.Web.ViewModels.User
{
    using System.Collections.Generic;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Common;

    public class UserAllRequestsViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public ICollection<RequestBaseModel> ReceivedRequests { get; set; }
    }
}
