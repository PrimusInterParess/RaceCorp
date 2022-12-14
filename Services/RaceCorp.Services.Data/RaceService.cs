namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.RaceViewModels;

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

        public async Task CreateAsync(RaceCreateModel model, string userId)
        {
            var race = new Race
            {
                Name = model.Name,
                Date = model.Date,
                Description = model.Description,
                FormatId = int.Parse(model.FormatId),
                ApplicationUserId = userId,
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

            try
            {
                var logo = await this.logoService
                .ProccessingData(
                model.RaceLogo,
                userId);

                race.LogoPath = $"/{logo.ParentFolderName}/{logo.ChildFolderName}/{logo.Id}.{logo.Extension}";
                race.Logo = logo;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            if (model.Traces.Count != 0)
            {
                foreach (var traceInputModel in model.Traces)
                {
                    try
                    {
                        var gpx = await this.gpxService
                        .ProccessingData(
                        traceInputModel.GpxFile,
                        userId,
                        model.Name);

                        var trace = await this.traceService
                        .ProccedingData(traceInputModel, gpx.GoogleDriveId);

                        trace.GpxPath = $"/{gpx.ParentFolderName}/{gpx.ChildFolderName}/{gpx.Id}.{gpx.Extension}";
                        trace.Gpx = gpx;

                        race.Traces.Add(trace);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException(e.Message);
                    }
                }
            }

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

        public RaceAllViewModel All(
            int page,
            int itemsPerPage = GlobalIntValues.ItemsPerPage)
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
                    LogoPath = r.LogoPath,
                    Town = r.Town.Name,
                    Mountain = r.Mountain.Name,
                    Date = r.Date.ToString(GlobalConstants.DateStringFormat),
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

        public async Task EditAsync(RaceEditViewModel model,string userId)
        {
            var raceDb = this.raceRepo
                .All()
                .FirstOrDefault(r => r.Id == model.Id);

            if (raceDb == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRaceId);
            }

            raceDb.Name = model.Name;
            raceDb.Description = model.Description;
            raceDb.FormatId = int.Parse(model.FormatId);

            if (model.RaceLogo != null)
            {
                try
                {
                    var logo = await this.logoService
                    .ProccessingData(
                    model.RaceLogo,
                    userId);

                    raceDb.LogoPath = $"/{logo.ParentFolderName}/{logo.ChildFolderName}/{logo.Id}.{logo.Extension}";
                    raceDb.Logo = logo;
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(e.Message);
                }
            }

            var mountainDb = await this.mountanService
                .ProccesingData(model.Mountain);

            var townDb = await this.townService
                .ProccesingData(model.Town);

            raceDb.Mountain = mountainDb;
            raceDb.Town = townDb;

            await this.raceRepo.SaveChangesAsync();
        }

        public RaceAllViewModel GetUpcomingRaces(int page, int itemsPerPage = GlobalIntValues.ItemsPerPage)
        {
            var count = this.raceRepo
                 .All()
                 .Where(r => r.Date > DateTime.Now)
                 .Count();

            var races = this.raceRepo
                .AllAsNoTracking()
                .OrderBy(x => x.Date)
                .Where(r => r.Date > DateTime.Now)
                .OrderBy(r => r.CreatedOn)
                .ThenBy(r => r.Name)
                .Select(r => new RaceInAllViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    LogoPath = r.LogoPath,
                    Town = r.Town.Name,
                    Mountain = r.Mountain.Name,
                    Date = r.Date.ToString(GlobalConstants.DateStringFormat),
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
            var race = this.raceRepo
                .All()
                .FirstOrDefault(r => r.Id == id);

            this.raceRepo.Delete(race);

            var result = await this.raceRepo.SaveChangesAsync();

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public void UpdateInfo(RaceProfileViewModel raceModel, ApplicationUser user)
        {
            if (user != null)
            {
                raceModel.IsRegistered = raceModel.RegisteredUsers.Any(u => u.ApplicationUserId == user.Id);
                raceModel.IsOwner = raceModel.ApplicationUserId == user.Id;
            }
            else
            {
                raceModel.IsRegistered = false;
            }

            raceModel.HasPassed = DateTime.Parse(raceModel.Date) < DateTime.Now;
        }
    }
}
