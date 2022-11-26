namespace RaceCorp.Web.ViewModels.User
{
    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class CreatedRideBaseModel : IMapTo<Ride>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ride, CreatedRideBaseModel>()
                .ForMember(x => x.Id, opt
                       => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Name, opt
                       => opt.MapFrom(x => x.Name))
                               .ForMember(x => x.Date, opt
                       => opt.MapFrom(x => x.Date.ToString(GlobalConstants.DateStringFormat)));
        }
    }
}
