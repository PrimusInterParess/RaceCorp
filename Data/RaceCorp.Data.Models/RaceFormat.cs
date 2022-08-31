namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RaceFormat
    {
        public int Id { get; set; }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }

        public int FormatId { get; set; }

        public virtual Format Format { get; set; }
    }
}
