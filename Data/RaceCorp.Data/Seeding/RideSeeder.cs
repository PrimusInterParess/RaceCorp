namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Common;
    using RaceCorp.Data.Models;

    public class RideSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Rides.Any())
            {
                return;
            }

            var userId = dbContext.Users.FirstOrDefault(u => u.Email == "yborisov@gmail.com")?.Id;

            await dbContext.Rides.AddAsync(new Ride()
            {
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddDays(5),
                Description = "Amazing trace! Come join us! Let's have fun!",
                FormatId = 3,
                MountainId = 2,
                TownId = 1,
                Name = "Golden Bridges",
                ApplicationUserId = userId,
                Trace = new Trace
                {
                    Name = "Boyana-Golden Bridges",
                    CreatedOn = DateTime.Now,
                    DifficultyId = 2,
                    ControlTime = TimeSpan.FromHours(10),
                    Length = 39,
                    StartTime = DateTime.Now.AddDays(5),
                    GpxPath = "\\Gpxs\\Golden Bridges\\goldenBridges.gpx",
                    MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1X-l7S5MX4rw9JrSrakVp-AAgVVXmHTQf"),

                    Gpx = new Gpx
                    {
                        ParentFolderName = "Gpxs",
                        CreatedOn = DateTime.Now,
                        Extension = "gpx",
                        ChildFolderName = "Golden Bridges",
                        GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                        GoogleDriveId = "1X-l7S5MX4rw9JrSrakVp-AAgVVXmHTQf",
                        Id = "goldenBridges",
                        ApplicationUserId = userId,
                    },
                },
            });

            await dbContext.Rides.AddAsync(new Ride()
            {
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddDays(10),
                Description = "Fun uphill,fun downhill! Les't ride!",
                FormatId = 3,
                MountainId = 2,
                TownId = 1,
                Name = "Town Portal",
                ApplicationUserId = userId,
                Trace = new Trace
                {
                    Name = "Simeonovo-Dragaletsi",
                    CreatedOn = DateTime.Now,
                    DifficultyId = 2,
                    ControlTime = TimeSpan.FromHours(10),
                    Length = 19,
                    StartTime = DateTime.Now.AddDays(10),
                    GpxPath = "\\Gpxs\\Town Portal\\townPortalSimeonovo-Dragaletsi.gpx",
                    MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1Jl4CvWN_nh6m14RO_-ugYSmpNa4R4uKa"),

                    Gpx = new Gpx
                    {
                        ParentFolderName = "Gpxs",
                        CreatedOn = DateTime.Now,
                        Extension = "gpx",
                        ChildFolderName = "Town Portal",
                        GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                        GoogleDriveId = "1Jl4CvWN_nh6m14RO_-ugYSmpNa4R4uKa",
                        Id = "townPortalSimeonovo-Dragaletsi",
                        ApplicationUserId = userId,
                    },
                },
            });

            await dbContext.Rides.AddAsync(new Ride()
            {
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddDays(30),
                Description = "Fun uphill,fun downhill! Les't ride!",
                FormatId = 3,
                MountainId = 2,
                TownId = 1,
                Name = "Town Portal",
                ApplicationUserId = userId,
                Trace = new Trace
                {
                    Name = "Simeonovo-Pancharevo",
                    CreatedOn = DateTime.Now,
                    DifficultyId = 2,
                    ControlTime = TimeSpan.FromHours(10),
                    Length = 19,
                    StartTime = DateTime.Now.AddDays(30),
                    GpxPath = "\\Gpxs\\Town Portal\\townPortal.gpx",
                    MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1v4Nx9YNGCeD8WySvhvj_ukGVDNUC4lz3"),

                    Gpx = new Gpx
                    {
                        ParentFolderName = "Gpxs",
                        CreatedOn = DateTime.Now,
                        Extension = "gpx",
                        ChildFolderName = "Town Portal",
                        GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                        GoogleDriveId = "1v4Nx9YNGCeD8WySvhvj_ukGVDNUC4lz3",
                        Id = "townPortal",
                        ApplicationUserId = userId,
                    },
                },
            });

            await dbContext.Rides.AddAsync(new Ride()
            {
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddDays(60),
                Description = "Fun uphill,fun downhill! Les't ride!",
                FormatId = 3,
                MountainId = 2,
                TownId = 1,
                Name = "Town Portal",
                ApplicationUserId = userId,
                Trace = new Trace
                {
                    Name = "Gorna Bania-Pancharevo",
                    CreatedOn = DateTime.Now,
                    DifficultyId = 2,
                    ControlTime = TimeSpan.FromHours(10),
                    Length = 24,
                    StartTime = DateTime.Now.AddDays(60),
                    GpxPath = "\\Gpxs\\Town Portal\\townPortalGornaBaniaPancharevo.gpx",
                    MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1De6IwgWzfPTbPdY0cO-Eh1v9wQOfhwVw"),

                    Gpx = new Gpx
                    {
                        ParentFolderName = "Gpxs",
                        CreatedOn = DateTime.Now,
                        Extension = "gpx",
                        ChildFolderName = "Town Portal",
                        GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                        GoogleDriveId = "1De6IwgWzfPTbPdY0cO-Eh1v9wQOfhwVw",
                        Id = "townPortalGornaBaniaPancharevo",
                        ApplicationUserId = userId,
                    },
                },
            });

            await dbContext.Rides.AddAsync(new Ride()
            {
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddDays(120),
                Description = "Fun uphill,fun downhill! Les't ride!",
                FormatId = 3,
                MountainId = 1,

                TownId = 4,
                Name = "Kresna",
                ApplicationUserId = userId,
                Trace = new Trace
                {
                    Name = "Kresna-Vlahi-Sinanitsa",
                    CreatedOn = DateTime.Now,
                    DifficultyId = 2,
                    ControlTime = TimeSpan.FromHours(10),
                    Length = 39,
                    StartTime = DateTime.Now.AddDays(120),
                    GpxPath = "\\Gpxs\\Kresna\\kresnaEpic.gpx",
                    MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1EKOlp-9gBYmpY8hzx8jxlMbCdVdYS-gH"),

                    Gpx = new Gpx
                    {
                        ParentFolderName = "Gpxs",
                        CreatedOn = DateTime.Now,
                        Extension = "gpx",
                        ChildFolderName = "Kresna",
                        GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                        GoogleDriveId = "1EKOlp-9gBYmpY8hzx8jxlMbCdVdYS-gH",
                        Id = "kresnaEpic",
                        ApplicationUserId = userId,
                    },
                },
            });
        }
    }
}
