namespace RaceCorp.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using RaceCorp.Data.Common.Models;

    public class AdminContactReply : BaseDeletableModel<int>
    {
        public string AdminId { get; set; }

        public virtual ApplicationUser Admin { get; set; }

        public int AdminContactId { get; set; }

        [ForeignKey(nameof(AdminContactId))]

        public AdminContact AdminContact { get; set; }

        public string Content { get; set; }
    }
}
