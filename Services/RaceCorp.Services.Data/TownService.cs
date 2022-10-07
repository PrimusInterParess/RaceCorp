namespace RaceCorp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.RaceViewModels;
    using RaceCorp.Web.ViewModels.Ride;
    using RaceCorp.Web.ViewModels.Town;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class TownService : ITownService
    {
        private readonly IDeletableEntityRepository<Town> townsRepo;
        private readonly IImageService imageService;

        public TownService(IDeletableEntityRepository<Town> townsRepo, IImageService imageService)
        {
            this.townsRepo = townsRepo;
            this.imageService = imageService;
        }

        public TownRacesProfileViewModel AllRaces(int townId, int pageId, int itemsPerPage = 3)
        {
            var town = this.townsRepo.AllAsNoTracking()
               .Include(t => t.Races)
               .ThenInclude(r => r.Mountain)
               .Include(r => r.Races)
               .ThenInclude(r => r.Logo)
               .FirstOrDefault(t => t.Id == townId);

            var count = town.Races.Count();

            var races = town.Races.Select(r => new RaceInAllViewModel()
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

            return new TownRacesProfileViewModel()
            {
                Races = raceData,
                Id = town.Id,
                Name = town.Name,
            };
        }

        public TownRidesProfileViewModel AllRides(int townId, int pageId, int itemsPerPage = 3)
        {
            var town = this.townsRepo.AllAsNoTracking()
                .Include(t => t.Rides)
                .ThenInclude(r => r.Mountain)
                .Include(r => r.Rides)
                .ThenInclude(r => r.Trace)
                .FirstOrDefault(t => t.Id == townId);

            var count = town.Rides.Count();

            var rides = town.Rides.Select(r => new RideInAllViewModel()
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                TrackUrl = r.Trace.TrackUrl,
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

            return new TownRidesProfileViewModel()
            {
                Rides = rideData,
                Id = town.Id,
                Name = town.Name,
            };
        }

        public async Task Create(TownCreateViewModel model)
        {
            var alreadyExists = this.townsRepo.All().Any(t => t.Name == model.Name);

            if (alreadyExists)
            {
                throw new Exception(TownNameAlreadyExists);
            }
            else
            {
                var town = new Town()
                {
                    Name = model.Name,
                };

                try
                {
                    await this.townsRepo.AddAsync(town);
                    await this.townsRepo.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw new Exception(IvalidOperationMessage);
                }
            }
        }

        public List<T> GetAll<T>()
        {
            return this
                .townsRepo
                .AllAsNoTracking()
                .To<T>()
                .ToList();
        }

        public IEnumerable<KeyValuePair<string, string>> GetTownsKVP()
        {
            return this.townsRepo.All()
               .Select(f => new TownViewModel()
               {
                   Id = f.Id,
                   Name = f.Name,
               }).Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name));
        }

        public async Task SaveImage(PictureUploadModel model, string userId, string imagePath)
        {
            try
            {
                var image = this.imageService.ProccessingImageData(model.Picture, userId);

                image.Name = TownImageName;

                await this.imageService
                     .SaveImageIntoFileSystem(
                         model.Picture,
                         imagePath,
                         TownFolderName,
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
