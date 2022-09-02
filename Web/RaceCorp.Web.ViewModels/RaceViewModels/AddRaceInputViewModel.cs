namespace RaceCorp.Web.ViewModels.RaceViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    public class AddRaceInputViewModel
    {
        [Display(Name = "Race name")]
        [MinLength(3)]
        public string Name { get; set; }

        [MinLength(3)]
        public string Town { get; set; }

        [MinLength(3)]
        public string Mountain { get; set; }

        public DateTime Date { get; set; }

        [MinLength(20)]
        public string Description { get; set; }

        public ICollection<RaceDifficultyViewModel> Difficulties { get; set; }

        [MinLength(1)]
        public string Format { get; set; }

        // TODO: input for image uploads
    }
}
