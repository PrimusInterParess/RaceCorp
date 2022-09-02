namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    public class DifficultiesServiceList : IDifficultiesServiceList
    {
        private readonly IDeletableEntityRepository<Difficulty> difficultiesRepo;

        public DifficultiesServiceList(IDeletableEntityRepository<Difficulty> difficultiesRepo)
        {
            this.difficultiesRepo = difficultiesRepo;
        }

        public HashSet<DifficultyViewModel> GetDifficulties()
        {
            return this.difficultiesRepo.All().Select(d => new DifficultyViewModel
            {
                Id = d.Id,
                Level = d.Level.ToString(),
            }).ToHashSet();
        }
    }
}
