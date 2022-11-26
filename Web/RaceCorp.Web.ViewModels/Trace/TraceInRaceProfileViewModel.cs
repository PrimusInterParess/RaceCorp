namespace RaceCorp.Web.ViewModels.Trace
{
    using System.Collections.Generic;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.User;

    public class TraceInRaceProfileViewModel : IMapTo<Trace>
    {
        public string DifficultyName { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? Length { get; set; }

        public int DifficultyId { get; set; }

        public double? ControlTime { get; set; }

        public string StartTime { get; set; }

        public ICollection<UserTraceBaseModel> RegisteredUsers { get; set; } = new HashSet<UserTraceBaseModel>();
    }
}
