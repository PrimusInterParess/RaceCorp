namespace RaceCorp.Web.ViewModels.Town
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class TownViewModel : IMapFrom<Town>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
