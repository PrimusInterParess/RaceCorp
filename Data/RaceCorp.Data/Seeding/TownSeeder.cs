namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;

    public class TownSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Towns.Any())
            {
                return;
            }

            await dbContext.Towns.AddAsync(new Town() { Name = "Sofia" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Plovdiv" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Vratsa" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Stara Zagora" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Petrich" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Kazanlak" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Belogradchik" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Asenovgrad" });
        }
    }
}
