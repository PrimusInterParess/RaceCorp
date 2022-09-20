namespace RaceCorp.Data.Models
{
    using System;

    using RaceCorp.Data.Common.Models;

    public class Logo : BaseModel<string>
    {
        public Logo()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Extension { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }
    }
}
