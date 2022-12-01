namespace RaceCorp.Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    using RaceCorp.Data.Models;

    public abstract class RideBaseModel : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Description { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }

        public int FormatId { get; set; }

        public virtual Format Format { get; set; }
    }
}
