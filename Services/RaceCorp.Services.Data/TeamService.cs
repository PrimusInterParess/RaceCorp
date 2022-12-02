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

    public class TeamService : ITeamService
    {
        private readonly IDeletableEntityRepository<Team> teamRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IFileService fileService;

        public TeamService(
            IDeletableEntityRepository<Team> teamRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Town> townRepo,
            IFileService fileService)
        {
            this.teamRepo = teamRepo;
            this.userRepo = userRepo;
            this.townRepo = townRepo;
            this.fileService = fileService;
        }

        public List<T> All<T>()
        {
            return this.teamRepo
                .All()
                .To<T>()
                .ToList();
        }

        public T ById<T>(string id)
        {
            return this.teamRepo
                .AllAsNoTracking()
                .Where(t => t.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task CreateAsync(TeamCreateBaseModel inputMode, string roothPath)
        {
            var alredyExists = this.teamRepo
                .All()
                .Any(t => t.Name == inputMode.Name);

            if (alredyExists)
            {
                throw new InvalidOperationException(GlobalErrorMessages.TeamAlreadyExists);
            }

            var user = this.userRepo
                .All()
                .Include(u => u.Team)
                .FirstOrDefault(u => u.Id == inputMode.CreatorId);

            if (user.Team != null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.AlreadyHaveCreatedTeam);
            }

            var town = this.townRepo
                .All()
                .FirstOrDefault(t => t.Name.ToLower() == inputMode.TownName.ToLower());

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
                var logoImage = await this.fileService
                    .ProccessingImageData(inputMode.Logo, user.Id, roothPath, inputMode.Name);

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

        public async Task EditAsync(TeamEditViewModel inputModel, string roothPath)
        {
            var teamDb = this.teamRepo
                .All()
                .Include(t => t.Images)
                .Include(t => t.Town)
                .FirstOrDefault(t => t.Id == inputModel.Id);

            if (teamDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidTeam);
            }

            if (teamDb.Name != inputModel.Name)
            {
                var nameAlreadyExists = this.teamRepo
                    .All()
                    .Any(t => t.Name == inputModel.Name);

                if (nameAlreadyExists)
                {
                    throw new InvalidOperationException(GlobalErrorMessages.TeamAlreadyExists);
                }

                teamDb.Name = inputModel.Name;
            }

            if (teamDb.Town.Name != inputModel.TownName)
            {
                var town = this.townRepo
                    .All()
                    .FirstOrDefault(t => t.Name.ToLower() == inputModel.TownName.ToLower());

                if (town == null)
                {
                    town = new Town
                    {
                        Name = inputModel.TownName,
                        CreatedOn = DateTime.Now,
                    };

                    await this.townRepo.AddAsync(town);
                }

                teamDb.Town = town;
            }

            teamDb.Description = inputModel.Description;

            if (inputModel.Logo != null)
            {
                try
                {
                    var logoImage = await this.fileService
                        .ProccessingImageData(inputModel.Logo, inputModel.ApplicationUserId, roothPath, inputModel.Name);

                    teamDb.LogoImagePath = $"\\{logoImage.ParentFolderName}\\{logoImage.ChildFolderName}\\{logoImage.Id}.{logoImage.Extension}";
                    teamDb.Images.Add(logoImage);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(e.Message);
                }
            }

            teamDb.ModifiedOn = DateTime.Now;

            try
            {
                await this.teamRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
        }

        public TeamProfileViewModel GetProfileById(string id, string currentUserId)
        {
            var teamDto = this.teamRepo
                .AllAsNoTracking()
                .Where(t => t.Id == id)
                .To<TeamProfileViewModel>()
                .FirstOrDefault();

            if (teamDto == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidTeam);
            }

            teamDto.CurrentUserIsOwner = teamDto.ApplicationUserId == currentUserId;
            teamDto.IsMember = teamDto.TeamMembers.Any(m => m.Id == currentUserId);
            teamDto.RequestedJoin = teamDto.JoinRequests.Any(r => r.RequesterId == currentUserId);

            return teamDto;
        }

        public List<T> GetTeamMembers<T>(string teamId)
        {
            return this.teamRepo
                .AllAsNoTracking()
                .Where(t => t.Id == teamId)
                .Include(t => t.TeamMembers)
                .To<T>()
                .ToList();
        }

        public async Task RemoveUserAsync(string teamId, string memberId)
        {
            var teamDb = this.teamRepo
                .All()
                .Include(t => t.TeamMembers)
                .Include(t => t.ApplicationUser)
                .ThenInclude(to => to.Requests)
                .FirstOrDefault(t => t.Id == teamId);

            if (teamDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var memberDb = this.userRepo
                .All()
                .Include(m => m.Team)
                .Include(m => m.MemberInTeam)
                .FirstOrDefault(m => m.Id == memberId);

            if (memberDb == null ||
                memberDb.Team != null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var teamOwner = teamDb.ApplicationUser;

            var requestToRemove = teamOwner
               .Requests
               .FirstOrDefault(r => r.IsApproved && r.RequesterId == memberDb.Id && r.Type == GlobalConstants.RequestTypeTeamJoin);

            if (requestToRemove != null)
            {
                teamOwner.Requests.Remove(requestToRemove);
            }

            teamDb.TeamMembers.Remove(memberDb);
            memberDb.MemberInTeam = null;

            teamDb.ModifiedOn = DateTime.Now;

            await this.teamRepo.SaveChangesAsync();
        }
    }
}
