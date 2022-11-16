namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IConversationService
    {
        Task CheckForExisting(string name);
    }
}
