namespace RaceCorp.Services.Data
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using RaceCorp.Services.Data.Contracts;

    public class ImageService : IImageService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

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
    }
}
