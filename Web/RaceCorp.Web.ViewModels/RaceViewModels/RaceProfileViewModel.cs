namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    public class RaceProfileViewModel : RaceViewModel
    {
        public List<DifficultyInRaceProfileViewModel> Traces { get; set; }
    }
}
