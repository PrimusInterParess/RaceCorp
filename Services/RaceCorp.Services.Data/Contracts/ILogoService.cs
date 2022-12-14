namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;

    public interface ILogoService
    {
        Task<Logo> ProccessingData(IFormFile logoInputFile, string userId);
    }
}
