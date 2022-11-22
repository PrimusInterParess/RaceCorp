using RaceCorp.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceCorp.Data.Models
{
    public class ChatGroupName : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
