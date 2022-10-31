namespace RaceCorp.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;

        public UserService(IDeletableEntityRepository<ApplicationUser> userRepo)
        {
            this.userRepo = userRepo;
        }

        public T GetById<T>(string id)
        {
            return this.userRepo.AllAsNoTracking().Where(u => u.Id == id).To<T>().FirstOrDefault();
        }
    }
}