namespace RaceCorp.Web.ViewModels.Common
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class MessageInListViewModel : IMapFrom<Message>
    {
        public string Id { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverFirstName { get; set; }

        public string ReceiverLastName { get; set; }

        public string SenderId { get; set; }

        public string SenderFirstName { get; set; }

        public string SenderLastName { get; set; }

        public string SenderProfilePicturePath { get; set; }

        public string ReceiverProfilePicturePath { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }
    }
}
