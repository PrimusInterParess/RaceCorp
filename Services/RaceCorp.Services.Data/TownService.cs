namespace RaceCorp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.HomeViewModels;

    using static RaceCorp.Services.Constants.Common;

    public class TownService : ITownService
    {
        private readonly IDeletableEntityRepository<Town> townsRepo;
        private readonly IImageService imageService;

        public TownService(IDeletableEntityRepository<Town> townsRepo, IImageService imageService)
        {
            this.townsRepo = townsRepo;
            this.imageService = imageService;
        }

        public HashSet<TownViewModel> GetTowns()
        {
            return this.townsRepo.All().Select(t => new TownViewModel
            {
                Id = t.Id,
                Name = t.Name,
            }).ToHashSet();
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
