namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        bool ValidateImageExtension(string extension);

        Task SaveImageIntoFileSystem(IFormFile image, string imagePath, string folderName, string imageDbId, string extension);
    }
}
