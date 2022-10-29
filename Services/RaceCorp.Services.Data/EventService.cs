namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.EventRegister;

    public class EventService : IEventService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IRepository<ApplicationUserRide> userRideRepo;
        private readonly IRepository<ApplicationUserRace> userRaceRepo;

        public EventService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Race> raceRepo,
            IRepository<ApplicationUserRide> userRideRepo,
            IRepository<ApplicationUserRace> userRaceRepo)
        {
            this.rideRepo = rideRepo;
            this.raceRepo = raceRepo;
            this.userRideRepo = userRideRepo;
            this.userRaceRepo = userRaceRepo;
        }

        public async Task<bool> RegisterUserEvent(EventRegisterModel eventModel)
        {
            var result = false;

            if (eventModel.EventType == "Ride")
            {
                result = await this.RegisterUserRide(eventModel);
            }
            else
            {
                result = await this.RegisterUserRace(eventModel);
            }

            return result;
        }

        public async Task<bool> Unregister(EventRegisterModel eventModel)
        {
            var result = false;

            if (eventModel.EventType == "Ride")
            {
                result = await this.UnregisterUserRide(eventModel);
            }
            else
            {
                result = await this.UnregisterUserRace(eventModel);
            }

            return result;
        }

        private async Task<bool> RegisterUserRide(EventRegisterModel eventModel)
        {
            var ride = this.rideRepo.All().Include(r => r.RegisteredUsers).FirstOrDefault(r => r.Id == eventModel.Id);

            if (ride.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId))
            {
                return false;
            }

            var userRide = new ApplicationUserRide
            {
                ApplicationUserId = eventModel.UserId,
                RideId = eventModel.Id,
            };

            await this.userRideRepo.AddAsync(userRide);
            await this.rideRepo.SaveChangesAsync();

            return true;
        }

        private async Task<bool> RegisterUserRace(EventRegisterModel eventModel)
        {
            var race = this.raceRepo.All().Include(r => r.RegisteredUsers).FirstOrDefault(r => r.Id == eventModel.Id);

            if (race.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId))
            {
                return false;
            }

            var userRace = new ApplicationUserRace
            {
                ApplicationUserId = eventModel.UserId,
                RaceId = eventModel.Id,
            };

            await this.userRaceRepo.AddAsync(userRace);
            await this.raceRepo.SaveChangesAsync();

            return true;
        }

        private async Task<bool> UnregisterUserRide(EventRegisterModel eventModel)
        {
            var ride = this.rideRepo.All().Include(r => r.RegisteredUsers).FirstOrDefault(r => r.Id == eventModel.Id);
            var registeredUser = ride.RegisteredUsers.FirstOrDefault(u => u.ApplicationUserId == eventModel.UserId);

            try
            {
                this.userRideRepo.Delete(registeredUser);
                await this.userRideRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            return true;
        }

        private async Task<bool> UnregisterUserRace(EventRegisterModel eventModel)
        {
            var race = this.raceRepo.All().Include(r => r.RegisteredUsers).FirstOrDefault(r => r.Id == eventModel.Id);
            var registeredUser = race.RegisteredUsers.FirstOrDefault(u => u.ApplicationUserId == eventModel.UserId);

            try
            {
                this.userRaceRepo.Delete(registeredUser);
                await this.userRaceRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            return true;
        }
    }
}
