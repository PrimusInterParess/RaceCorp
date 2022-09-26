namespace RaceCorp.Web.ViewModels.DifficultyViewModels
{
    using System.Collections.Generic;

    using AutoMapper;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class RaceDifficultyInputViewModel : RaceDifficultyCreateViewModel, IMapFrom<RideDifficulty>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int RaceId { get; set; }

        public string DifficultyName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DifficultiesKVP { get; set; } = new List<KeyValuePair<string, string>>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RideDifficulty, RaceDifficultyInputViewModel>()
                .ForMember(x => x.DifficultyName, opt
                    => opt.MapFrom(x => x.Difficulty.Level.ToString()))
                .ForMember(x => x.ControlTime, opt
                    => opt.MapFrom(x => x.ControlTime.TotalHours));
        }
    }
}
