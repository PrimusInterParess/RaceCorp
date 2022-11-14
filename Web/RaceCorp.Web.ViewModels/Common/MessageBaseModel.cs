namespace RaceCorp.Web.ViewModels.Common
{
    public class MessageBaseModel
    {
        public string HubId { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public string Content { get; set; }
    }
}
