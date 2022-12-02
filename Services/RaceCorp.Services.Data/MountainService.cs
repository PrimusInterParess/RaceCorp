namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.Mountain;
    using RaceCorp.Web.ViewModels.RaceViewModels;
    using RaceCorp.Web.ViewModels.Ride;
    using RaceCorp.Web.ViewModels.Town;

    using static RaceCorp.Services.Constants.Common;

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
               .Select(m => new MountainViewModel()
               {
                   Id = m.Id,
                   Name = m.Name,
               }).Select(m => new KeyValuePair<string, string>(m.Id.ToString(), m.Name));
        }

        public HashSet<MountainViewModel> GetMountains()
        {
            return this.mountainsRepo
                .All()
                .Select(m => new MountainViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                }).ToHashSet();
        }

        public List<T> GetAll<T>()
        {
            return this
                 .mountainsRepo
                 .AllAsNoTracking()
                 .OrderBy(m => m.Name)
                 .Where(m => m.Rides.Count() != 0 || m.Races.Count() != 0)
                 .To<T>()
                 .ToList();
        }

        public MountainRidesProfileViewModel AllRides(int mountainId, int pageId, int itemsPerPage = 3)
        {
            var mountain = this.mountainsRepo
               .AllAsNoTracking()
               .Include(m => m.Rides)
               .ThenInclude(r => r.Town)
               .Include(m => m.Rides)
               .ThenInclude(r => r.Trace)
               .ThenInclude(t => t.Gpx)
               .FirstOrDefault(m => m.Id == mountainId);

            var count = mountain.Rides.Count();

            var rides = mountain
                .Rides
                .OrderBy(r => r.Date)
                .Select(r => new RideInAllViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    TraceMapUrl = r.Trace.MapUrl,
                    TownName = r.Town.Name,
                    MountainName = r.Mountain.Name,
                })
                .Skip((pageId - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            var rideData = new RideAllViewModel()
            {
                PageNumber = pageId,
                ItemsPerPage = itemsPerPage,
                RacesCount = count,
                Rides = rides,
            };

            return new MountainRidesProfileViewModel()
            {
                Rides = rideData,
                Id = mountain.Id,
                Name = mountain.Name,
            };
        }

        public MountainRacesProfileViewModel AllRaces(int mountainId, int pageId, int itemsPerPage = GlobalIntValues.ItemsPerPage)
        {
            var mountain = this.mountainsRepo
              .AllAsNoTracking()
              .Include(t => t.Races)
              .ThenInclude(r => r.Town)
              .Include(r => r.Races)
              .ThenInclude(r => r.Logo)
              .FirstOrDefault(t => t.Id == mountainId);

            var count = mountain.Races.Count();

            var races = mountain
                .Races
                .OrderBy(r => r.Date)
                .Select(r => new RaceInAllViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    LogoPath = LogoRootPath + r.LogoId + "." + r.Logo.Extension,
                    Town = r.Town.Name,
                    Mountain = r.Mountain.Name,
                })
           .Skip((pageId - 1) * itemsPerPage)
           .Take(itemsPerPage)
           .ToList();

            var raceData = new RaceAllViewModel()
            {
                PageNumber = pageId,
                ItemsPerPage = itemsPerPage,
                RacesCount = count,
                Races = races,
            };

            return new MountainRacesProfileViewModel()
            {
                Races = raceData,
                Id = mountain.Id,
                Name = mountain.Name,
            };
        }

        public async Task<Mountain> ProccesingData(string name)
        {
            var mountainDb = this.mountainsRepo
                .All()
                .FirstOrDefault(t => t.Name.ToLower() == name.ToLower());

            if (mountainDb == null)
            {
                mountainDb = new Mountain
                {
                    Name = name,
                    CreatedOn = DateTime.Now,
                };

                await this.mountainsRepo.AddAsync(mountainDb);
            }

            return mountainDb;
        }
    }
}
