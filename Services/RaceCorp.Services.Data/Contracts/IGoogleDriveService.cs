namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IGoogleDriveService
    {
        Task<string> UloadGpxFileToDrive(string gpxFilePath, string uploadFileName);
    }
}
