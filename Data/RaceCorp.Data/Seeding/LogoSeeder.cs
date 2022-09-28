namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;

    public class LogoSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Logos.Any())
            {
                return;
            }

            await dbContext.Logos.AddAsync(new Logo() { Id = "Kresna", Extension = "jpg" });

            await dbContext.Logos.AddAsync(new Logo() { Id = "Vitosha100km", Extension = "jpg" });

            await dbContext.Logos.AddAsync(new Logo() { Id = "Dragalevo", Extension = "jpg" });

            await dbContext.Logos.AddAsync(new Logo() { Id = "Bike4Chepan", Extension = "png" });

            await dbContext.Logos.AddAsync(new Logo() { Id = "Murgash", Extension = "jpg" });

            await dbContext.Logos.AddAsync(new Logo() { Id = "AsenovgradskiBairi", Extension = "jpg" });
        }
    }
}
