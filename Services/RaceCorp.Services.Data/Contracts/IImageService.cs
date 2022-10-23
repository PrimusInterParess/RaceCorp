namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;

    public interface IImageService
    {
        bool ValidateImageExtension(string extension);

        Task SaveImageIntoFileSystem(IFormFile imageInputFile, string imagePath, string folderName, string imageDbId, string extension);

        Image ProccessingData(IFormFile imageInputFile, string userId);

        Task SaveAsyncImageIntoDb(Image imageData);
    }
}
