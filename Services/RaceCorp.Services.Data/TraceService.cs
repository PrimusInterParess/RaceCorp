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

    public class TraceService : ITraceService
    {
        private readonly IRepository<Trace> traceRepo;

        public TraceService(IRepository<Trace> raceTraceRepo)
        {
            this.traceRepo = raceTraceRepo;
        }

        public RaceTraceProfileModel GetRaceDifficultyProfileViewModel(int raceId, int traceId)
        {
            var trace = this.traceRepo
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

                // Gpx file path/GoogleId
                LogoPath = LogoRootPath + trace.Race.LogoId + "." + trace.Race.Logo.Extension,
            };
        }

        public async Task EditAsync(RaceTraceEditModel model)
        {
            var trace = this.traceRepo
                .All()
                .FirstOrDefault(rd => rd.Id == model.Id);

            trace.Name = model.Name;
            trace.Length = (int)model.Length;
            trace.DifficultyId = model.DifficultyId;
            trace.ControlTime = TimeSpan.FromHours((double)model.ControlTime);

            // gpx file edit? get file location
            trace.StartTime = (DateTime)model.StartTime;

            await this.traceRepo.SaveChangesAsync();
        }

        public T GetById<T>(int raceId, int traceId)
        {
            return this.traceRepo
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

                // TODO: add logic for gpx file
                ControlTime = TimeSpan.FromHours((double)model.ControlTime),
                RaceId = model.RaceId,
            };

            await this.traceRepo.AddAsync(trace);
            await this.traceRepo.SaveChangesAsync();
        }

        public Trace GetTraceDbModel(TraceInputModel traceInputModel, Gpx gpx)
        {
            return new Trace()
            {
                Name = traceInputModel.Name,
                DifficultyId = traceInputModel.DifficultyId,
                ControlTime = TimeSpan.FromHours((double)traceInputModel.ControlTime),
                Length = (int)traceInputModel.Length,
                CreatedOn = DateTime.Now,
                StartTime = (DateTime)traceInputModel.StartTime,
                Gpx = gpx,
            };
        }
    }
}
