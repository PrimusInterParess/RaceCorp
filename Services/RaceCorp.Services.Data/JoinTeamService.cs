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

    public class JoinTeamService : IJoinTeamService
    {
        private readonly IDeletableEntityRepository<Team> teamRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Request> requestRepo;

        public JoinTeamService(
            IDeletableEntityRepository<Team> teamRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Request> requestRepo)
        {
            this.teamRepo = teamRepo;
            this.userRepo = userRepo;
            this.requestRepo = requestRepo;
        }

        public async Task RequestJoinTeamAsync(string teamId, string requesterId)
        {
            var teamDb = this.teamRepo
                 .All()
                 .Include(t => t.ApplicationUser)
                 .ThenInclude(u => u.Requests)
                 .FirstOrDefault(t => t.Id == teamId);

            if (teamDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidTeam);
            }

            var teamOwner = teamDb.ApplicationUser;

            if (teamOwner.Id == requesterId)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

            if (teamOwner.Requests.Any(r => r.RequesterId == requesterId && r.Type == GlobalConstants.RequestTypeTeamJoin))
            {
                throw new InvalidOperationException(GlobalErrorMessages.AlreadyRequested);
            }

            var requester = this.userRepo
                .All()
                .Include(u => u.Team)
                .Include(u => u.MemberInTeam)
                .FirstOrDefault(u => u.Id == requesterId);

            if (requester == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

            if (requester.Team != null ||
                requester.MemberInTeam != null)
            {
                var teamName = requester.Team == null ? requester.MemberInTeam.Name : requester.Team.Name;

                throw new InvalidOperationException(string.Format(GlobalErrorMessages.AlreadyHaveTeam, teamName));
            }

            var request = new Request()
            {
                Type = GlobalConstants.RequestTypeTeamJoin,
                TargetUser = teamOwner,
                RequesterId = requesterId,
                Description = $"{requester.FirstName} {requester.LastName} want to join {teamDb.Name}",
                CreatedOn = DateTime.UtcNow,
            };

            teamOwner.Requests.Add(request);

            try
            {
                await this.requestRepo.AddAsync(request);
                await this.requestRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
        }
    }
}
