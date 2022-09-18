namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllRacesViewModel
    {
        ICollection<RaceViewModel> Races { get; set; } = new List<RaceViewModel>();
    }
}
