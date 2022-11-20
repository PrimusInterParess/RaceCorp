namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Models;

    public class Conversation : BaseDeletableModel<string>
    {
        public string InterlocutorId { get; set; }

        public string AuthorId { get; set; }

        public string UserEmail { get; set; }

        public string LastMessageContent { get; set; }

        public string LastMessageDate { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserProfilePicturePath { get; set; }
    }
}
