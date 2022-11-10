namespace RaceCorp.Web.ViewModels.Common
{
    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RequestBaseModel : IMapFrom<Request>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ApplicationUserId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Request, RequestBaseModel>()
                                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                                .ForMember(x => x.ApplicationUserId, opt => opt.MapFrom(x => x.ApplicationUserId));

        }
    }
}
