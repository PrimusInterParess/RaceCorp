namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;
    using RaceCorp.Data.Models.Enums;

    public class DifficultySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Difficulties.Any())
            {
                return;
            }

            await dbContext.Difficulties.AddAsync(new Difficulty() { Level = (DifficultyLevel)1 });

            await dbContext.Difficulties.AddAsync(new Difficulty() { Level = (DifficultyLevel)2 });

            await dbContext.Difficulties.AddAsync(new Difficulty() { Level = (DifficultyLevel)3 });

            await dbContext.Difficulties.AddAsync(new Difficulty() { Level = (DifficultyLevel)4 });
        }
    }
}
