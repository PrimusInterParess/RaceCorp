namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;

    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var userRoleId = dbContext.Roles.FirstOrDefault(r => r.Name == GlobalConstants.UserRoleName)?.Id;
            var adminRoleId = dbContext.Roles.FirstOrDefault(r => r.Name == GlobalConstants.AdministratorRoleName)?.Id;

            await dbContext.AddAsync(new ApplicationUser
            {
                CreatedOn = DateTime.Now,
                Email = "diesonnekind@gmail.com",
                UserName = "diesonnekind@gmail.com",
                NormalizedEmail = "diesonnekind@gmail.com".ToUpper(),
                NormalizedUserName = "diesonnekind@gmail.com".ToUpper(),
                FirstName = "Yordan",
                LastName = "Borisov",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.DaniProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>()
                {
                    new IdentityUserRole<string>() { RoleId = adminRoleId },
                },
            });

            await dbContext.AddAsync(new ApplicationUser
            {
                CreatedOn = DateTime.Now,
                Email = "kborisova@gmail.com",
                UserName = "kborisova@gmail.com",
                NormalizedEmail = "=kborisova@gmail.com".ToUpper(),
                NormalizedUserName = "kborisova@gmail.com".ToUpper(),
                FirstName = "Karolina",
                LastName = "Borisova",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.KariProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = userRoleId }, },
            });

            await dbContext.AddAsync(new ApplicationUser
            {
                CreatedOn = DateTime.Now,
                Email = "kborisov@gmail.com",
                UserName = "kborisov@gmail.com",
                NormalizedEmail = "kborisov@gmail.com".ToUpper(),
                NormalizedUserName = "kborisov@gmail.com".ToUpper(),
                FirstName = "Krum",
                LastName = "Borisov",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.KrumProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = userRoleId }, },
            });

            await dbContext.AddAsync(new ApplicationUser
            {
                CreatedOn = DateTime.Now,
                Email = "eborisova@gmail.com",
                UserName = "eborisova@gmail.com",
                NormalizedEmail = "=eborisova@gmail.com".ToUpper(),
                NormalizedUserName = "eborisova@gmail.com".ToUpper(),
                FirstName = "Estelle",
                LastName = "Borisova",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.EstelleProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = userRoleId }, },
            });

            await dbContext.AddAsync(new ApplicationUser
            {
                CreatedOn = DateTime.Now,
                Email = "nborisova@gmail.com",
                UserName = "nborisova@gmail.com",
                NormalizedEmail = "nborisova@gmail.com".ToUpper(),
                NormalizedUserName = "nborisova@gmail.com".ToUpper(),
                FirstName = "Nebesna",
                LastName = "Borisova",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.NebesnaProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = userRoleId }, },
            });
        }
    }
}
