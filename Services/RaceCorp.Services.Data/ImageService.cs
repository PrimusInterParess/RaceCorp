namespace RaceCorp.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class ImageService : IImageService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };
        private readonly IRepository<Image> imageRepo;

        public ImageService(IRepository<Image> imageRepo)
        {
            this.imageRepo = imageRepo;
        }

        public bool ValidateImageExtension(string extension)
        {
            return this.allowedExtensions.Any(x => extension.EndsWith(x));
        }

        public async Task SaveImageIntoFileSystem(
            IFormFile image,
            string imagePath,
            string folderName,
            string imageDbId,
            string extension)
        {
            Directory.CreateDirectory($"{imagePath}/{folderName}/");

            var physicalPath = $"{imagePath}/{folderName}/{imageDbId}.{extension}";
            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await image.CopyToAsync(fileStream);
        }

        public Image ProccessingImageData(IFormFile image, string userId)
        {
            var extension = string.Empty;

            try
            {
                extension = Path.GetExtension(image.FileName).TrimStart('.');
            }
            catch (Exception)
            {
                throw new Exception("Image required!");
            }

            var validateImageExtension = this.ValidateImageExtension(extension);

            if (validateImageExtension == false)
            {
                throw new Exception(InvalidImageExtension + extension);
            }

            if (image.Length > 10 * 1024 * 1024)
            {
                throw new Exception(InvalidImageSize);
            }

            return new Image()
            {
                UserId = userId,
                CreatedOn = DateTime.Now,
                Extension = extension,
            };
        }

        public async Task SaveAsyncImageIntoDb(Image imageData)
        {
            try
            {
                await this.imageRepo.AddAsync(imageData);
                await this.imageRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
