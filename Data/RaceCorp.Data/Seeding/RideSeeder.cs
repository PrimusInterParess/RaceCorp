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
                    ControlTime = TimeSpan.FromHours(8.0),
                    DifficultyId = 4,
                    CreatedOn = DateTime.Now,
                    Length = 50,
                    Name = "Golden Bridges",
                    StartTime = new DateTime(2022, 4, 22, 10, 0, 0),
                    TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221DIhM-I_g-4owocu19CrWf9igDd-MzkJ5%22%5D%7D",
                },
            });

            await dbContext.Rides.AddAsync(new Ride()
            {
                Name = "Town Portal with The Pro",
                Date = new DateTime(2022, 4, 22),
                FormatId = 2,
                CreatedOn = DateTime.Now,
                MountainId = 1,
                TownId = 1,
                Trace = new Trace()
                {
                    ControlTime = TimeSpan.FromHours(4.0),
                    DifficultyId = 3,
                    CreatedOn = DateTime.Now,
                    Length = 20,
                    Name = "Town Portal",
                    StartTime = new DateTime(2022, 4, 22, 10, 0, 0),
                    TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221v4Nx9YNGCeD8WySvhvj_ukGVDNUC4lz3%22%5D%7D",
                },
            });

            await dbContext.Rides.AddAsync(new Ride()
            {
                Name = "Simeonvo-Dragaleci with The Pro",
                Date = new DateTime(2022, 4, 22),
                FormatId = 2,
                CreatedOn = DateTime.Now,
                MountainId = 1,
                TownId = 1,
                Trace = new Trace()
                {
                    ControlTime = TimeSpan.FromHours(3.0),
                    DifficultyId = 1,
                    CreatedOn = DateTime.Now,
                    Length = 20,
                    Name = "Simeonvo-Dragaleci",
                    StartTime = new DateTime(2022, 4, 22, 10, 0, 0),
                    TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221HjOBDl4wtcaMOt6DR9-GfCqCMnclQkHS%22%5D%7D",
                },
            });

            await dbContext.Rides.AddAsync(new Ride()
            {
                Name = "To Panchaka with The Pro",
                Date = new DateTime(2022, 4, 22),
                FormatId = 2,
                CreatedOn = DateTime.Now,
                MountainId = 1,
                TownId = 1,
                Trace = new Trace()
                {
                    ControlTime = TimeSpan.FromHours(4.0),
                    DifficultyId = 2,
                    CreatedOn = DateTime.Now,
                    Length = 24,
                    Name = "To Panchaka",
                    StartTime = new DateTime(2022, 4, 22, 10, 0, 0),
                    TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221qaK-FNt5digW_BZfVDhNgdi62WDeZyD2%22%5D%7D",
                },
            });
        }
    }
}
