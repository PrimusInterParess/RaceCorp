namespace RaceCorp.Web.ViewModels.HomeViewModels
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<KeyValuePair<string, string>> Towns { get; set; } = new HashSet<KeyValuePair<string, string>>();

        public IEnumerable<KeyValuePair<string, string>> Mountains { get; set; } = new HashSet<KeyValuePair<string, string>>();

        public IEnumerable<KeyValuePair<string, string>> Formats { get; set; } = new HashSet<KeyValuePair<string, string>>();

        public IEnumerable<KeyValuePair<string, string>> Difficulties { get; set; } = new HashSet<KeyValuePair<string, string>>();

        public string Id { get; set; }

        public string Type { get; set; }
    }
}
