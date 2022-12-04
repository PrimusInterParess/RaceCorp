namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.EventRegister;

    public class RegisterUserRideService : IRegisterUserRideService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<ApplicationUserRide> userRideRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public RegisterUserRideService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<ApplicationUserRide> userRideRepo,
            UserManager<ApplicationUser> userManager)
        {
            this.rideRepo = rideRepo;
            this.userRideRepo = userRideRepo;
            this.userManager = userManager;
        }

        public async Task RegisterUserRide(EventRegisterModel eventModel)
        {
            var ride = this.rideRepo
                .All()
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            if (ride == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var user = await this.userManager
                .FindByIdAsync(eventModel.UserId);

            if (user == null)
            {
                throw new Exception(GlobalErrorMessages.InvalidRequest);
            }

            if (ride.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId))
            {
                throw new Exception(GlobalErrorMessages.InvalidRequest);
            }

            var userRide = new ApplicationUserRide
            {
                CreatedOn = DateTime.UtcNow,
                Ride = ride,
                ApplicationUser = user,
            };

            await this.userRideRepo.AddAsync(userRide);
            await this.rideRepo.SaveChangesAsync();
        }
    }
}
