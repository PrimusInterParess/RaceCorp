namespace RaceCorp.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var adminRole = await roleManager.FindByNameAsync(roleName);
            if (adminRole == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            var racerRole = await roleManager.FindByNameAsync(GlobalConstants.RacerRoleName);
            if (racerRole == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole()
                {
                    Id = GlobalConstants.RacerRoleName,
                    Name = GlobalConstants.RacerRoleName,
                });
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            var raceOwnerRole = await roleManager.FindByNameAsync(GlobalConstants.RaceOwnerRoleName);
            if (raceOwnerRole == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole()
                {
                    Id = GlobalConstants.RaceOwnerRoleName,
                    Name = GlobalConstants.RaceOwnerRoleName,
                });
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
