﻿namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserInboxViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string ProfilePicturePath { get; set; }

        public ICollection<UserConversationViewModel> Conversations { get; set; } = new HashSet<UserConversationViewModel>();
    }
}