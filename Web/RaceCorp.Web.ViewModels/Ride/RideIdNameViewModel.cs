namespace RaceCorp.Web.ViewModels.Ride
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RideIdNameViewModel : IMapFrom<Ride>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
