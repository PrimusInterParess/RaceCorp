namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IConnectUserService
    {
        Task RequestConnectUserAsync(string requesterId, string targetUserId);
    }
}
