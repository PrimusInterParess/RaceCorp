namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Web.ViewModels.ApplicationUsers;

    public interface IUserService
    {
        public T GetById<T>(string id);

        Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath);

        List<T> GetRequest<T>(string userId);

        Task<bool> ProccessRequestAsync(int requestId, string userId);
    }
}
