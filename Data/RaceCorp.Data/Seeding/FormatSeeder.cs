namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;
    using RaceCorp.Data.Models.Enums;

    public class FormatSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Formats.Any())
            {
                return;
            }

            await dbContext.Formats.AddAsync(new Format() { Name = "XCO" });

            await dbContext.Formats.AddAsync(new Format() { Name = "XCM" });

            await dbContext.Formats.AddAsync(new Format() { Name = "XC" });

            await dbContext.Formats.AddAsync(new Format() { Name = "XCC" });

            await dbContext.Formats.AddAsync(new Format() { Name = "DH" });

            await dbContext.Formats.AddAsync(new Format() { Name = "XCE" });

            await dbContext.Formats.AddAsync(new Format() { Name = "Enduro" });

            await dbContext.Formats.AddAsync(new Format() { Name = "Cyclocross" });
        }
    }
}
