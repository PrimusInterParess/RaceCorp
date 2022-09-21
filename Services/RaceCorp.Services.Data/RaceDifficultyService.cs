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
    }
}
