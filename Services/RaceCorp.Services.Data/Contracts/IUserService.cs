namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IUserService
    {
        public T GetById<T>(string id);

        Task<bool> SaveProfileImage(IFormFile inputFile, string userId, string roothPath);
    }
}
