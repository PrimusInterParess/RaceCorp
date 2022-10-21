namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;

    public interface IGpxService
    {
        bool ValidateExtension(string extension);

        Task SaveIntoFileSystem(IFormFile file, string filePath, string folderName, string fileDbId, string extension);

        Gpx ProccessingData(IFormFile file, string userId);

        Task SaveAsyncIntoDb(Gpx fileData);

    }
}
