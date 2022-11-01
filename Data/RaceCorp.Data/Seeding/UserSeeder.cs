using RaceCorp.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RaceCorp.Data.Seeding
{
    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            await dbContext.AddAsync(new ApplicationUser
            {
                CreatedOn = DateTime.Now,
                Email = "yborisov@gmail.com",
                UserName = "yborisov@gmail.com",
                NormalizedEmail = "yborisov@gmail.com".ToUpper(),
                NormalizedUserName = "yborisov@gmail.com".ToUpper(),
                FirstName = "Yordan",
                LastName = "Borisov",
                Gender = "Male",
                Town = new Town { CreatedOn = DateTime.Now, Name = "Sofia" },
                Country = "Bulgaria",
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
            }); ;
        }
    }
}
