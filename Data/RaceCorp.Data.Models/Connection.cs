namespace RaceCorp.Data.Models
{
    using RaceCorp.Data.Common.Models;

    public class Connection : BaseDeletableModel<string>
    {
        public string InterlocutorId { get; set; }

        public virtual ApplicationUser Interlocutor { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
