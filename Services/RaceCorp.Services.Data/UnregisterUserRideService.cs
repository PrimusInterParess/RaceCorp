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
    using RaceCorp.Web.ViewModels.EventRegister;

    public class UnregisterUserRideService : IUnregisterUserRideService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<ApplicationUserRide> userRideRepo;

        public UnregisterUserRideService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<ApplicationUserRide> userRideRepo)
        {
            this.rideRepo = rideRepo;
            this.userRideRepo = userRideRepo;
        }

        public async Task UnregisterUserRide(EventRegisterModel eventModel)
        {
            var ride = this.rideRepo
                .All()
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            if (ride == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var user = ride
                .RegisteredUsers
                .FirstOrDefault(u => u.ApplicationUserId == eventModel.UserId);

            if (user == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

            try
            {
                this.userRideRepo.Delete(user);
                await this.userRideRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
