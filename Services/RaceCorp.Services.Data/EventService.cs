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
        private readonly IDeletableEntityRepository<Trace> traceRepo;
        private readonly IRepository<ApplicationUserRide> userRideRepo;
        private readonly IRepository<ApplicationUserRace> userRaceRepo;
        private readonly IRepository<ApplicationUserTrace> userTraceRepo;

        public EventService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Trace> traceRepo,
            IRepository<ApplicationUserRide> userRideRepo,
            IRepository<ApplicationUserRace> userRaceRepo,
            IRepository<ApplicationUserTrace> userTraceRepo)
        {
            this.rideRepo = rideRepo;
            this.raceRepo = raceRepo;
            this.traceRepo = traceRepo;
            this.userRideRepo = userRideRepo;
            this.userRaceRepo = userRaceRepo;
            this.userTraceRepo = userTraceRepo;
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
            var race = this.raceRepo
                .All()
                .Include(r => r.Traces)
                .ThenInclude(t => t.RegisteredUsers)
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            var trace = race.Traces.FirstOrDefault(t => t.Id == int.Parse(eventModel.TraceId));
            Trace traceUserRegisterIn = null;

            var isAlredyRegistered = race.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId);

            if (isAlredyRegistered)
            {
                traceUserRegisterIn = race.Traces
                 .FirstOrDefault(t => t.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId));

                var currdentTraceSpan = trace.StartTime;

                var traceAlreadyRegesteredInSpan = traceUserRegisterIn.StartTime + traceUserRegisterIn.ControlTime;

                if (currdentTraceSpan < traceAlreadyRegesteredInSpan)
                {
                    throw new InvalidOperationException($"Cannot Register for this trace.You are alredy registered for {traceUserRegisterIn.Name} and IT starts {traceUserRegisterIn.StartTime}");
                }
            }

            var userTrace = new ApplicationUserTrace
            {
                ApplicationUserId = eventModel.UserId,
                TraceId = trace.Id,
                RaceId = race.Id,
            };

            if (isAlredyRegistered == false)
            {
                var userRace = new ApplicationUserRace
                {
                    ApplicationUserId = eventModel.UserId,
                    RaceId = eventModel.Id,
                    TraceId = trace.Id,
                };

                await this.userRaceRepo.AddAsync(userRace);
            }

            try
            {
                await this.userTraceRepo.AddAsync(userTrace);
                await this.raceRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error registering User for race");
            }

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
            var race = this.raceRepo
                .All()
                .Include(t => t.Traces)
                .ThenInclude(t => t.RegisteredUsers)
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            var trace = race.Traces
                .FirstOrDefault(t => t.Id == int.Parse(eventModel.TraceId));

            var participatesInAnotherTrace = race.Traces
                .FirstOrDefault(t => t.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId && u.TraceId != trace.Id));

            if (participatesInAnotherTrace == null)
            {
                var registeredUserRace = race.RegisteredUsers.FirstOrDefault(u => u.ApplicationUserId == eventModel.UserId);
                this.userRaceRepo.Delete(registeredUserRace);
            }

            var registeredUserTrace = trace.RegisteredUsers.FirstOrDefault(u => u.ApplicationUserId == eventModel.UserId);

            try
            {
                this.userTraceRepo.Delete(registeredUserTrace);
                await this.userTraceRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            return true;
        }
    }
}
