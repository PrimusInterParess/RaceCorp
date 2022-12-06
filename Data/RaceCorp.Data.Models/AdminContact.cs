namespace RaceCorp.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using RaceCorp.Data.Common.Models;

    public class AdminContact : BaseDeletableModel<int>
    {
        public string AdminId { get; set; }

        public virtual ApplicationUser Admin { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public bool IsReplied { get; set; }

        public int? AdminContactReplyId { get; set; }

        [ForeignKey(nameof(AdminContactReplyId))]
        public virtual AdminContactReply AdminContactReply { get; set; }
    }
}
