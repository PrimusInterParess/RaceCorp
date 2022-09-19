using System.Collections.Generic;

namespace RaceCorp.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Security;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.AspNetCore.Hosting;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class RaceService : IRaceService
    {
        private const string LogosFolderName = "logos";
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Difficulty> difficultyRepo;
        private readonly IRepository<RaceDifficulty> traceRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IImageService imageService;

        public RaceService(
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Difficulty> difficultyRepo,
            IRepository<RaceDifficulty> traceRepo,
            IDeletableEntityRepository<Town> townRepo,
            IImageService imageService)
        {
            this.raceRepo = raceRepo;
            this.mountainRepo = mountainRepo;
            this.difficultyRepo = difficultyRepo;
            this.traceRepo = traceRepo;
            this.townRepo = townRepo;
            this.imageService = imageService;
        }

        public async Task CreateAsync(
            RaceCreateInputViewModel model,
            string imagePath,
            string userId)
        {
            var race = new Race();
            race.Name = model.Name;
            race.Date = model.Date;
            race.Description = model.Description;
            race.FormatId = int.Parse(model.FormatId);
            race.UserId = userId;

            var mountainData = this.mountainRepo.All().FirstOrDefault(m => m.Name.ToLower() == model.Mountain.ToLower());

            if (mountainData == null)
            {
                mountainData = new Mountain()
                {
                    Name = model.Mountain,
                };

                await this.mountainRepo.AddAsync(mountainData);
            }

            race.Mountain = mountainData;

            var townData = this.townRepo.All().FirstOrDefault(t => t.Name.ToLower() == model.Town.ToLower());

            if (townData == null)
            {
                townData = new Town()
                {
                    Name = model.Town,
                };

                await this.townRepo.AddAsync(townData);
            }

            race.Town = townData;

            foreach (var trace in model.Difficulties)
            {
                var raceTrace = new RaceDifficulty()
                {
                    ControlTime = TimeSpan.FromHours((double)trace.ControlTime),
                    DifficultyId = int.Parse(trace.DifficultyId),
                    Length = (int)trace.Length,
                    Race = race,
                    StartTime = (DateTime)trace.StartTime,
                    TrackUrl = trace.TrackUrl,
                };

                race.Traces.Add(raceTrace);
            }

            var extension = Path.GetExtension(model.RaceLogo.FileName).TrimStart('.');

            var validateImageExtension = this.imageService.ValidateImageExtension(extension);

            if (validateImageExtension == false)
            {
                throw new Exception($"Invalid image extension {extension}");
            }

            if (model.RaceLogo.Length > 10 * 1024 * 1024)
            {
                throw new Exception("invalid file size. It needs to be max 10mb.");
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

        public List<RaceViewModel> All()
        {
            return this.raceRepo.AllAsNoTracking().Select(r => new RaceViewModel()
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                LogoPath = "/images/logos/" + r.LogoId + "." + r.Logo.Extension,
                Town = r.Town.Name,
                TownId = r.TownId,
                Mountain = r.Mountain.Name,
                MountainId = r.MountainId,
            })
                .OrderByDescending(r => r.Id)
                .ToList();
        }
    }
}
