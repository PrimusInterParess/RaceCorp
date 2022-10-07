namespace RaceCorp.Web.ViewModels.Mountain
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.Town;

    public class MountainListViewModel
    {
        public List<MountainRacesRidesViewModel> Mountains { get; set; } = new List<MountainRacesRidesViewModel>();
    }
}
