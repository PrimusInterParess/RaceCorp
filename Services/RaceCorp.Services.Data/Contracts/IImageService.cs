namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.Common;

    public interface IImageService
    {
        void Process(IFormFile inputImage, string roothPath, string imageParentFolderName, string folderName, string userId);

        bool ValidateImageExtension(string extension);

        Task SaveImageIntoFileSystem(IFormFile imageInputFile, string imagePath, string folderName, string imageDbId, string extension);

        Image ProccessingData(IFormFile imageInputFile, string userId);

        Task SaveAsyncImageIntoDb(Image imageData);

        Task SaveImageAsync(PictureUploadModel model, string userId, string imagePath, string folderName, string imageName);
    }
}
