namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;

    public class ConnectUserService : IConnectUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Request> requestRepo;

        public ConnectUserService(
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Request> requestRepo)
        {
            this.userRepo = userRepo;
            this.requestRepo = requestRepo;
        }

        public async Task RequestConnectUserAsync(string requesterId, string targetUserId)
        {
            var requester = this.userRepo
               .All()
               .Include(u => u.Requests)
               .Include(u => u.Connections)
               .FirstOrDefault(u => u.Id == requesterId);

            var targetUserDb = this.userRepo
                .All()
                .Include(u => u.Requests)
                .Include(u => u.Connections)
                .FirstOrDefault(u => u.Id == targetUserId);

            if (requester == null ||
                targetUserDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            if (targetUserDb.Connections.Any(c => c.Id == requester.Id) ||
                targetUserDb.Requests.Any(r => r.Type == GlobalConstants.RequestTypeConnectUser && r.RequesterId == requester.Id))
            {
                throw new InvalidOperationException(GlobalErrorMessages.AlreadyRequestedConnection);
            }

            var request = new Request()
            {
                Type = GlobalConstants.RequestTypeConnectUser,
                TargetUser = targetUserDb,
                Requester = requester,
                Description = $"{requester.FirstName} {requester.LastName} want to connect with you",
                CreatedOn = DateTime.UtcNow,
            };

            targetUserDb.Requests.Add(request);

            try
            {
                await this.requestRepo.AddAsync(request);
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }
        }
    }
}
