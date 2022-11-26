namespace RaceCorp.Web.ViewModels.User
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserConversationViewModel : IMapFrom<Conversation>, IHaveCustomMappings
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

        public int UnreadMessages { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Conversation, UserConversationViewModel>()
                .ForMember(x => x.UnreadMessages, opt =>
                opt.MapFrom(x => x.ApplicationUser.InboxMessages.Where(m => m.SenderId == x.InterlocutorId && m.IsRead == false).ToList().Count()));
        }
    }
}
