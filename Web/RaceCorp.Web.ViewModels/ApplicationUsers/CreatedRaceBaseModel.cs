namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class CreatedRaceBaseModel : IMapTo<Race>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Race, CreatedRaceBaseModel>()
                .ForMember(x => x.Id, opt
                       => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Name, opt
                       => opt.MapFrom(x => x.Name));
        }
    }
}
