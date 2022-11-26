namespace RaceCorp.Web.ViewModels.User
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    using System;

    public class UserConnectionsViewModel : IMapFrom<Connection>
    {
        public string Id { get; set; }

        public string InterlocutorId { get; set; }

        public string InterlocutorProfilePicturePath { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
