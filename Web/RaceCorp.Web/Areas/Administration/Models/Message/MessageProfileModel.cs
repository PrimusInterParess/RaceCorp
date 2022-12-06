namespace RaceCorp.Web.Areas.Administration.Models.Message
{
    using Microsoft.Build.Framework;

    public class MessageProfileModel
    {
        public int Id { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string Subject { get; set; }

        public string CreatedOn { get; set; }

        [Required]
        public string Content { get; set; }

        public string ReplyContent { get; set; }

        public string ReplyDate { get; set; }
    }
}
