namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.Message;
    using RaceCorp.Web.ViewModels.User;

    public interface IMessageService
    {
        Task<List<T>> GetMessages<T>(string authorId, string interlocutorId);

        UserInboxViewModel GetByIdUserInboxViewModel(string id);

        MessageInputModel GetMessageModelAsync(string receiverId, string senderId);

        Task<Message> SaveMessageAsync(MessageInputModel model, string senderId);
    }
}
