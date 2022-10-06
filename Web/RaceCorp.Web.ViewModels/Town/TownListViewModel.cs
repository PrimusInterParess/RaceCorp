namespace RaceCorp.Web.ViewModels.Town
{
    using System.Collections.Generic;

    public class TownListViewModel
    {
       public List<TownRacesRidesViewModel> Towns { get; set; } = new List<TownRacesRidesViewModel>();
    }
}
