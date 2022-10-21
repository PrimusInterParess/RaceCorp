namespace RaceCorp.Services.Data
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using static System.Net.Mime.MediaTypeNames;

    public class GpxService : IGpxService
    {
        private readonly string[] allowedExtensions = new[] { "gpx" };

        public Gpx ProccessingData(IFormFile file, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveAsyncIntoDb(Gpx fileData)
        {
            throw new System.NotImplementedException();
        }

        public async Task SaveIntoFileSystem(IFormFile file, string filePath, string folderName, string fileDbId, string extension)
        {
            Directory.CreateDirectory($"{filePath}/{folderName}/");

            var physicalPath = $"{filePath}/{folderName}/{fileDbId}.{extension}";
            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        public bool ValidateExtension(string extension)
        {
            return this.allowedExtensions.Any(x => extension.EndsWith(x));
        }
    }
}
