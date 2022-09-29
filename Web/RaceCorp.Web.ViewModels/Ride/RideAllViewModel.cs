namespace RaceCorp.Web.ViewModels.Ride
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.CommonViewModels;

    public class RideAllViewModel : PagingViewModel
    {
        public ICollection<RideInAllViewModel> Rides { get; set; } = new List<RideInAllViewModel>();
    }
}
