namespace RaceCorp.Services.Data
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Ride;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    using Trace = RaceCorp.Data.Models.Trace;

    public class RideService : IRideService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Trace> traceRepo;
        private readonly IRepository<ApplicationUserRide> userRideRepo;
        private readonly IRepository<Gpx> gpxRepo;
        private readonly IGpxService gpxService;
        private readonly ITraceService traceService;
        private readonly ITownService townService;
        private readonly IMountanService mountanService;

        public RideService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Trace> traceRepo,
            IRepository<ApplicationUserRide> userRideRepo,
            IRepository<Gpx> gpxRepo,
            IGpxService gpxService,
            ITraceService traceService,
            ITownService townService,
            IMountanService mountanService)
        {
            this.rideRepo = rideRepo;
            this.traceRepo = traceRepo;
            this.userRideRepo = userRideRepo;
            this.gpxRepo = gpxRepo;
            this.gpxService = gpxService;
            this.traceService = traceService;
            this.townService = townService;
            this.mountanService = mountanService;
        }

        public RideAllViewModel All(
            int page,
            int itemsPerPage = GlobalIntValues.ItemsPerPage)
        {
            var count = this.rideRepo
                .All()
                .Count();

            var rides = this.rideRepo
                .AllAsNoTracking()
                .OrderByDescending(r => r.CreatedOn)
                .Include(r => r.Trace)
                .Select(r => new RideInAllViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    TraceMapUrl = r.Trace.MapUrl,
                    TownName = r.Town.Name,
                    MountainName = r.Mountain.Name,
                    TraceStartTime = r.Trace.StartTime.ToString(GlobalConstants.DateMessageFormat),
                })
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            return new RideAllViewModel()
            {
                PageNumber = page,
                ItemsPerPage = itemsPerPage,
                RacesCount = count,
                Rides = rides,
            };
        }

        public async Task CreateAsync(RideCreateViewModel model, string userId)
        {
            var mountainDb = await this.mountanService
                .ProccesingData(model.Mountain);

            var townDb = await this.townService
                .ProccesingData(model.Town);

            var gpx = await this.gpxService
                .ProccessingData(
                model.Trace.GpxFile,
                userId,
                model.Name);

            var trace = await this.traceService
                .ProccedingData(model.Trace, gpx.GoogleDriveId);

            trace.Gpx = gpx;

            var ride = new Ride()
            {
                Name = model.Name,
                Date = (DateTime)model.Date,
                CreatedOn = DateTime.Now,
                Description = model.Description,
                FormatId = int.Parse(model.FormatId),
                ApplicationUserId = userId,
                Town = townDb,
                Mountain = mountainDb,
                Trace = trace,
            };

            try
            {
                await this.rideRepo.AddAsync(ride);
                await this.rideRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task EditAsync(RideEditVIewModel model, string userId)
        {
            var rideDb = this.rideRepo
                .All()
                .Include(r => r.Mountain)
                .Include(r => r.Town)
                .FirstOrDefault(r => r.Id == model.Id);

            if (rideDb == null)
            {
                throw new Exception(IvalidOperationMessage);
            }

            var mountainDb = await this.mountanService
                .ProccesingData(model.Mountain);

            var townDb = await this.townService
                .ProccesingData(model.Town);

            rideDb.Mountain = mountainDb;
            rideDb.Town = townDb;
            rideDb.ModifiedOn = DateTime.UtcNow;
            rideDb.Description = model.Description;
            rideDb.FormatId = int.Parse(model.FormatId);
            rideDb.Date = model.Date;
            rideDb.Name = model.Name;

            var traceDb = this.traceRepo
                .All()
                .FirstOrDefault(t => t.Id == model.TraceId);

            traceDb.Name = model.Trace.Name;
            traceDb.Length = (int)model.Trace.Length;
            traceDb.DifficultyId = model.Trace.DifficultyId;
            traceDb.ControlTime = TimeSpan.FromHours((double)model.Trace.ControlTime);
            traceDb.StartTime = (DateTime)model.Trace.StartTime;

            if (model.Trace.GpxFile != null)
            {
                var gpx = await this.gpxService
                .ProccessingData(
                model.Trace.GpxFile,
                userId,
                model.Name);

                await this.gpxRepo
                    .AddAsync(gpx);

                traceDb.Gpx = gpx;
                traceDb.MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, gpx.GoogleDriveId);
            }

            try
            {
                await this.rideRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception(IvalidOperationMessage);
            }
        }

        public T GetById<T>(int id)
        {
            return this.rideRepo
               .AllAsNoTracking()
               .Where(r => r.Id == id)
               .To<T>()
               .FirstOrDefault();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ride = this.rideRepo
                .All()
                .FirstOrDefault(r => r.Id == id);

            this.rideRepo.Delete(ride);

            var result = await this.rideRepo.SaveChangesAsync();

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public RideAllViewModel GetUpcomingRides(int page, int itemsPerPage = GlobalIntValues.ItemsPerPage)
        {
            var count = this.rideRepo
                 .All()
                 .Where(r => r.Date > DateTime.Now)
                 .Count();

            var rides = this.rideRepo
                .AllAsNoTracking()
                .OrderBy(r => r.Date)
                .Where(r => r.Date > DateTime.Now)
                .Include(r => r.Trace)
                .Select(r => new RideInAllViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    TraceMapUrl = r.Trace.MapUrl,
                    TownName = r.Town.Name,
                    MountainName = r.Mountain.Name,
                    TraceStartTime = r.Trace.StartTime.ToString(GlobalConstants.DateStringFormat),
                })
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            return new RideAllViewModel()
            {
                PageNumber = page,
                ItemsPerPage = itemsPerPage,
                RacesCount = count,
                Rides = rides,
            };
        }

        public void UpdateInfo(RideProfileVIewModel rideModel, ApplicationUser user)
        {
            if (user != null)
            {
                rideModel.IsRegistered = rideModel.RegisteredUsers.Any(u => u.ApplicationUserId == user.Id);
                rideModel.IsOwner = rideModel.ApplicationUserId == user.Id;
            }
            else
            {
                rideModel.IsRegistered = false;
            }

            rideModel.HasPassed = DateTime.Parse(rideModel.StartTime) < DateTime.Now;
        }
    }
}
