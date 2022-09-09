using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    public class AllRacesViewModel
    {
        ICollection<RaceViewModel> Races { get; set; } = new List<RaceViewModel>();
    }
}
