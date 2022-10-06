namespace RaceCorp.Services.Data
{
    using System;
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

    public class RideService : IRideService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;

        public RideService(
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Town> townRepo,
            IDeletableEntityRepository<Mountain> mountainRepo)
        {
            this.rideRepo = rideRepo;
            this.townRepo = townRepo;
            this.mountainRepo = mountainRepo;
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
