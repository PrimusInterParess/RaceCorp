namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.FormatViewModels;

    public class FormatServiceList : IFormatServicesList
    {
        private readonly IDeletableEntityRepository<Format> formatRepo;

        public FormatServiceList(IDeletableEntityRepository<Format> formatRepo)
        {
            this.formatRepo = formatRepo;
        }

        public HashSet<FormatViewModel> GetFormats()
        {
            return this.formatRepo.All().Select(f => new FormatViewModel()
            {
                Id = f.Id,
                Name = f.Name,
            }).ToHashSet();
        }
    }
}
