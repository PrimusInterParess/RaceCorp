namespace RaceCorp.Web.ViewModels.Connection
{
    using System;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserConnectionInAllViewModel : IMapFrom<Connection>
    {
        public string InterlocutorId { get; set; }

        public string InterlocutorProfilePicturePath { get; set; }

        public string InterlocutorFirstName { get; set; }

        public string InterlocutorLastName { get; set; }

        public string InterlocutorTownName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
