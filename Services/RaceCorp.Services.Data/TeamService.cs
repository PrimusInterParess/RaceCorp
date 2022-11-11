namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Team;
    using static System.Net.Mime.MediaTypeNames;

    public class TeamService : ITeamService
    {
        private readonly IDeletableEntityRepository<Team> teamRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IFileService fileService;
        private readonly IDeletableEntityRepository<Request> requestRepo;

        public TeamService(
            IDeletableEntityRepository<Team> teamRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Town> townRepo,
            IFileService fileService,
            IDeletableEntityRepository<Request> requestRepo)
        {
            this.teamRepo = teamRepo;
            this.userRepo = userRepo;
            this.townRepo = townRepo;
            this.fileService = fileService;
            this.requestRepo = requestRepo;
        }

        public List<T> All<T>()
        {
            return this.teamRepo.All().To<T>().ToList();
        }

        public T ById<T>(string id)
        {
            return this.teamRepo.AllAsNoTracking().To<T>().FirstOrDefault();
        }

        public async Task CreateAsync(TeamCreateBaseModel inputMode, string roothPath)
        {
            var alredyExists = this.teamRepo.All().Any(t => t.Name == inputMode.Name);

            if (alredyExists)
            {
                throw new InvalidOperationException(GlobalErrorMessages.TeamAlreadyExists);
            }

            var user = this.userRepo.All().Include(u => u.Team).FirstOrDefault(u => u.Id == inputMode.CreatorId);

            if (user.Team != null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.AlreadyHaveCreatedTeam);
            }

            var town = this.townRepo.All().FirstOrDefault(t => t.Name.ToLower() == inputMode.TownName.ToLower());

            if (town == null)
            {
                town = new Town
                {
                    Name = inputMode.TownName,
                    CreatedOn = DateTime.Now,
                };
            }

            var team = new Team
            {
                Name = inputMode.Name,
                ApplicationUser = user,
                CreatedOn = DateTime.UtcNow,
                Town = town,
                Description = inputMode.Description,
            };

            user.MemberInTeam = team;

            try
            {
                var logoImage = await this.fileService.ProccessingImageData(inputMode.Logo, user.Id, roothPath, inputMode.Name);
                team.LogoImagePath = $"\\{logoImage.ParentFolderName}\\{logoImage.ChildFolderName}\\{logoImage.Id}.{logoImage.Extension}";
                team.Images.Add(logoImage);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            try
            {
                await this.teamRepo.AddAsync(team);
                await this.teamRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task<bool> RequestJoin(string teamId, string userId)
        {
            var teamDb = this.teamRepo
                .All()
                .Include(t => t.ApplicationUser)
                .ThenInclude(u => u.Requests)
                .FirstOrDefault(t => t.Id == teamId);

            var teamOwner = teamDb.ApplicationUser;

            if (teamDb == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

            if (teamOwner.Id == userId)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

            if (teamOwner.Requests.Any(r => r.RequesterId == userId))
            {
                throw new InvalidOperationException(GlobalErrorMessages.AlreadyRequested);
            }

            var requester = this.userRepo
                .All()
                .Include(u => u.Team)
                .Include(u => u.MemberInTeam)
                .FirstOrDefault(u => u.Id == userId);

            if (requester == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

            var request = new Request()
            {
                ApplicationUser = teamOwner,
                RequesterId = userId,
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

                return false;
            }

            return true;
        }
    }
}
