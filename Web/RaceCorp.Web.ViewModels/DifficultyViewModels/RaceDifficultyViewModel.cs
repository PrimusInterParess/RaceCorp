namespace RaceCorp.Web.ViewModels.DifficultyViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RaceDifficultyViewModel
    {
        [Required]
        [MinLength(1)]
        public int Length { get; set; }

        [Required]
        [MinLength(1)]
        public TimeSpan ControlTime { get; set; }

        [Required]
        [Url]
        public string TrackUrl { get; set; }

        [Required]
        public DateTime StartTime { get; set; }


        public int DifficultyId { get; set; }

        public IEnumerable<DifficultyViewModel> Difficulties { get; set; } = new List<DifficultyViewModel>();
    }
}
