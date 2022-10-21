namespace RaceCorp.Web.ViewModels.Trace
{
    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RideTraceEditModel : TraceInputModel
    {
        public int Id { get; set; }

        public string DifficultyName { get; set; }
    }
}
