namespace RaceCorp.Web.ViewModels.DifficultyViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TraceInRaceProfileViewModel
    {
        public string DifficultyName { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? Length { get; set; }

        public int DifficultyId { get; set; }

        public double? ControlTime { get; set; }

        public string StartTime { get; set; }

        public string TrackUrl { get; set; }
    }
}
