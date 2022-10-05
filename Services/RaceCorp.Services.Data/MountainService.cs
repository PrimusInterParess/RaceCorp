namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.CommonViewModels;

    using static RaceCorp.Services.Constants.Common;

    public class MountainService : IMountanService
    {
        private readonly IDeletableEntityRepository<Mountain> mountainsRepo;
        private readonly IImageService imageService;


        public MountainService(IDeletableEntityRepository<Mountain> mountainsRepo, IImageService imageService)
        {
            this.mountainsRepo = mountainsRepo;
            this.imageService = imageService;
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

        public async Task SaveImageAsync(PictureUploadModel model, string userId, string imagePath)
        {
            try
            {
                var image = this.imageService.ProccessingImageData(model.Picture, userId);

                image.Name = MountainImageName;

                await this.imageService
                     .SaveImageIntoFileSystem(
                         model.Picture,
                         imagePath,
                         MountainFolderName,
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
