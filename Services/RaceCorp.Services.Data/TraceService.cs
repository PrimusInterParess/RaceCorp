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
        private readonly IRepository<Gpx> gpxRepo;
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IGpxService gpxService;

        public TraceService(
            IRepository<Trace> raceTraceRepo,
            IRepository<Gpx> gpxRepo,
            IDeletableEntityRepository<Race> raceRepo,
            IGpxService gpxService)
        {
            this.traceRepo = raceTraceRepo;
            this.gpxRepo = gpxRepo;
            this.raceRepo = raceRepo;
            this.gpxService = gpxService;
        }

        public RaceTraceProfileModel GetRaceTraceProfileViewModel(int raceId, int traceId)
        {
            var trace = this.traceRepo
                .AllAsNoTracking()
                .Include(t => t.Race).ThenInclude(r => r.Logo)
                .Include(t => t.Difficulty)
                .Include(t => t.Gpx)
                .FirstOrDefault(t => t.RaceId == raceId && t.Id == traceId);

            if (trace == null)
            {
                throw new Exception(InvalidTrace);
            }

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
                LogoPath = LogoRootPath + trace.Race.LogoId + "." + trace.Race.Logo.Extension,
                GoogleDriveId = trace.Gpx.GoogleDriveId,
            };
        }

        public async Task EditAsync(RaceTraceEditModel model, string gxpFileRoothPath, string userId, string pathToServiceAccountKeyFile)
        {
            var trace = this.traceRepo
                .All()
                .FirstOrDefault(rd => rd.Id == model.Id);

            trace.Name = model.Name;
            trace.Length = (int)model.Length;
            trace.DifficultyId = model.DifficultyId;
            trace.ControlTime = TimeSpan.FromHours((double)model.ControlTime);

            var raceName = this.raceRepo.All().FirstOrDefault(r => r.Id == model.RaceId).Name;

            if (model.GpxFile != null)
            {
                var gpx = await this.gpxService
                .ProccessingData(
                model.GpxFile,
                userId,
                raceName,
                gxpFileRoothPath,
                pathToServiceAccountKeyFile);

                await this.gpxRepo
                    .AddAsync(gpx);
                trace.Gpx = gpx;
            }

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

        public async Task CreateRaceTraceAsync(RaceTraceEditModel model, string gxpFileRoothPath, string userId, string pathToServiceAccountKeyFile)
        {

            var raceName = this.raceRepo.All().FirstOrDefault(r => r.Id == model.RaceId).Name;

            var gpx = await this.gpxService
               .ProccessingData(
               model.GpxFile,
               userId,
               raceName,
               gxpFileRoothPath,
               pathToServiceAccountKeyFile);

            await this.gpxRepo
                    .AddAsync(gpx);

            var trace = new Trace()
            {
                Name = model.Name,
                Length = (int)model.Length,
                DifficultyId = model.DifficultyId,
                StartTime = (DateTime)model.StartTime,
                ControlTime = TimeSpan.FromHours((double)model.ControlTime),
                RaceId = model.RaceId,
                Gpx = gpx,
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

        public async Task<bool> DeleteTraceAsync(int id)
        {
            var trace = this.traceRepo.All().FirstOrDefault(t => t.Id == id);

            this.traceRepo.Delete(trace);

            var result = await this.traceRepo.SaveChangesAsync();

            if (result == 0)
            {
                return false;
            }

            return true;
        }
    }
}
