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
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IImageService imageService;

        public RaceService(
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Trace> difficultyRepo,
            IDeletableEntityRepository<Town> townRepo,
            IImageService imageService)
        {
            this.raceRepo = raceRepo;
            this.mountainRepo = mountainRepo;
            this.townRepo = townRepo;
            this.imageService = imageService;
        }

        public async Task CreateAsync(
            RaceCreateModel model,
            string imagePath,
            string userId)
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

                await this.mountainRepo.AddAsync(mountainData);
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

                await this.townRepo.AddAsync(townData);
            }

            race.Town = townData;

            if (model.Difficulties.Count != 0)
            {
                foreach (var trace in model.Difficulties)
                {
                    var traceData = new Trace()
                    {
                        Name = trace.Name,
                        ControlTime = TimeSpan.FromHours((double)trace.ControlTime),
                        DifficultyId = trace.DifficultyId,
                        Length = (int)trace.Length,
                        StartTime = (DateTime)trace.StartTime,
                        TrackUrl = trace.TrackUrl,
                    };

                    race.Traces.Add(traceData);
                }
            }

            var extension = string.Empty;

            try
            {
                extension = Path.GetExtension(model.RaceLogo.FileName).TrimStart('.');
            }
            catch (Exception)
            {
                throw new Exception(LogoImageRequired);
            }

            var validateImageExtension = this.imageService.ValidateImageExtension(extension);

            if (validateImageExtension == false)
            {
                throw new Exception(InvalidImageExtension + extension);
            }

            if (model.RaceLogo.Length > 10 * 1024 * 1024)
            {
                throw new Exception(InvalidImageSize);
            }

            var logo = new Logo()
            {
                Extension = extension,
                UserId = userId,
            };

            await this.imageService
                 .SaveImageIntoFileSystem(
                     model.RaceLogo,
                     imagePath,
                     LogosFolderName,
                     logo.Id,
                     extension);

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
            var count = this.raceRepo.All().Count();
            var races = this.raceRepo.AllAsNoTracking().Select(r => new RaceInAllViewModel()
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                LogoPath = LogoRootPath + r.LogoId + "." + r.Logo.Extension,
                Town = r.Town.Name,
                Mountain = r.Mountain.Name,
            }).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

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
            return this.raceRepo.All().Count();
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
            return this.raceRepo.AllAsNoTracking().Any(r => r.Id == id);
        }

        public async Task EditAsync(RaceEditViewModel model, string logoPath, string userId)
        {
            // TODO: take out the image logic out of Edit/Create methods into private method
            // TODO: take out common login from Edit/Create methods such as mountain/town ex
            var race = this.raceRepo.All().FirstOrDefault(r => r.Id == model.Id);

            race.Name = model.Name;
            race.Description = model.Description;
            race.FormatId = int.Parse(model.FormatId);

            if (model.RaceLogo != null)
            {
                var extension = string.Empty;

                try
                {
                    extension = Path.GetExtension(model.RaceLogo.FileName).TrimStart('.');
                }
                catch (Exception)
                {
                    throw new Exception(LogoImageRequired);
                }

                var validateImageExtension = this.imageService.ValidateImageExtension(extension);

                if (validateImageExtension == false)
                {
                    throw new Exception(InvalidImageExtension + extension);
                }

                if (model.RaceLogo.Length > 10 * 1024 * 1024)
                {
                    throw new Exception(InvalidImageSize);
                }

                var logo = new Logo()
                {
                    Extension = extension,
                    UserId = userId,
                };

                await this.imageService
                     .SaveImageIntoFileSystem(
                         model.RaceLogo,
                         logoPath,
                         LogosFolderName,
                         logo.Id,
                         extension);

                race.Logo = logo;
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

                await this.townRepo.AddAsync(townData);
            }

            race.Town = townData;

            await this.raceRepo.SaveChangesAsync();
        }

        public RaceInAllViewModel GetUpcommingEvents()
        {
            var raceData = this.raceRepo.All().Where(r => r.Date > DateTime.Now).FirstOrDefault();

            return new RaceInAllViewModel();
        }

        public async Task SaveImageAsync(PictureUploadModel model, string userId, string imagePath)
        {
            try
            {
                var image = this.imageService.ProccessingImageData(model.Picture, userId);

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
    }
}
