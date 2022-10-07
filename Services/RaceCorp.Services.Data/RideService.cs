namespace RaceCorp.Services.Data
{
    using System;
    using System.Diagnostics;
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
    using static RaceCorp.Services.Constants.Messages;

    using Trace = RaceCorp.Data.Models.Trace;

    public class RideService : IRideService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Trace> traceRepo;

        public RideService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Town> townRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Trace> traceRepo)
        {
            this.rideRepo = rideRepo;
            this.townRepo = townRepo;
            this.mountainRepo = mountainRepo;
            this.traceRepo = traceRepo;
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
                    TrackUrl = r.Trace.TrackUrl,
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

        public async Task CreateAsync(RideCreateViewModel model, string userId)
        {
            // validate entities!!!
            var town = this
                .townRepo
                .AllAsNoTracking()
                .FirstOrDefault(t => t.Name == model.Town);
            var mountain = this
                .mountainRepo
                .AllAsNoTracking()
                .FirstOrDefault(m => m.Name == model.Mountain);

            // TODO: Validate inputData!
            var ride = new Ride()
            {
                Name = model.Name,
                Date = (DateTime)model.Date,
                CreatedOn = DateTime.Now,
                Description = model.Description,
                FormatId = int.Parse(model.FormatId),
                UserId = userId,
                TownId = town.Id,
                MountainId = mountain.Id,
                Trace = new Trace()
                {
                    Name = model.Trace.Name,
                    DifficultyId = model.Trace.DifficultyId,
                    ControlTime = TimeSpan.FromHours((double)model.Trace.ControlTime),
                    Length = (int)model.Trace.Length,
                    CreatedOn = DateTime.Now,
                    StartTime = (DateTime)model.Trace.StartTime,
                    TrackUrl = model.Trace.TrackUrl,
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
            traceDb.TrackUrl = model.Trace.TrackUrl;
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
