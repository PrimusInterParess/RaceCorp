namespace RaceCorp.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        public T GetById<T>(string id);
    }
}
