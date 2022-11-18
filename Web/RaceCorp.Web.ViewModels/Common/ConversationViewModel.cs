﻿namespace RaceCorp.Web.ViewModels.Common
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.ApplicationUsers;

    public class ConversationViewModel
    {
        public string Name { get; set; }

        public ICollection<UserConversationViewModel> Messages { get; set; }
    }
}
