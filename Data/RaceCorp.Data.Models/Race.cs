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

        public virtual Logo Logo { get; set; }

        public ICollection<Trace> Traces { get; set; } = new HashSet<Trace>();

        // public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
    }
}
