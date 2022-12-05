namespace RaceCorp.Web.Areas.Administration.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;
    using RaceCorp.Web.Areas.Administration.Models.User;

    public class AdminUserService : IAdminUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IRepository<ApplicationRole> roleRepo;

        public AdminUserService(
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IRepository<ApplicationRole> roleRepo)
        {
            this.userRepo = userRepo;
            this.roleRepo = roleRepo;
        }

        public List<UserAllDashboardModel> GetAllUsers()
        {
            var adminRoleId = this.roleRepo.All().FirstOrDefault(r => r.Name == GlobalConstants.AdministratorRoleName)?.Id;

            return this.userRepo
                .AllWithDeleted()
                .Include(u => u.Roles)
                .Where(u => u.Roles.Any(r => r.RoleId == adminRoleId) == false)
                .Select(u => new UserAllDashboardModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    IsDeleted = u.IsDeleted,
                }).ToList();
        }

        public async Task DeleteUser(string id)
        {
            var race = this.userRepo
                .All()
                .FirstOrDefault(u => u.Id == id);

            this.userRepo.Delete(race);

            await this.userRepo.SaveChangesAsync();
        }

        public async Task UndeleteUser(string id)
        {
            var user = this.userRepo
               .AllWithDeleted()
               .FirstOrDefault(u => u.Id == id);

            user.IsDeleted = false;

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
