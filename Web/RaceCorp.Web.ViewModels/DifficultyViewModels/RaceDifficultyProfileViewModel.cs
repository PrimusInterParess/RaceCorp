namespace RaceCorp.Web.ViewModels.DifficultyViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RaceDifficultyProfileViewModel : RaceDifficultyViewModel
    {
        public string RaceName { get; set; }

        public int RaceId { get; set; }

        public string Difficulty { get; set; }

        public string LogoPath { get; set; }

    }
}
