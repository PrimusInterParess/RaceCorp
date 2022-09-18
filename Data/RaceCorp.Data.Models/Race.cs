using System.ComponentModel.DataAnnotations.Schema;

namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;

    public class Race : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public int FormatId { get; set; }

        public virtual Format Format { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public string LogoId { get; set; }

        public Logo Logo { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }

        // public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
        public virtual ICollection<RaceDifficulty> Traces { get; set; } = new HashSet<RaceDifficulty>();
    }
}
