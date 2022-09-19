namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;
    using System.Collections.Generic;

    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.CommonViewModels;

    public class RaceAllViewModel : PagingViewModel
    {
        public IEnumerable<RaceViewModel> Races { get; set; } = new List<RaceViewModel>();
    }
}
