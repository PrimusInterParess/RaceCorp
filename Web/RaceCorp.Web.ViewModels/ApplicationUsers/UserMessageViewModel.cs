using System.Collections.Generic;

namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    public class UserMessageViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public ICollection<UserConnectionsViewModel> Messages { get; set; }
    }
}
