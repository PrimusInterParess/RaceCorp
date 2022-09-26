using System.Globalization;
using System.Threading.Tasks;
using RaceCorp.Services.Mapping;

namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class RaceDifficultyService : IRaceDifficultyService
    {
        private readonly IRepository<RaceDifficulty> raceDiffRepo;

        public RaceDifficultyService(IRepository<RaceDifficulty> raceDiffRepo)
        {
            this.raceDiffRepo = raceDiffRepo;
        }

        public RaceDifficultyProfileViewModel GetRaceDifficultyProfileViewModel(int raceId, int traceId)
        {
            var trace = this.raceDiffRepo
                .AllAsNoTracking()
                .Include(t => t.Race).ThenInclude(r => r.Logo)
                .Include(t => t.Difficulty)
                .FirstOrDefault(t => t.RaceId == raceId && t.Id == traceId);

            if (trace == null)
            {
                throw new Exception(InvalidTrace);
            }
            ////LogoRootPath + race.LogoId + "." + race.Logo.Extension
            return new RaceDifficultyProfileViewModel()
            {
                Id = trace.Id,
                Name = trace.Name,
                RaceName = trace.Race.Name,
                RaceId = trace.RaceId,
                Difficulty = trace.Difficulty.Level.ToString(),
                DifficultyId = trace.DifficultyId,
                ControlTime = trace.ControlTime.TotalHours,
                Length = trace.Length,
                StartTime = trace.StartTime,
                TrackUrl = trace.TrackUrl,
                LogoPath = LogoRootPath + trace.Race.LogoId + "." + trace.Race.Logo.Extension,
            };
        }

        public async Task EditAsync(RaceDifficultyInputViewModel model)
        {
            var trace = this.raceDiffRepo
                .All()
                .FirstOrDefault(rd => rd.Id == model.Id);

            trace.Name = model.Name;
            trace.Length = (int)model.Length;
            trace.DifficultyId = model.DifficultyId;
            trace.ControlTime = TimeSpan.FromHours((double)model.ControlTime);
            trace.TrackUrl = model.TrackUrl;
            trace.StartTime = (DateTime)model.StartTime;

            await this.raceDiffRepo.SaveChangesAsync();
        }

        public T GetById<T>(int raceId, int traceId)
        {
            return this.raceDiffRepo
                .AllAsNoTracking()
                .Where(rd => rd.Id == traceId && rd.RaceId == raceId)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task CreateAsync(RaceDifficultyInputViewModel model)
        {
            var trace = new RaceDifficulty()
            {
                RaceId = model.RaceId,
                Name = model.Name,
                Length = (int)model.Length,
                DifficultyId = model.DifficultyId,
                StartTime = (DateTime)model.StartTime,
                TrackUrl = model.TrackUrl,
                ControlTime = TimeSpan.FromHours((double)model.ControlTime),
            };

            await this.raceDiffRepo.AddAsync(trace);
            await this.raceDiffRepo.SaveChangesAsync();
        }
    }
}
