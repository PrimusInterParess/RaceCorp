using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceCorp.Data.Common.Repositories;
using RaceCorp.Data.Models;
using RaceCorp.Web.ViewModels.FormatViewModels;
using RaceCorp.Web.ViewModels.HomeViewModels;

namespace RaceCorp.Services.Data
{
    public class FormatServiceList:IFormatServicesList
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
