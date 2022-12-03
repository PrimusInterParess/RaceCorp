namespace RaceCorp.Data.Models.BaseModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using RaceCorp.Data.Common.Models;

    public abstract class FileBaseModel : BaseDeletableModel<string>
    {
        public FileBaseModel() => this.Id = Guid.NewGuid().ToString();

        public string Extension { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ParentFolderName { get; set; }

        public string ChildFolderName { get; set; }
    }
}
