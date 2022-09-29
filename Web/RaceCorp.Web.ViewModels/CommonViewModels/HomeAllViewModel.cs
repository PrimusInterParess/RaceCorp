namespace RaceCorp.Web.ViewModels.CommonViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.RaceViewModels;
    using RaceCorp.Web.ViewModels.Ride;

    public class HomeAllViewModel
    {
        public string Message { get; set; }

        public ICollection<RaceInAllViewModel> Races { get; set; } = new List<RaceInAllViewModel>();

        public ICollection<RideInAllViewModel> Rides { get; set; } = new List<RideInAllViewModel>();
    }
}
