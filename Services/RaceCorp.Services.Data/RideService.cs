namespace RaceCorp.Services.Data
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.RaceViewModels;
    using RaceCorp.Web.ViewModels.Ride;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Drive;
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
        private readonly IGoogleDriveService googleDriveService;

        public RideService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Town> townRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Trace> traceRepo,
            IRepository<Gpx> gpxRepo,
            IGpxService gpxService,
            IGoogleDriveService googleDriveService)
        {
            this.rideRepo = rideRepo;
            this.townRepo = townRepo;
            this.mountainRepo = mountainRepo;
            this.traceRepo = traceRepo;
            this.gpxRepo = gpxRepo;
            this.gpxService = gpxService;
            this.googleDriveService = googleDriveService;
        }

        public RideAllViewModel All(int page, int itemsPerPage = 3)
        {
            var count = this
                .rideRepo
                .All()
                .Count();
            var rides = this
                .rideRepo
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

        public async Task CreateAsync(RideCreateViewModel model, string gxpFileRoothPath, string userId, string pathToServiceAccountKeyFile)
        {
            // validate entries!!!
            var townData = this
                .townRepo
                .AllAsNoTracking()
                .FirstOrDefault(t => t.Name == model.Town);
            var mountainData = this
                .mountainRepo
                .AllAsNoTracking()
                .FirstOrDefault(m => m.Name == model.Mountain);

            if (mountainData == null)
            {
                mountainData = new Mountain()
                {
                    Name = model.Mountain,
                };

                await this.mountainRepo.AddAsync(mountainData);
            }

            if (townData == null)
            {
                townData = new Town()
                {
                    Name = model.Town,
                };

                await this.townRepo.AddAsync(townData);
            }

            var extension = string.Empty;

            try
            {
                extension = Path.GetExtension(model.Trace.GpxFile.FileName).TrimStart('.');
            }
            catch (Exception)
            {
                throw new Exception(GpxFileRequired);
            }

            var validateFileExtention = this.gpxService.ValidateExtension(extension);

            if (validateFileExtention == false)
            {
                throw new Exception(InvalidFileExtension + extension);
            }

            var gpx = new Gpx()
            {
                Extension = extension,
                UserId = userId,
            };

            try
            {
                await this.gpxService.SaveIntoFileSystem(model.Trace.GpxFile, gxpFileRoothPath, model.Name, gpx.Id, extension);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var gxpFilePath = $"{gxpFileRoothPath}\\{model.Name}\\{gpx.Id}.{extension}";
            var taskResult = this.googleDriveService.UloadGpxFileToDrive(gxpFilePath, pathToServiceAccountKeyFile, model.Name, DirectoryId);
            var googleId = taskResult.Result;

            gpx.GoogleDriveId = googleId;
            gpx.GoogleDriveDirectoryId = DirectoryId;

            try
            {
                await this.gpxRepo.AddAsync(gpx);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var ride = new Ride()
            {
                Name = model.Name,
                Date = (DateTime)model.Date,
                CreatedOn = DateTime.Now,
                Description = model.Description,
                FormatId = int.Parse(model.FormatId),
                UserId = userId,
                Town = townData,
                Mountain = mountainData,
                Trace = new Trace()
                {
                    Name = model.Trace.Name,
                    DifficultyId = model.Trace.DifficultyId,
                    ControlTime = TimeSpan.FromHours((double)model.Trace.ControlTime),
                    Length = (int)model.Trace.Length,
                    CreatedOn = DateTime.Now,
                    StartTime = (DateTime)model.Trace.StartTime,
                    Gpx = gpx,
                },
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

        public async Task EditAsync(RideEditVIewModel model)
        {
            var rideDb = this
                .rideRepo
                .All()
                .Include(r => r.Mountain)
                .Include(r => r.Town)
                .FirstOrDefault(r => r.Id == model.Id);

            if (rideDb == null)
            {
                throw new Exception(IvalidOperationMessage);
            }

            rideDb.ModifiedOn = DateTime.UtcNow;

            if (rideDb.Mountain.Name != model.Mountain)
            {
                var mountain = this.mountainRepo.All().FirstOrDefault(m => m.Name == model.Name);

                if (mountain == null)
                {
                    mountain = new Mountain()
                    {
                        Name = model.Mountain,
                    };

                    try
                    {
                        await this.mountainRepo.AddAsync(mountain);
                    }
                    catch (Exception)
                    {
                        throw new Exception(IvalidOperationMessage);
                    }
                }

                rideDb.Mountain = mountain;
            }

            if (rideDb.Town.Name != model.Town)
            {
                var town = this.townRepo.All().FirstOrDefault(m => m.Name == model.Name);

                if (town == null)
                {
                    town = new Town()
                    {
                        Name = model.Town,
                    };

                    await this.townRepo.AddAsync(town);
                }

                rideDb.Town = town;
            }

            rideDb.Description = model.Description;
            rideDb.FormatId = int.Parse(model.FormatId);
            rideDb.Date = model.Date;
            rideDb.Name = model.Name;

            var traceDb = this
                .traceRepo
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
