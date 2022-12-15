namespace RaceCorp.Web.ViewModels.User
{
    using System;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserConnectionsViewModel : IMapFrom<Connection>
    {
        public string Id { get; set; }

        public string InterlocutorId { get; set; }

        public string InterlocutorProfilePicturePath { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
