namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Ride;

    using static RaceCorp.Services.Constants.Messages;

    using Trace = RaceCorp.Data.Models.Trace;

    public class RideService : IRideService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Trace> traceRepo;
        private readonly IRepository<Gpx> gpxRepo;
        private readonly IGpxService gpxService;
        private readonly ITraceService traceService;

        public RideService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Town> townRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Trace> traceRepo,
            IRepository<Gpx> gpxRepo,
            IGpxService gpxService,
            ITraceService traceService)
        {
            this.rideRepo = rideRepo;
            this.townRepo = townRepo;
            this.mountainRepo = mountainRepo;
            this.traceRepo = traceRepo;
            this.gpxRepo = gpxRepo;
            this.gpxService = gpxService;
            this.traceService = traceService;
        }

        public RideAllViewModel All(
            int page,
            int itemsPerPage = 3)
        {
            var count = this.rideRepo
                .All()
                .Count();

            var rides = this.rideRepo
                .AllAsNoTracking()
                .Include(r => r.Trace)
                .Select(r => new RideInAllViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    GoogleDriveId = r.Trace.Gpx.GoogleDriveId,
                    TownName = r.Town.Name,
                    MountainName = r.Mountain.Name,
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

        public async Task CreateAsync(
            RideCreateViewModel model,
            string gxpFileRoothPath,
            string userId,
            string pathToServiceAccountKeyFile)
        {
            var townDto = this
                .townRepo
                .All()
                .FirstOrDefault(t => t.Name.ToLower() == model.Town.ToLower());
            var mountainDto = this
                .mountainRepo
                .All()
                .FirstOrDefault(m => m.Name.ToLower() == model.Mountain.ToLower());

            if (mountainDto == null)
            {
                mountainDto = new Mountain()
                {
                    Name = model.Mountain,
                };

                await this.mountainRepo
                    .AddAsync(mountainDto);
            }

            if (townDto == null)
            {
                townDto = new Town()
                {
                    Name = model.Town,
                };

                await this.townRepo
                    .AddAsync(townDto);
            }

            var gpx = await this.gpxService
                .ProccessingData(
                model.Trace.GpxFile,
                userId,
                model.Name,
                gxpFileRoothPath,
                pathToServiceAccountKeyFile);

            await this.gpxRepo
                .AddAsync(gpx);

            var trace = this.traceService
                .GetTraceDbModel(
                model.Trace,
                gpx);

            await this.traceRepo
                .AddAsync(trace);

            var ride = new Ride()
            {
                Name = model.Name,
                Date = (DateTime)model.Date,
                CreatedOn = DateTime.Now,
                Description = model.Description,
                FormatId = int.Parse(model.FormatId),
                UserId = userId,
                Town = townDto,
                Mountain = mountainDto,
                Trace = trace,
            };

            try
            {
                await this.rideRepo
                    .AddAsync(ride);

                await this.rideRepo
                    .SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task EditAsync(RideEditVIewModel model)
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

            rideDb.ModifiedOn = DateTime.UtcNow;

            if (rideDb.Mountain.Name.ToLower() != model.Mountain.ToLower())
            {
                var mountainDb = this.mountainRepo
                    .All()
                    .FirstOrDefault(m => m.Name.ToLower() == model.Name.ToLower());

                if (mountainDb == null)
                {
                    mountainDb = new Mountain()
                    {
                        Name = model.Mountain,
                    };

                    try
                    {
                        await this.mountainRepo
                            .AddAsync(mountainDb);
                    }
                    catch (Exception)
                    {
                        throw new Exception(IvalidOperationMessage);
                    }
                }

                rideDb.Mountain = mountainDb;
            }

            if (rideDb.Town.Name.ToLower() != model.Town.ToLower())
            {
                var townDb = this.townRepo
                    .All()
                    .FirstOrDefault(m => m.Name.ToLower() == model.Name.ToLower());

                if (townDb == null)
                {
                    townDb = new Town()
                    {
                        Name = model.Town,
                    };

                    await this.townRepo
                        .AddAsync(townDb);
                }

                rideDb.Town = townDb;
            }

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

            // add gpx file data
            traceDb.StartTime = (DateTime)model.Trace.StartTime;
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
    }
}
