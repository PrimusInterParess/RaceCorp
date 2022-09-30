namespace RaceCorp.Services.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Trace;
    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class RaceTraceService : IRaceTraceService
    {
        private readonly IRepository<Trace> raceTraceRepo;

        public RaceTraceService(IRepository<Trace> raceTraceRepo)
        {
            this.raceTraceRepo = raceTraceRepo;
        }

        public RaceTraceProfileModel GetRaceDifficultyProfileViewModel(int raceId, int traceId)
        {
            var trace = this.raceTraceRepo
                .AllAsNoTracking()
                .Include(t => t.Race).ThenInclude(r => r.Logo)
                .Include(t => t.Difficulty)
                .FirstOrDefault(t => t.RaceId == raceId && t.Id == traceId);

            if (trace == null)
            {
                throw new Exception(InvalidTrace);
            }
            ////LogoRootPath + race.LogoId + "." + race.Logo.Extension
            return new RaceTraceProfileModel()
            {
                Id = trace.Id,
                Name = trace.Name,
                RaceName = trace.Race.Name,
                RaceId = (int)trace.RaceId,
                Difficulty = trace.Difficulty.Level.ToString(),
                DifficultyId = trace.DifficultyId,
                ControlTime = trace.ControlTime.TotalHours,
                Length = trace.Length,
                StartTime = trace.StartTime.ToString("HH:MM"),
                TrackUrl = trace.TrackUrl,
                LogoPath = LogoRootPath + trace.Race.LogoId + "." + trace.Race.Logo.Extension,
            };
        }

        public async Task EditAsync(RaceTraceEditModel model)
        {
            var trace = this.raceTraceRepo
                .All()
                .FirstOrDefault(rd => rd.Id == model.Id);

            trace.Name = model.Name;
            trace.Length = (int)model.Length;
            trace.DifficultyId = model.DifficultyId;
            trace.ControlTime = TimeSpan.FromHours((double)model.ControlTime);
            trace.TrackUrl = model.TrackUrl;
            trace.StartTime = (DateTime)model.StartTime;

            await this.raceTraceRepo.SaveChangesAsync();
        }

        public T GetById<T>(int raceId, int traceId)
        {
            return this.raceTraceRepo
                .AllAsNoTracking()
                .Where(rt => rt.Id == traceId && rt.RaceId == raceId)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task CreateAsync(RaceTraceEditModel model)
        {
            var trace = new Trace()
            {
                Name = model.Name,
                Length = (int)model.Length,
                DifficultyId = model.DifficultyId,
                StartTime = (DateTime)model.StartTime,
                TrackUrl = model.TrackUrl,
                ControlTime = TimeSpan.FromHours((double)model.ControlTime),
                RaceId = model.RaceId,
            };

            await this.raceTraceRepo.AddAsync(trace);
            await this.raceTraceRepo.SaveChangesAsync();
        }
    }
}
