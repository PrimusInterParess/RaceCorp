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

            await dbContext.Towns.AddAsync(new Town() { Name = "Dragoman" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Buhovo" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Kresna" });

            await dbContext.Towns.AddAsync(new Town() { Name = "Asenovgrad" });
        }
    }
}
