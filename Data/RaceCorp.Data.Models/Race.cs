namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;

    public class Race : RideBaseModel
    {
        public string LogoId { get; set; }

        public Logo Logo { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }

        public int FormatId { get; set; }

        public virtual Format Format { get; set; }

        public ICollection<Trace> Traces { get; set; } = new HashSet<Trace>();

        // public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
    }
}
