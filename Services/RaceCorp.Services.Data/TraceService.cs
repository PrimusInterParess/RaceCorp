namespace RaceCorp.Services.Data
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
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

        public async Task EditAsync(RaceTraceEditModel model, string roothPath, string userId)
        {
            var trace = this.traceRepo
                .All()
                .FirstOrDefault(rd => rd.Id == model.Id);

            trace.Name = model.Name;
            trace.Length = (int)model.Length;
            trace.DifficultyId = model.DifficultyId;
            trace.ControlTime = TimeSpan.FromHours((double)model.ControlTime);

            var serviceAccountPath = Path.GetFullPath(GlobalConstants.ServiceAccountPath);

            var raceName = this.raceRepo
                .All()
                .FirstOrDefault(r => r.Id == model.RaceId).Name;

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


            var raceName = this.raceRepo
                .All()
                .FirstOrDefault(r => r.Id == model.RaceId).Name;

            if (raceName == null)
            {
                throw new NullReferenceException(GlobalErrorMessages.InvalidInputData);
            }

            var serviceAccountPath = Path.GetFullPath(GlobalConstants.ServiceAccountPath);

            try
            {
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
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

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
            var trace = this.traceRepo
                .All()
                .FirstOrDefault(t => t.Id == id);

            if (trace == null)
            {
                throw new NullReferenceException(GlobalErrorMessages.InvalidInputData);
            }

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
