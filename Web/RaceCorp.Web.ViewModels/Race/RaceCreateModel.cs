namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.Race;
    using RaceCorp.Web.ViewModels.Trace;

    using static RaceCorp.Web.ViewModels.Constants.Formating;
    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;
    using static RaceCorp.Web.ViewModels.Constants.StringValues;

    public class RaceCreateModel : RaceBaseInputModel
    {
        public ICollection<TraceInputModel> Traces { get; set; } = new List<TraceInputModel>();

        public IEnumerable<KeyValuePair<string, string>> DifficultiesKVP { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
