namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
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

            await dbContext.Races.AddAsync(new Race()
            {
                Name = "Kresna",
                FormatId = 2,
                LogoId = "Kresna",
                Date = DateTime.Now,
                CreatedOn = new DateTime(2022, 4, 22),
                MountainId = 1,
                Description = "No beginers allowed! Only pro-riders! Up,down - great fun!",
                TownId = 4,
                Traces = new List<Trace>()
                {
                    new Trace()
                    {
                        Name = "Fury",
                        Length = 50,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(18.0),
                        DifficultyId = 1,
                        StartTime = new DateTime(2022, 4, 22, 10, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221nd3WLIpgfDKCEsb7bhnqEZdy1diKSxPJ%22%5D%7D",
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race()
            {
                Name = "Vitosha100km",
                FormatId = 2,
                LogoId = "Vitosha100km",
                Date = DateTime.Now,
                CreatedOn = new DateTime(2022, 6, 18),
                MountainId = 2,
                Description = "The most popular race in Bulgaria! Come,ride,have fun!",
                TownId = 1,
                Traces = new List<Trace>()
                {
                    new Trace()
                    {
                        Name = "MTB",
                        Length = 100,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(18.0),
                        DifficultyId = 1,
                        StartTime = new DateTime(2022, 6, 18, 6, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221muY_eyxWszlLhYAtus5_HuWB1Et4xXAZ%22%5D%7D",
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race()
            {
                Name = "Dragalevo",
                FormatId = 1,
                LogoId = "Dragalevo",
                Date = DateTime.Now,
                CreatedOn = new DateTime(2022, 4, 22),
                MountainId = 2,
                Description = "Rollercoster in the heart of Vitosha! Difficult trace with very tehnical course!",
                TownId = 1,
                Traces = new List<Trace>()
                {
                    new Trace()
                    {
                        Name = "Dragalevo XCO",
                        Length = 18,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(4.0),
                        DifficultyId = 1,
                        StartTime = new DateTime(2022, 4, 22, 10, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221BXdW6q-1985PtsdGsf2vfU2CT7hP7equ%22%5D%7D",
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race()
            {
                Name = "Bike4Chepan",
                FormatId = 1,
                LogoId = "Bike4Chepan",
                Date = DateTime.Now,
                CreatedOn = new DateTime(2022, 5, 11),
                MountainId = 3,
                Description = "First race of the season! Come and check your level!",
                TownId = 2,
                Traces = new List<Trace>()
                {
                    new Trace()
                    {
                        Name = "Long",
                        Length = 44,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(10.0),
                        DifficultyId = 2,
                        StartTime = new DateTime(2022, 5, 11, 6, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221JIhx8oyweUJmytzwbsPp5QBAbe0KuJVD%22%5D%7D",
                    },
                    new Trace()
                    {
                        Name = "Short",
                        Length = 24,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(7.0),
                        DifficultyId = 2,
                        StartTime = new DateTime(2022, 5, 11, 6, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221Za4X3oxzkgXIqzgvBH3TLxtPQrUOoSC8%22%5D%7D",
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race()
            {
                Name = "Murgash",
                FormatId = 1,
                LogoId = "Murgash",
                Date = DateTime.Now,
                CreatedOn = new DateTime(2022, 9, 17),
                MountainId = 5,
                Description = "Most demanding and difficult race you can ride! Come test your value!",
                TownId = 3,
                Traces = new List<Trace>()
                {
                    new Trace()
                    {
                        Name = "Epic",
                        Length = 75,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(19.0),
                        DifficultyId = 1,
                        StartTime = new DateTime(2022, 9, 17, 9, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221a1OiDa6oQcxh2yNXSbZgexMJ27Eh0Lmc%22%5D%7D",
                    },
                    new Trace()
                    {
                        Name = "Classic",
                        Length = 45,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(14.0),
                        DifficultyId = 2,
                        StartTime = new DateTime(2022, 9, 17, 6, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221qua4f7H0yFNLWzD8x4-i5hTzkmAdFCR5%22%5D%7D",
                    },
                    new Trace()
                    {
                        Name = "Picnik",
                        Length = 20,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(7.0),
                        DifficultyId = 4,
                        StartTime = new DateTime(2022, 9, 17, 6, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221DQqa_i8S-FSfJNi9WEJLWWQl0bVM4LrV%22%5D%7D",
                    },
                },
            });

            await dbContext.Races.AddAsync(new Race()
            {
                Name = "Asenovgradski bairi",
                FormatId = 2,
                LogoId = "AsenovgradskiBairi",
                Date = DateTime.Now,
                CreatedOn = new DateTime(2022, 7, 12),
                MountainId = 4,
                Description = "Race not for bigginers. Really tehnical on the uphill and donw hill. Be prepared!",
                TownId = 5,
                Traces = new List<Trace>()
                {
                    new Trace()
                    {
                        Name = "Long",
                        Length = 42,
                        CreatedOn = DateTime.Now,
                        ControlTime = TimeSpan.FromHours(8.0),
                        DifficultyId = 1,
                        StartTime = new DateTime(2022, 7, 12, 11, 0, 0),
                        TrackUrl = "https://gpx.studio/?state=%7B%22ids%22:%5B%221nd3WLIpgfDKCEsb7bhnqEZdy1diKSxPJ%22%5D%7D",
                    },
                },
            });
        }
    }
}
