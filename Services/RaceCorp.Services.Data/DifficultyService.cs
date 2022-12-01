namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;

    public class DifficultyService : IDifficultyService
    {
        private readonly IDeletableEntityRepository<Difficulty> difficultiesRepo;

        public DifficultyService(IDeletableEntityRepository<Difficulty> difficultiesRepo)
        {
            this.difficultiesRepo = difficultiesRepo;
        }

        public HashSet<DifficultyViewModel> GetDifficulties()
        {
            return this.difficultiesRepo
                .All()
                .Select(d => new DifficultyViewModel
                {
                    Id = d.Id,
                    Level = d.Level.ToString(),
                }).ToHashSet();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDifficultiesKVP()
        {
            return this.difficultiesRepo.All()
                .Select(d => new DifficultyViewModel()
                {
                    Id = d.Id,
                    Level = d.Level.ToString(),
                }).Select(d => new KeyValuePair<string, string>(d.Id.ToString(), d.Level));
        }
    }
}
