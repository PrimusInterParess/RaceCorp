namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using System.Collections.Generic;

    using AutoMapper;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserConversationViewModel
    {
        public string Id { get; set; }

        public string InterlocutorId { get; set; }

        public string AuthorId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string LastMessageContent { get; set; }

        public string LastMessageDate { get; set; }

        public string Email { get; set; }

        public string UserProfilePicturePath { get; set; }
    }
}
