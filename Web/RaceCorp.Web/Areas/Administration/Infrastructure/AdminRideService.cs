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
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;
    using RaceCorp.Web.Areas.Administration.Models.Race;
    using RaceCorp.Web.Areas.Administration.Models.Ride;

    public class AdminRideService : IAdminRideService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;

        public AdminRideService(IDeletableEntityRepository<Ride> rideRepo)
        {
            this.rideRepo = rideRepo;
        }

        public List<RideIndexPageModel> GetNoOwnerRides()
        {
            return this.rideRepo
                .All()
                .Include(r => r.ApplicationUser).
                Where(r => r.ApplicationUserId == null || r.ApplicationUser == null)
             .Select(r => new RideIndexPageModel
             {
                 Id = r.Id,
                 Name = r.Name,
             }).ToList();
        }

        public List<RideAllDashboardModel> GetAllRides()
        {
            var result = this.rideRepo
                .AllWithDeleted()
                .OrderByDescending(r => r.CreatedOn).Select(r => new RideAllDashboardModel
                {
                    CreatedOn = r.CreatedOn.ToString(GlobalConstants.DateStringFormat),
                    Id = r.Id,
                    IsDeleted = r.IsDeleted,
                    Name = r.Name,
                }).ToList();

            return result;
        }

        public async Task UndeleteRide(int id)
        {
            var ride = this.rideRepo
                .AllWithDeleted()
                .FirstOrDefault(r => r.Id == id);

            ride.IsDeleted = false;

            try
            {
                await this.rideRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DeleteRide(int id)
        {
            var ride = this.rideRepo
                .All()
                .FirstOrDefault(r => r.Id == id);

            this.rideRepo.Delete(ride);

            await this.rideRepo.SaveChangesAsync();
        }
    }
}
