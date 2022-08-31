namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Models;

    public class Image : BaseModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int RaceId { get; set; }

        public virtual Race Race { get; set; }

        public string AddByUserId { get; set; }

        public virtual ApplicationUser AddByUser { get; set; }

        public string Extension { get; set; }
    }
}
