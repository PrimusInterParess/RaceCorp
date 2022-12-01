namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;

    public class FormatService : IFormatServices
    {
        private readonly IDeletableEntityRepository<Format> formatRepo;

        public FormatService(IDeletableEntityRepository<Format> formatRepo)
        {
            this.formatRepo = formatRepo;
        }

        public HashSet<FormatViewModel> GetFormats()
        {
            return this.formatRepo
                .All()
                .Select(f => new FormatViewModel()
            {
                Id = f.Id,
                Name = f.Name,
            }).ToHashSet();
        }

        public IEnumerable<KeyValuePair<string, string>> GetFormatKVP()
        {
            return this.formatRepo.All()
                .Select(f => new FormatViewModel()
            {
                Id = f.Id,
                Name = f.Name,
            }).Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name));
        }
    }
}
