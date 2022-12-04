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

    public class UnregisterUserRaceService : IUnregisterUserRaceService
    {
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<ApplicationUserRace> userRaceRepo;
        private readonly IDeletableEntityRepository<ApplicationUserTrace> userTraceRepo;

        public UnregisterUserRaceService(
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<ApplicationUserRace> userRaceRepo,
            IDeletableEntityRepository<ApplicationUserTrace> userTraceRepo)
        {
            this.raceRepo = raceRepo;
            this.userRaceRepo = userRaceRepo;
            this.userTraceRepo = userTraceRepo;
        }

        public async Task UnregisterUserRace(EventRegisterModel eventModel)
        {
            var race = this.raceRepo
                .All()
                .Include(t => t.Traces)
                .ThenInclude(t => t.RegisteredUsers)
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            if (race == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var trace = race.Traces
                .FirstOrDefault(t => t.Id == int.Parse(eventModel.TraceId));

            if (trace == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var participatesInAnotherTrace = race.Traces
                .FirstOrDefault(t =>
                t.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId && u.TraceId != trace.Id));

            if (participatesInAnotherTrace == null)
            {
                var registeredUserRace = race.RegisteredUsers.FirstOrDefault(u =>
                u.ApplicationUserId == eventModel.UserId);

                this.userRaceRepo.Delete(registeredUserRace);
            }

            var registeredUserTrace = trace
                .RegisteredUsers
                .FirstOrDefault(u =>
            u.ApplicationUserId == eventModel.UserId);

            try
            {
                this.userTraceRepo.Delete(registeredUserTrace);
                await this.userTraceRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
