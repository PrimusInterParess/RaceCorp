namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;

    using RaceCorp.Web.ViewModels.HomeViewModels;

    public class GetIndexPageCategoriesList : IGetIndexPageCategoriesList
    {
        private readonly IDifficultiesServiceList getDifficultiesServiceList;
        private readonly IFormatServicesList formatServicesList;
        private readonly IDeletableEntityRepository<Town> townsRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainsRepo;

        public GetIndexPageCategoriesList(
            IDifficultiesServiceList getDifficultiesServiceList,
            IFormatServicesList formatServicesList,
            IDeletableEntityRepository<Town> townsRepo,
            IDeletableEntityRepository<Mountain> mountainsRepo)
        {
            this.getDifficultiesServiceList = getDifficultiesServiceList;
            this.formatServicesList = formatServicesList;
            this.townsRepo = townsRepo;
            this.mountainsRepo = mountainsRepo;
        }

        public IndexViewModel GetCategories()
        {
            var towns = this.townsRepo.All().Select(t => new TownIndexViewModel
            {
                Id = t.Id,
                Name = t.Name,
            }).ToHashSet();

            var mountains = this.mountainsRepo.All().Select(m => new MountainIndexViewModel
            {
                Id = m.Id,
                Name = m.Name,
            }).ToHashSet();

            var formats = this.formatServicesList.GetFormats();

            var difficulties = this.getDifficultiesServiceList.GetDifficulties();

            return new IndexViewModel
            {
                Towns = towns.Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.Name)),
                Mountains = mountains.Select(m => new KeyValuePair<string, string>(m.Id.ToString(), m.Name)),
                Formats = formats.Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name)),
                Difficulties = difficulties.Select(d => new KeyValuePair<string, string>(d.Id.ToString(), d.Level)),
            };
        }
    }
}
