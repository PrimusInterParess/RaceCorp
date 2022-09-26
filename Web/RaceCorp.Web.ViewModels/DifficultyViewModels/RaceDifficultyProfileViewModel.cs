namespace RaceCorp.Web.ViewModels.DifficultyViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RaceDifficultyProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string RaceName { get; set; }

        public int RaceId { get; set; }

        public string Difficulty { get; set; }

        public string LogoPath { get; set; }

        public int? Length { get; set; }

        public int DifficultyId { get; set; }

        public double? ControlTime { get; set; }

        public DateTime? StartTime { get; set; }

        public string TrackUrl { get; set; }
    }
}
