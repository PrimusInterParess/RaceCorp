﻿namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.CommonViewModels;
    using RaceCorp.Web.ViewModels.HomeViewModels;

    public class MountainService : IMountanService
    {
        private readonly IDeletableEntityRepository<Mountain> mountainsRepo;

        public MountainService(IDeletableEntityRepository<Mountain> mountainsRepo)
        {
            this.mountainsRepo = mountainsRepo;
        }

        public IEnumerable<KeyValuePair<string, string>> GetMountainsKVP()
        {
            return this.mountainsRepo.All()
               .Select(f => new MountainViewModel()
               {
                   Id = f.Id,
                   Name = f.Name,
               }).Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name));
        }

        public HashSet<MountainViewModel> GetMountains()
        {
            return this.mountainsRepo.All().Select(t => new MountainViewModel
            {
                Id = t.Id,
                Name = t.Name,
            }).ToHashSet();
        }
    }
}
