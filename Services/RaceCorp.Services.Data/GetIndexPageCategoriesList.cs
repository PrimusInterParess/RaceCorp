namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Data.Models.Enums;
    using RaceCorp.Web.ViewModels.HomeViewModels;

    public class GetIndexPageCategoriesList : IGetIndexPageCategoriesList
    {
        private readonly IDeletableEntityRepository<Town> townsRepo;
        private readonly IDeletableEntityRepository<Difficulty> difficultiesRepo;
        private readonly IDeletableEntityRepository<Mountain> mountansRepo;
        private readonly IDeletableEntityRepository<Format> formatRepo;

        public GetIndexPageCategoriesList(
            IDeletableEntityRepository<Town> townsRepo,
            IDeletableEntityRepository<Difficulty> difficultiesRepo,
            IDeletableEntityRepository<Mountain> mountansRepo,
            IDeletableEntityRepository<Format> formatRepo)
        {
            this.townsRepo = townsRepo;
            this.difficultiesRepo = difficultiesRepo;
            this.mountansRepo = mountansRepo;
            this.formatRepo = formatRepo;
        }

        public IndexViewModel GetCategories()
        {
            var towns = this.townsRepo.All().Select(t => new TownIndexViewModel
            {
                Id = t.Id,
                Name = t.Name,
            }).ToHashSet();

            var mountains = this.mountansRepo.All().Select(m => new MountainIndexViewModel
            {
                Id = m.Id,
                Name = m.Name,
            }).ToHashSet();

            var formats = this.formatRepo.All().Select(f => new FormatIndexViewModel
            {
                Id = f.Id,
                Name = f.Name,
            }).ToHashSet();

            var difficulties = this.difficultiesRepo.All().Select(d => new DifficultyIndexViewModel
            {
                Id = d.Id,
                Name = d.Level.ToString(),
            }).ToHashSet();

            return new IndexViewModel
            {
                Towns = towns.Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.Name)),
                Mountains = mountains.Select(m => new KeyValuePair<string, string>(m.Id.ToString(), m.Name)),
                Formats = formats.Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name)),
                Difficulties = difficulties.Select(d => new KeyValuePair<string, string>(d.Id.ToString(), d.Name)),
            };
        }

    }
}
