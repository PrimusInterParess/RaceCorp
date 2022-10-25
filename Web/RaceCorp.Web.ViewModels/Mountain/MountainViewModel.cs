namespace RaceCorp.Web.ViewModels.Mountain
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class MountainViewModel : IMapFrom<Mountain>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
