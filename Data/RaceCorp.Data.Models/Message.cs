namespace RaceCorp.Data.Models
{
    using System;

    using RaceCorp.Data.Common.Models;

    public class Message : BaseDeletableModel<string>
    {
        public Message() => this.Id = Guid.NewGuid().ToString();

        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string RevceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }
    }
}
