namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualBasic;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Ride;

    public class RideService : IRideService
    {
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;

        public RideService(IDeletableEntityRepository<Ride> rideRepo, IDeletableEntityRepository<Town> townRepo, IDeletableEntityRepository<Mountain> mountainRepo)
        {
            this.rideRepo = rideRepo;
            this.townRepo = townRepo;
            this.mountainRepo = mountainRepo;
        }

        public async Task CreateAsync(RideCreateViewModel model, string userId)
        {
            // validate entities!!!
            var town = this.townRepo.AllAsNoTracking().FirstOrDefault(t => t.Name == model.Name);
            var mountain = this.mountainRepo.AllAsNoTracking().FirstOrDefault(m => m.Name == model.Name);

            // TODO: Validate inputData!
            var ride = new Ride()
            {
                Name = model.Name,
                Date = (DateTime)model.Date,
                CreatedOn = DateTime.Now,
                Description = model.Description,
                FormatId = int.Parse(model.FormatId),
                UserId = userId,
                Town = town,
                Mountain = mountain,
            };

            await this.rideRepo.AddAsync(ride);
            await this.rideRepo.SaveChangesAsync();
        }
    }
}
