namespace RaceCorp.Web.ViewModels.Search
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RideSearchViewModel : IMapFrom<Ride>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
