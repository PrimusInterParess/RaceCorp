namespace RaceCorp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.FormatViewModels;
    using RaceCorp.Web.ViewModels.HomeViewModels;

    public class TownService : ITownService
    {
        private readonly IDeletableEntityRepository<Town> townsRepo;

        public TownService(IDeletableEntityRepository<Town> townsRepo)
        {
            this.townsRepo = townsRepo;
        }

        public HashSet<TownViewModel> GetTowns()
        {
            return this.townsRepo.All().Select(t => new TownViewModel
            {
                Id = t.Id,
                Name = t.Name,
            }).ToHashSet();
        }

        public IEnumerable<KeyValuePair<string, string>> GetTownsKVP()
        {
            return this.townsRepo.All()
               .Select(f => new TownViewModel()
               {
                   Id = f.Id,
                   Name = f.Name,
               }).Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name));
        }
    }
}
