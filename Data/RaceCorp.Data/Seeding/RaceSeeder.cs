namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Common;
    using RaceCorp.Data.Models;

    public class RaceSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Races.Any())
            {
                return;
            }

            var userId = dbContext.Users.FirstOrDefault(u => u.Email == "yborisov@gmail.com")?.Id;

            await dbContext.Races.AddAsync(new Race
            {
                Name = "Vitosha100km",
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddMonths(7),
                Description = "Most popular race in Bulgaria!",
                FormatId = 2,
                ApplicationUserId = userId,
                LogoPath = "\\Images\\logos\\vitosha100.jpg",
                Logo = new Logo
                {
                    ParentFolderName = "Images",
                    ChildFolderName = "logos",
                    CreatedOn = DateTime.Now,
                    Extension = "jpg",
                    Id = "vitosha100",
                    ApplicationUserId = userId,
                },
                MountainId = 2,
                TownId = 1,
                Traces = new HashSet<Trace>()
                {
                    new Trace
                    {
                        Name = "MTB",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 1,
                        StartTime = DateTime.Now.AddMonths(7),
                        ControlTime = TimeSpan.FromHours(18),
                        Length = 100,
                        GpxPath = "\\Gpxs\\Vitosha100km\\vitosha100km.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1arDdkmCrnPrYfZPKq6NlISKOOKq2BXFY"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "Vitosha100km",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1arDdkmCrnPrYfZPKq6NlISKOOKq2BXFY",
                            Id = "vitosha100km",
                            ApplicationUserId = userId,
                        },
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race
            {
                Name = "Murgash",
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddMonths(7),
                Description = "Test yourself! We have tree different traces you can pick from! From beginers to pros!",
                FormatId = 2,
                ApplicationUserId = userId,
                LogoPath = "\\Images\\logos\\murgash.jpg",
                Logo = new Logo
                {
                    ParentFolderName = "Images",
                    ChildFolderName = "logos",
                    CreatedOn = DateTime.Now,
                    Extension = "jpg",
                    Id = "murgash",
                    ApplicationUserId = userId,
                },
                MountainId = 5,
                TownId = 3,
                Traces = new HashSet<Trace>()
                {
                    new Trace
                    {
                        Name = "Picnic",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 4,
                        StartTime = DateTime.Now.AddMonths(5),
                        ControlTime = TimeSpan.FromHours(8),
                        Length = 19,
                        GpxPath = "\\Gpxs\\Murgash\\murgashPicnic.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1DQqa_i8S-FSfJNi9WEJLWWQl0bVM4LrV"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "Murgash",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1DQqa_i8S-FSfJNi9WEJLWWQl0bVM4LrV",
                            Id = "murgashPicnic",
                            ApplicationUserId = userId,
                        },
                    },
                    new Trace
                    {
                        Name = "Classic",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 2,
                        StartTime = DateTime.Now.AddMonths(5),
                        ControlTime = TimeSpan.FromHours(14),
                        Length = 44,
                        GpxPath = "\\Gpxs\\Murgash\\murgashClassic.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1JodQPhuPOmba9KuyU8U-ZI2DFvlsUnkL"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "Murgash",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1JodQPhuPOmba9KuyU8U-ZI2DFvlsUnkL",
                            Id = "murgashClassic",
                            ApplicationUserId = userId,
                        },
                    },
                    new Trace
                    {
                        Name = "Epic",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 1,
                        StartTime = DateTime.Now.AddMonths(5),
                        ControlTime = TimeSpan.FromHours(18),
                        Length = 75,
                        GpxPath = "\\Gpxs\\Murgash\\murgashEpic.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "12X2AxNk2nMYBiYHYrg1OmTpjoB82rTI6"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",

                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "Murgash",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "12X2AxNk2nMYBiYHYrg1OmTpjoB82rTI6",
                            Id = "murgashEpic",
                            ApplicationUserId = userId,
                        },
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race
            {
                Name = "Bike4Chepan",
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddMonths(7),
                Description = "Speed,skills,concentration,endurance - all needed for the race!Test yourself!",
                FormatId = 2,
                ApplicationUserId = userId,
                LogoPath = "\\Images\\logos\\bike4chepan.png",
                Logo = new Logo
                {
                    ParentFolderName = "Images",
                    ChildFolderName = "logos",
                    CreatedOn = DateTime.Now,
                    Extension = "png",
                    Id = "bike4chepan",
                    ApplicationUserId = userId,
                },
                MountainId = 3,
                TownId = 2,
                Traces = new HashSet<Trace>()
                {
                    new Trace
                    {
                        Name = "Long",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 2,
                        StartTime = DateTime.Now.AddMonths(3),
                        ControlTime = TimeSpan.FromHours(12),
                        Length = 42,
                        GpxPath = "\\Gpxs\\Bike4Chepan\\bike4ChepanLong.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1JIhx8oyweUJmytzwbsPp5QBAbe0KuJVD"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "Bike4Chepan",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1JIhx8oyweUJmytzwbsPp5QBAbe0KuJVD",
                            Id = "bike4ChepanLong",
                            ApplicationUserId = userId,
                        },
                    },
                    new Trace
                    {
                        Name = "Short",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 3,
                        StartTime = DateTime.Now.AddMonths(3).AddHours(13),
                        ControlTime = TimeSpan.FromHours(12),
                        Length = 42,
                        GpxPath = "\\Gpxs\\Bike4Chepan\\bike4ChepanShort.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1Za4X3oxzkgXIqzgvBH3TLxtPQrUOoSC8"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "Bike4Chepan",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1Za4X3oxzkgXIqzgvBH3TLxtPQrUOoSC8",
                            Id = "bike4ChepanShort",
                            ApplicationUserId = userId,
                        },
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race
            {
                Name = "XCO Dragalevo",
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddMonths(5),
                Description = "One of the best's XCO's you can race! Come and dig deep!",
                FormatId = 1,
                ApplicationUserId = userId,
                LogoPath = "\\Images\\logos\\xcoDragalevci.jpg",
                Logo = new Logo
                {
                    ParentFolderName = "Images",
                    ChildFolderName = "logos",
                    CreatedOn = DateTime.Now,
                    Extension = "jpg",
                    Id = "xcoDragalevci",
                    ApplicationUserId = userId,
                },
                MountainId = 2,
                TownId = 1,
                Traces = new HashSet<Trace>()
                {
                    new Trace
                    {
                        Name = "XCO Dragalevo",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 1,
                        StartTime = DateTime.Now.AddMonths(5),
                        ControlTime = TimeSpan.FromHours(5),
                        Length = 18,
                        GpxPath = "\\Gpxs\\XCODragalevo\\xcoDragalevo.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1BXdW6q-1985PtsdGsf2vfU2CT7hP7equ"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "XCODragalevo",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1BXdW6q-1985PtsdGsf2vfU2CT7hP7equ",
                            Id = "xcoDragalevo",
                            ApplicationUserId = userId,
                        },
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race
            {
                Name = "XCO Simeonovo",
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddMonths(4),
                Description = "Roller Coaster. Come and have fun with one of the best racers in Sofia!",
                FormatId = 1,
                ApplicationUserId = userId,
                LogoPath = "\\Images\\logos\\xcoSimeonovo.jpg",

                Logo = new Logo
                {
                    ParentFolderName = "Images",
                    ChildFolderName = "logos",
                    CreatedOn = DateTime.Now,
                    Extension = "jpg",
                    Id = "xcoSimeonovo",
                    ApplicationUserId = userId,
                },
                MountainId = 2,
                TownId = 1,
                Traces = new HashSet<Trace>()
                {
                    new Trace
                    {
                        Name = "XCO Simeonovo",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 2,
                        StartTime = DateTime.Now.AddMonths(4),
                        ControlTime = TimeSpan.FromHours(4),
                        Length = 16,
                        GpxPath = "\\Gpxs\\XCOSimeonovo\\xcoSimeonovo.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1hg3hgYLJnIgxLi1vUAUHuMbNRdz030Hw"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",

                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "XCOSimeonovo",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1hg3hgYLJnIgxLi1vUAUHuMbNRdz030Hw",
                            Id = "xcoSimeonovo",
                            ApplicationUserId = userId,
                        },
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race
            {
                Name = "Asenovgradski Bairi",
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddMonths(10),
                Description = "In the heart of Rodopi Mountain,race that can challenge your skills,endurance,will!",
                FormatId = 2,
                ApplicationUserId = userId,
                LogoPath = "\\Images\\logos\\asenovgradskiBairi.png",

                Logo = new Logo
                {
                    ParentFolderName = "Images",
                    ChildFolderName = "logos",
                    CreatedOn = DateTime.Now,
                    Extension = "png",
                    Id = "asenovgradskiBairi",
                    ApplicationUserId = userId,
                },
                MountainId = 4,
                TownId = 5,
                Traces = new HashSet<Trace>()
                {
                    new Trace
                    {
                        Name = "Long",
                        CreatedOn = DateTime.Now,
                        DifficultyId = 2,
                        StartTime = DateTime.Now.AddMonths(10),
                        ControlTime = TimeSpan.FromHours(9),
                        Length = 42,
                        GpxPath = "\\Gpxs\\AsenovgradskiBairi\\asenovgradskiBairiLong.gpx",
                        MapUrl = string.Format(GlobalConstants.MapUrlTraceGpx, "1qc1_wE28CCfAzvpRQsmyzatBYYIS62Sr"),

                        Gpx = new Gpx
                        {
                            ParentFolderName = "Gpxs",
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            ChildFolderName = "AsenovgradskiBairi",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1qc1_wE28CCfAzvpRQsmyzatBYYIS62Sr",
                            Id = "asenovgradskiBairiLong",
                            ApplicationUserId = userId,
                        },
                    },
                },
            });
        }
    }
}
