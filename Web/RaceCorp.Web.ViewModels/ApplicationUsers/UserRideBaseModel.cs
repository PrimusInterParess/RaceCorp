namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserRideBaseModel : IMapFrom<ApplicationUserRide>
    {
        public int RideId { get; set; }

        public string RideName { get; set; }

        public string RideTraceStartTime { get; set; }
    }
}
