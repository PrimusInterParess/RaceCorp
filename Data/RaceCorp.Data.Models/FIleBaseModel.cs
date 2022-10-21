namespace RaceCorp.Data.Models
{
    using System;

    using RaceCorp.Data.Common.Models;

    public abstract class FileBaseModel : BaseModel<string>
    {
        public FileBaseModel()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Extension { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
