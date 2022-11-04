namespace RaceCorp.Data.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using RaceCorp.Data.Common.Models;

    public abstract class FileBaseModel : BaseModel<string>
    {
        public FileBaseModel()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Extension { get; set; }

        [AllowNull]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ParentFolderName { get; set; }

        public string ChildFolderName { get; set; }
    }
}
