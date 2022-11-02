using Microsoft.AspNetCore.Identity;
using RaceCorp.Common;
using RaceCorp.Data.Models;
using System;
using System.Collections.Generic;
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
                Id = "DaniBorisov",
                CreatedOn = DateTime.Now,
                Email = "yborisov@gmail.com",
                UserName = "yborisov@gmail.com",
                NormalizedEmail = "yborisov@gmail.com".ToUpper(),
                NormalizedUserName = "yborisov@gmail.com".ToUpper(),
                FirstName = "Yordan",
                LastName = "Borisov",
                Gender = "Male",
                TownId = 1,
                Country = "Bulgaria",
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = GlobalConstants.RaceOwnerRoleName }, },
            });

            await dbContext.AddAsync(new ApplicationUser
            {
                Id = "KarolinaBorisova",
                CreatedOn = DateTime.Now,
                Email = "kborisova@gmail.com",
                UserName = "kborisova@gmail.com",
                NormalizedEmail = "=kborisova@gmail.com".ToUpper(),
                NormalizedUserName = "kborisova@gmail.com".ToUpper(),
                FirstName = "Karolina",
                LastName = "Borisova",
                Gender = "Female",
                TownId = 1,
                Country = "Bulgaria",
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = GlobalConstants.RacerRoleName }, },

            });

            await dbContext.AddAsync(new ApplicationUser
            {

                Id = "KrumBorisov",
                CreatedOn = DateTime.Now,
                Email = "kborisov@gmail.com",
                UserName = "kborisov@gmail.com",
                NormalizedEmail = "kborisov@gmail.com".ToUpper(),
                NormalizedUserName = "kborisov@gmail.com".ToUpper(),
                FirstName = "Krum",
                LastName = "Borisov",
                Gender = "Male",
                TownId = 1,
                Country = "Bulgaria",
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = GlobalConstants.RacerRoleName }, },

            });

            await dbContext.AddAsync(new ApplicationUser
            {

                Id = "EstelleBorisova",
                CreatedOn = DateTime.Now,
                Email = "eborisova@gmail.com",
                UserName = "eborisova@gmail.com",
                NormalizedEmail = "=eborisova@gmail.com".ToUpper(),
                NormalizedUserName = "eborisova@gmail.com".ToUpper(),
                FirstName = "Estelle",
                LastName = "Borisova",
                Gender = "Female",
                TownId = 1,
                Country = "Bulgaria",
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",

            });

            await dbContext.AddAsync(new ApplicationUser
            {
                Id = "NebesnaBorisova",
                CreatedOn = DateTime.Now,
                Email = "nborisova@gmail.com",
                UserName = "nborisova@gmail.com",
                NormalizedEmail = "=nborisova@gmail.com".ToUpper(),
                NormalizedUserName = "nborisova@gmail.com".ToUpper(),
                FirstName = "Nebesna",
                LastName = "Borisova",
                Gender = "Female",
                TownId = 1,
                Country = "Bulgaria",
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = GlobalConstants.RacerRoleName }, },

            });
        }
    }
}
