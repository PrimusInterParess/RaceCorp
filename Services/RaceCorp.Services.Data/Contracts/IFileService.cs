namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;

    public interface IFileService
    {
        string ValidateFile(IFormFile file, string expectedFileType);

        Task SaveFileIntoFileSystem(
            IFormFile file,
            string roothPath,
            string folderName,
            string dbId,
            string extension);

        Task<Logo> ProccessingLogoData(IFormFile file, string userId, string imagePath);

        Task<Image> ProccessingImageData(IFormFile file, string userId, string roothPath, string childrenFolderName);
    }
}
