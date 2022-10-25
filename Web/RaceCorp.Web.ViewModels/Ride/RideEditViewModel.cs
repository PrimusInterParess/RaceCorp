namespace RaceCorp.Web.ViewModels.Ride
{
    using AutoMapper;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RideEditVIewModel : RideTraceBaseInputModel, IMapFrom<Ride>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int TraceId { get; set; }

        public new void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ride, RideEditVIewModel>()
            .ForMember(x => x.Mountain, opt
            => opt.MapFrom(x => x.Mountain.Name))
             .ForMember(x => x.Town, opt
              => opt.MapFrom(x => x.Town.Name));
        }
    }
}
