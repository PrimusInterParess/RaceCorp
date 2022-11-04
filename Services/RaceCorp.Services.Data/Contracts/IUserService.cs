namespace RaceCorp.Services.Data.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Web.ViewModels.ApplicationUsers;

    public interface IUserService
    {
        public T GetById<T>(string id);

        Task<bool> SaveProfileImage(IFormFile inputFile, string userId, string roothPath);

        Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath, ClaimsPrincipal claimPrincipal);
    }
}
