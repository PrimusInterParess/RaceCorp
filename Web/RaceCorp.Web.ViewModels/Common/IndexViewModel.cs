namespace RaceCorp.Web.ViewModels.CommonViewModels
{
    using RaceCorp.Web.ViewModels.RaceViewModels;
    using RaceCorp.Web.ViewModels.Ride;
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<KeyValuePair<string, string>> Towns { get; set; } = new HashSet<KeyValuePair<string, string>>();

        public IEnumerable<KeyValuePair<string, string>> Mountains { get; set; } = new HashSet<KeyValuePair<string, string>>();

        public IEnumerable<KeyValuePair<string, string>> Formats { get; set; } = new HashSet<KeyValuePair<string, string>>();

        public IEnumerable<KeyValuePair<string, string>> Difficulties { get; set; } = new HashSet<KeyValuePair<string, string>>();

        public string TownId { get; set; }

        public string FormatId { get; set; }

        public string DifficultyId { get; set; }

        public string MountainId { get; set; }

        public ICollection<RaceInAllViewModel> Races { get; set; } = new List<RaceInAllViewModel>();

    }
}
