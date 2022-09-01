namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;

    public class MountainSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Mountains.Any())
            {
                return;
            }

            await dbContext.Mountains.AddAsync(new Mountain() { Name = "Pirin" });

            await dbContext.Mountains.AddAsync(new Mountain() { Name = "Rila" });

            await dbContext.Mountains.AddAsync(new Mountain() { Name = "Vitosha" });

            await dbContext.Mountains.AddAsync(new Mountain() { Name = "Balkan Mountains" });

            await dbContext.Mountains.AddAsync(new Mountain() { Name = "Osogovo" });

            await dbContext.Mountains.AddAsync(new Mountain() { Name = "Belasitza" });

            await dbContext.Mountains.AddAsync(new Mountain() { Name = "Lulin Mountain" });
        }
    }
}
