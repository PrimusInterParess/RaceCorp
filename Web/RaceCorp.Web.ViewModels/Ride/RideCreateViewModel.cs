namespace RaceCorp.Web.ViewModels.Ride
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.CommonViewModels;
    using RaceCorp.Web.ViewModels.DifficultyViewModels;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class RideCreateViewModel : RideBaseCreateViewModel
    {

        public RaceDifficultyCreateViewModel Trace { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DifficultiesKVP { get; set; } = new List<KeyValuePair<string, string>>();

    }
}
