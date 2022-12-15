namespace RaceCorp.Web.ViewModels.Request
{
    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RequestBaseModel : IMapFrom<Request>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string TargetUserId { get; set; }

        public string RequesterId { get; set; }

        public string Type { get; set; }

        public bool IsApproved { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Request, RequestBaseModel>()
                                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn.ToString(GlobalConstants.DateStringFormat)));
        }
    }
}
