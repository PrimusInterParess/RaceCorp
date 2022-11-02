namespace RaceCorp.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    using static RaceCorp.Services.Constants.Common;

    public class RaceService : IRaceService
    {
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly ITraceService traceService;
        private readonly IGpxService gpxService;
        private readonly ILogoService logoService;
        private readonly IMountanService mountanService;
        private readonly ITownService townService;

        public RaceService(
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Town> townRepo,
            ITraceService traceService,
            IGpxService gpxService,
            ILogoService logoService,
            IMountanService mountanService,
            ITownService townService)
        {
            this.raceRepo = raceRepo;
            this.mountainRepo = mountainRepo;
            this.townRepo = townRepo;
            this.traceService = traceService;
            this.gpxService = gpxService;
            this.logoService = logoService;
            this.mountanService = mountanService;
            this.townService = townService;
        }

        public async Task CreateAsync(RaceCreateModel model, string roothPath, string userId)
        {
            var race = new Race
            {
                Name = model.Name,
                Date = model.Date,
                Description = model.Description,
                FormatId = int.Parse(model.FormatId),
                UserId = userId,
            };

            var mountainData = this
                .mountainRepo
                .All()
                .FirstOrDefault(m => m.Name.ToLower() == model.Mountain.ToLower());

            if (mountainData == null)
            {
                mountainData = new Mountain()
                {
                    Name = model.Mountain,
                };

                await this.mountainRepo
                    .AddAsync(mountainData);
            }

            race.Mountain = mountainData;

            var townData = this
                .townRepo.All()
                .FirstOrDefault(t => t.Name.ToLower() == model.Town.ToLower());

            if (townData == null)
            {
                townData = new Town()
                {
                    Name = model.Town,
                };

                await this.townRepo
                    .AddAsync(townData);
            }

            race.Town = townData;


            var logo = await this.logoService
                .ProccessingData(
                model.RaceLogo,
                userId,
                roothPath);

            if (model.Difficulties.Count != 0)
            {
                var serviceAccountPath = Path.GetFullPath("\\Credentials\\testproject-366105-9ceb2767de2a.json");

                foreach (var traceInputModel in model.Difficulties)
                {
                    var gpx = await this.gpxService
                        .ProccessingData(
                        traceInputModel.GpxFile,
                        userId,
                        model.Name,
                        roothPath,
                        serviceAccountPath);

                    var trace = await this.traceService
                        .ProccedingData(traceInputModel);

                    trace.Gpx = gpx;

                    race.Traces.Add(trace);
                }
            }

            race.Logo = logo;

            try
            {
                await this.raceRepo.AddAsync(race);
                await this.raceRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public RaceAllViewModel All(int page, int itemsPerPage = 3)
        {
            var count = this.raceRepo
                .All()
                .Count();

            var races = this.raceRepo
                .AllAsNoTracking()
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new RaceInAllViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    LogoPath = LogoRootPath + r.LogoId + "." + r.Logo.Extension,
                    Town = r.Town.Name,
                    Mountain = r.Mountain.Name,
                })
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToList();

            return new RaceAllViewModel()
            {
                PageNumber = page,
                ItemsPerPage = itemsPerPage,
                RacesCount = count,
                Races = races,
            };
        }

        public int GetCount()
        {
            return this.raceRepo
                .All()
                .Count();
        }

        public T GetById<T>(int id)
        {
            return this.raceRepo
                .AllAsNoTracking()
                .Where(r => r.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public bool ValidateId(int id)
        {
            return this.raceRepo
                .AllAsNoTracking()
                .Any(r => r.Id == id);
        }

        public async Task EditAsync(RaceEditViewModel model, string roothPath, string userId)
        {
            var raceDb = this.raceRepo.All().FirstOrDefault(r => r.Id == model.Id);

            raceDb.Name = model.Name;
            raceDb.Description = model.Description;
            raceDb.FormatId = int.Parse(model.FormatId);

            if (model.RaceLogo != null)
            {
                var logo = await this.logoService
                 .ProccessingData(
                 model.RaceLogo,
                 userId,
                 roothPath);

                raceDb.Logo = logo;
            }

            var mountainDb = await this.mountanService.ProccesingData(model.Mountain);
            var townDb = await this.townService.ProccesingData(model.Town);

            raceDb.Mountain = mountainDb;
            raceDb.Town = townDb;

            await this.raceRepo.SaveChangesAsync();
        }

        public RaceAllViewModel GetUpcomingRaces(int page, int itemsPerPage = 3)
        {
            var count = this.raceRepo
                 .All()
                 .Count();

            var races = this.raceRepo
                .AllAsNoTracking()
                .Where(r => r.Date > DateTime.Now)
                .OrderBy(r => r.CreatedOn)
                .Select(r => new RaceInAllViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    LogoPath = LogoRootPath + r.LogoId + "." + r.Logo.Extension,
                    Town = r.Town.Name,
                    Mountain = r.Mountain.Name,
                })
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToList();

            return new RaceAllViewModel()
            {
                PageNumber = page,
                ItemsPerPage = itemsPerPage,
                RacesCount = count,
                Races = races,
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var race = this.raceRepo.All().FirstOrDefault(r => r.Id == id);

            this.raceRepo.Delete(race);

            var result = await this.raceRepo.SaveChangesAsync();

            if (result == 0)
            {
                return false;
            }

            return true;
        }
    }
}
