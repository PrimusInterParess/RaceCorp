namespace RaceCorp.Web.Areas.Administration.Models.Admin
{
    public class AdminContactMessage
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string CreatedOn { get; set; }

        public bool IsReplied { get; set; }
    }
}
