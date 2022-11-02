namespace RaceCorp.Services.Data
{
    using System;
    using System.Globalization;
    using System.IO;
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
                GpxId = trace.GpxId,
            };
        }

        public async Task EditAsync(RaceTraceEditModel model, string roothPath, string userId)
        {
            var trace = this.traceRepo
                .All()
                .FirstOrDefault(rd => rd.Id == model.Id);

            trace.Name = model.Name;
            trace.Length = (int)model.Length;
            trace.DifficultyId = model.DifficultyId;
            trace.ControlTime = TimeSpan.FromHours((double)model.ControlTime);

            var serviceAccountPath = Path.GetFullPath("\\Credentials\\testproject-366105-9ceb2767de2a.json");

            var raceName = this.raceRepo.All().FirstOrDefault(r => r.Id == model.RaceId).Name;

            if (model.GpxFile != null)
            {
                try
                {
                    var gpx = await this.gpxService
                     .ProccessingData(
                     model.GpxFile,
                     userId,
                     raceName,
                     roothPath,
                     serviceAccountPath);

                    await this.gpxRepo
                    .AddAsync(gpx);
                    trace.Gpx = gpx;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
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

        public async Task CreateRaceTraceAsync(RaceTraceEditModel model, string roothPath, string userId)
        {
            var raceName = this.raceRepo.All().FirstOrDefault(r => r.Id == model.RaceId).Name;

            var serviceAccountPath = Path.GetFullPath("\\Credentials\\testproject-366105-9ceb2767de2a.json");

            var gpx = await this.gpxService
               .ProccessingData(
               model.GpxFile,
               userId,
               raceName,
               roothPath,
               serviceAccountPath);

            var trace = await this.ProccedingData(model);
            trace.Gpx = gpx;
            trace.RaceId = model.RaceId;

            await this.traceRepo.SaveChangesAsync();
        }

        public async Task<Trace> ProccedingData(TraceInputModel traceInputModel)
        {
            var traceDto = new Trace()
            {
                Name = traceInputModel.Name,
                DifficultyId = traceInputModel.DifficultyId,
                ControlTime = TimeSpan.FromHours((double)traceInputModel.ControlTime),
                Length = (int)traceInputModel.Length,
                CreatedOn = DateTime.Now,
                StartTime = (DateTime)traceInputModel.StartTime,
            };

            await this.traceRepo.AddAsync(traceDto);

            return traceDto;
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
