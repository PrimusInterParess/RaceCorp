namespace RaceCorp.Web.ViewModels.User
{
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Trace;

    public class UserRaceBaseModel : IMapTo<ApplicationUserRace>, IHaveCustomMappings
    {
        public int RaceId { get; set; }

        public string RaceName { get; set; }

        public List<UserTraceBaseModel> Traces { get; set; } = new List<UserTraceBaseModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUserRace, UserRaceBaseModel>();

            configuration.CreateMap<Race, UserRaceBaseModel>();
        }
    }
}
