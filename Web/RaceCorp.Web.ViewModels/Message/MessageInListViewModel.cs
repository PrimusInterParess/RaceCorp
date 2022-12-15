namespace RaceCorp.Web.ViewModels.Message
{
    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class MessageInListViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverFirstName { get; set; }

        public string ReceiverLastName { get; set; }

        public string ReceiverEmail { get; set; }

        public string SenderId { get; set; }

        public string SenderFirstName { get; set; }

        public string SenderLastName { get; set; }

        public string SenderEmail { get; set; }

        public string SenderProfilePicturePath { get; set; }

        public string ReceiverProfilePicturePath { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, MessageInListViewModel>()
                .ForMember(x => x.SenderEmail, opt => opt.MapFrom(x => x.Sender.Email))
                .ForMember(x => x.ReceiverEmail, opt => opt.MapFrom(x => x.Receiver.Email))
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn.ToString("d/MMMM/yyyy HH:mm")));
        }
    }
}
