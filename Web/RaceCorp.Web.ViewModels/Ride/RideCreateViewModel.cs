namespace RaceCorp.Web.ViewModels.Ride
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.Trace;

    public class RideCreateViewModel : RideBaseInputModel
    {
        public TraceInputModel Trace { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DifficultiesKVP { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
