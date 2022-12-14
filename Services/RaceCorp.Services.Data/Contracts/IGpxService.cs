namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;

    public interface IGpxService
    {
        Task<Gpx> ProccessingData(IFormFile file, string userId, string childrenFolderName);

        Gpx GetGpxById(string id);
    }
}
