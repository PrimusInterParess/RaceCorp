namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class RaceService : IRaceService
    {
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Trace> traceRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IRepository<Gpx> gpxRepo;
        private readonly IRepository<Logo> logoRepo;
        private readonly IImageService imageService;
        private readonly ITraceService traceService;
        private readonly IGpxService gpxService;
        private readonly ILogoService logoService;

        public RaceService(
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Trace> traceRepo,
            IDeletableEntityRepository<Town> townRepo,
            IRepository<Gpx> gpxRepo,
            IRepository<Logo> logoRepo,
            IImageService imageService,
            ITraceService traceService,
            IGpxService gpxService,
            ILogoService logoService)
        {
            this.raceRepo = raceRepo;
            this.mountainRepo = mountainRepo;
            this.traceRepo = traceRepo;
            this.townRepo = townRepo;
            this.gpxRepo = gpxRepo;
            this.logoRepo = logoRepo;
            this.imageService = imageService;
            this.traceService = traceService;
            this.gpxService = gpxService;
            this.logoService = logoService;
        }

        public async Task CreateAsync(
            RaceCreateModel model,
            string roothPath,
            string imageParentFolderName,
            string userId,
            string gpxFolderName,
            string serviceAccountFolderName,
            string sereviceAccountKeyFileName)
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

            if (model.Difficulties.Count != 0)
            {
                var gpxRoothPath = $"{roothPath}\\{gpxFolderName}";
                var serviceAccountPath = $"{roothPath}\\{serviceAccountFolderName}\\{sereviceAccountKeyFileName}";

                foreach (var traceInputModel in model.Difficulties)
                {
                    var gpx = await this.gpxService
                        .ProccessingData(
                        traceInputModel.GpxFile,
                        userId,
                        model.Name,
                        gpxRoothPath,
                        serviceAccountPath);

                    await this.gpxRepo.AddAsync(gpx);

                    var trace = this.traceService
                        .GetTraceDbModel(traceInputModel, gpx);

                    await this.traceRepo.AddAsync(trace);

                    race.Traces.Add(trace);
                }
            }
            //TODO: FINISH HIM!
            var logo = await this.logoService
                .ProccessingData(
                model.RaceLogo,
                userId,
                imagePath);

            await this.logoRepo.AddAsync(logo);

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

        public async Task EditAsync(RaceEditViewModel model, string logoPath, string userId)
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
                 logoPath);

                await this.logoRepo.AddAsync(logo);

                raceDb.Logo = logo;
            }

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

                await this.mountainRepo.AddAsync(mountainData);
            }

            raceDb.Mountain = mountainData;

            var townData = this
                .townRepo.All()
                .FirstOrDefault(t => t.Name.ToLower() == model.Town.ToLower());

            if (townData == null)
            {
                townData = new Town()
                {
                    Name = model.Town,
                };

                await this.townRepo.AddAsync(townData);
            }

            raceDb.Town = townData;

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

        public async Task SaveImageAsync(PictureUploadModel model, string userId, string imagePath)
        {
            try
            {
                var image = this.imageService.ProccessingData(model.Picture, userId);

                image.Name = UpcommingRaceImageName;

                await this.imageService
                     .SaveImageIntoFileSystem(
                         model.Picture,
                         imagePath,
                         UpcommingRaceFolderName,
                         image.Id,
                         image.Extension);

                await this.imageService.SaveAsyncImageIntoDb(image);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
