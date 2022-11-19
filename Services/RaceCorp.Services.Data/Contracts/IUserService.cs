namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Web.ViewModels.ApplicationUsers;
    using RaceCorp.Web.ViewModels.Common;

    public interface IUserService
    {
        public T GetById<T>(string id);

        Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath);

        List<T> GetRequest<T>(string userId);

        List<T> GetAllAsync<T>();

        Task AddAsync(string currentUserId, string targetUserId);

        MessageInputModel GetMessageModelAsync(string receiverId, string senderId);

        Task SaveMessageAsync(MessageInputModel model, string senderId);

        UserInboxViewModel GetByIdUserInboxViewModel(string id);
    }
}
