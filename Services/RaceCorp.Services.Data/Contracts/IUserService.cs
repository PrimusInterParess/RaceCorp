namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.User;

    public interface IUserService
    {
        T GetById<T>(string id);

        UserProfileViewModel GetProfileModelById(string id, string currentUserId);

        UserAllRequestsViewModel GetRequestsModel(string userId);

        Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath);

        List<T> GetAllAsync<T>();

        List<UserAllViewModel> GetAllAsyncHomePage(string currentUserId);

        string GetUserEmail(string userId);
    }
}
