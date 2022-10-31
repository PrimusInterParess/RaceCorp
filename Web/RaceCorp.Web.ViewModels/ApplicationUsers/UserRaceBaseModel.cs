namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserRaceBaseModel : IMapFrom<ApplicationUserRace>
    {
        public int RaceId { get; set; }

        public string RaceName { get; set; }

        public string TraceName { get; set; }

        public string RaceTraceStartTime { get; set; }
    }
}
