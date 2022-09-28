namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.ValidationAttributes;
    using RaceCorp.Web.ViewModels.CommonViewModels;
    using RaceCorp.Web.ViewModels.DifficultyViewModels;
    using RaceCorp.Web.ViewModels.FormatViewModels;

    using static RaceCorp.Web.ViewModels.Constants.Formating;
    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;
    using static RaceCorp.Web.ViewModels.Constants.StringValues;

    public class RaceCreateViewModel : RideBaseCreateViewModel
    {
        public ICollection<RaceTraceCreateViewModel> Difficulties { get; set; } = new List<RaceTraceCreateViewModel>();

        public IEnumerable<KeyValuePair<string, string>> DifficultiesKVP { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
