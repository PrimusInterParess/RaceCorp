namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;

    public class RideSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Rides.Any())
            {
                return;
            }

            await dbContext.Rides.AddAsync(new Ride()
            {
                Name = "Golden Bridges with BT Revolution",
                Date = new DateTime(2022, 4, 22),
                FormatId = 2,
                CreatedOn = DateTime.Now,
                MountainId = 1,
                TownId = 1,
                Trace = new Trace()
                {
                    DifficultyId = 1,
                    CreatedOn = DateTime.Now,
                    Length = 50,
                    Name = "Golden Bridges",
                    StartTime = new DateTime(2022, 4, 22, 10, 0, 0),
                    TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221DIhM-I_g-4owocu19CrWf9igDd-MzkJ5%22%5D%7D",
                },
            });
        }
    }
}
