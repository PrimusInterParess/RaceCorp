namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;

    public class RaceSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Races.Any())
            {
                return;
            }

            await dbContext.Races.AddAsync(new Race
            {
                Name = "Vitosha100km",
                CreatedOn = DateTime.Now,
                Date = DateTime.Now.AddMonths(7),
                Description = "Most popular race in Bulgaria!",
                FormatId = 2,
                Logo = new Logo
                {
                    CreatedOn = DateTime.Now,
                    Extension = "jpg",
                    Id = "vitosha100",
                    UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "Vitosha100km",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1arDdkmCrnPrYfZPKq6NlISKOOKq2BXFY",
                            Id = "vitosha100km",
                            UserId = "DaniBorisov",
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
                Logo = new Logo
                {
                    CreatedOn = DateTime.Now,
                    Extension = "jpg",
                    Id = "murgash",
                    UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "Murgash",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1DQqa_i8S-FSfJNi9WEJLWWQl0bVM4LrV",
                            Id = "murgashPicnic",
                            UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "Murgash",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1JodQPhuPOmba9KuyU8U-ZI2DFvlsUnkL",
                            Id = "murgashClassic",
                            UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "Murgash",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "12X2AxNk2nMYBiYHYrg1OmTpjoB82rTI6",
                            Id = "murgashEpic",
                            UserId = "DaniBorisov",
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
                Logo = new Logo
                {
                    CreatedOn = DateTime.Now,
                    Extension = "png",
                    Id = "bike4chepan",
                    UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "Bike4Chepan",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1JIhx8oyweUJmytzwbsPp5QBAbe0KuJVD",
                            Id = "bike4ChepanLong",
                            UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "Bike4Chepan",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1Za4X3oxzkgXIqzgvBH3TLxtPQrUOoSC8",
                            Id = "bike4ChepanShort",
                            UserId = "DaniBorisov",
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
                Logo = new Logo
                {
                    CreatedOn = DateTime.Now,
                    Extension = "jpg",
                    Id = "xcoDragalevci",
                    UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "XCODragalevo",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1BXdW6q-1985PtsdGsf2vfU2CT7hP7equ",
                            Id = "xcoDragalevo",
                            UserId = "DaniBorisov",
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
                Logo = new Logo
                {
                    CreatedOn = DateTime.Now,
                    Extension = "jpg",
                    Id = "xcoSimeonovo",
                    UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "XCOSimeonovo",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1hg3hgYLJnIgxLi1vUAUHuMbNRdz030Hw",
                            Id = "xcoSimeonovo",
                            UserId = "DaniBorisov",
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
                Logo = new Logo
                {
                    CreatedOn = DateTime.Now,
                    Extension = "png",
                    Id = "asenovgradskiBairi",
                    UserId = "DaniBorisov",
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
                        Gpx = new Gpx
                        {
                            CreatedOn = DateTime.Now,
                            Extension = "gpx",
                            FolderName = "AsenovgradskiBairi",
                            GoogleDriveDirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa",
                            GoogleDriveId = "1qc1_wE28CCfAzvpRQsmyzatBYYIS62Sr",
                            Id = "asenovgradskiBairiLong",
                            UserId = "DaniBorisov",
                        },
                    },
                },
            });
        }
    }
}
