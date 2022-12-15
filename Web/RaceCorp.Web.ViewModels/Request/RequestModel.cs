namespace RaceCorp.Web.ViewModels.Request
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RequestModel : IMapFrom<Request>
    {
        public string RequesterId { get; set; }
    }
}
