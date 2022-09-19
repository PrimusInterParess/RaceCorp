namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RaceViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string LogoPath { get; set; }

        public string Town { get; set; }

        public int TownId { get; set; }

        public string Mountain { get; set; }

        public int MountainId { get; set; }
    }
}
