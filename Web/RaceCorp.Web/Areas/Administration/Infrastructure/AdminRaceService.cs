namespace RaceCorp.Web.Areas.Administration.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;
    using RaceCorp.Web.Areas.Administration.Models.Race;

    public class AdminRaceService : IAdminRaceService
    {
        private readonly IDeletableEntityRepository<Race> raceRepo;

        public AdminRaceService(
            IDeletableEntityRepository<Race> raceRepo)
        {
            this.raceRepo = raceRepo;
        }

        public async Task DeleteRace(int id)
        {
            var race = this.raceRepo
                .All()
                .FirstOrDefault(r => r.Id == id);

            this.raceRepo.Delete(race);

            await this.raceRepo.SaveChangesAsync();
        }

        public List<RaceAllDashboardModel> GetAllRaces()
        {
            return this.raceRepo
                .AllWithDeleted()
                .OrderByDescending(r => r.CreatedOn).Select(r => new RaceAllDashboardModel
                {
                    CreatedOn = r.CreatedOn.ToString(GlobalConstants.DateStringFormat),
                    Id = r.Id,
                    IsDeleted = r.IsDeleted,
                    Name = r.Name,
                }).ToList();
        }

        public List<RaceIndexPageModel> GetNoOwnerRaces()
        {

            return this.raceRepo.All()
                .Include(r => r.ApplicationUser)
                .Where(r => r.ApplicationUserId == null || r.ApplicationUser == null)
                .Select(r => new RaceIndexPageModel
                {
                    Id = r.Id,
                    Name = r.Name,
                }).ToList();
        }

        public async Task UndeleteRace(int id)
        {
            var race = this.raceRepo.AllWithDeleted().FirstOrDefault(r => r.Id == id);

            race.IsDeleted = false;

            try
            {
                await this.raceRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
