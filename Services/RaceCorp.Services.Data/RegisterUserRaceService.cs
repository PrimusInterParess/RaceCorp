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

    public class RegisterUserRaceService : IRegisterUserRaceService
    {
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<ApplicationUserRace> userRaceRepo;
        private readonly IDeletableEntityRepository<ApplicationUserTrace> userTraceRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public RegisterUserRaceService(
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<ApplicationUserRace> userRaceRepo,
            IDeletableEntityRepository<ApplicationUserTrace> userTraceRepo,
            UserManager<ApplicationUser> userManager)
        {
            this.raceRepo = raceRepo;
            this.userRaceRepo = userRaceRepo;
            this.userTraceRepo = userTraceRepo;
            this.userManager = userManager;
        }

        public async Task RegisterUserRace(EventRegisterModel eventModel)
        {
            var race = this.raceRepo
                .All()
                .Include(r => r.Traces)
                .ThenInclude(t => t.RegisteredUsers)
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            if (race == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var user = await this.userManager
                .FindByIdAsync(eventModel.UserId);

            if (user == null)
            {
                throw new Exception(GlobalErrorMessages.InvalidRequest);
            }

            var trace = race.Traces
                .FirstOrDefault(t => t.Id == int.Parse(eventModel.TraceId));

            if (trace == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            Trace traceUserRegisterIn = null;

            var isAlredyRegistered = race
                .RegisteredUsers
                .Any(u => u.ApplicationUserId == eventModel.UserId);

            if (isAlredyRegistered)
            {
                traceUserRegisterIn = race.Traces
                 .FirstOrDefault(t => t.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId));

                var currdentTraceSpan = trace.StartTime;

                var traceAlreadyRegesteredInSpan = traceUserRegisterIn.StartTime + traceUserRegisterIn.ControlTime;

                if (currdentTraceSpan < traceAlreadyRegesteredInSpan)
                {
                    throw new InvalidOperationException(
                        string.Format(GlobalErrorMessages.AlreadyRegisteredForAnotherTrace, traceUserRegisterIn.Name, traceUserRegisterIn.StartTime.ToString(GlobalConstants.DateStringFormat)));
                }
            }

            var userTrace = new ApplicationUserTrace
            {
                CreatedOn = DateTime.UtcNow,
                Race = race,
                Trace = trace,
                ApplicationUser = user,
            };

            user.Traces.Add(userTrace);

            if (isAlredyRegistered == false)
            {
                var userRace = new ApplicationUserRace
                {
                    CreatedOn = DateTime.UtcNow,
                    Race = race,
                    Trace = trace,
                    ApplicationUser = user,
                };

                await this.userRaceRepo.AddAsync(userRace);
            }

            try
            {
                await this.userTraceRepo.AddAsync(userTrace);
                await this.raceRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception(GlobalErrorMessages.InvalidRequest);
            }
        }
    }
}
