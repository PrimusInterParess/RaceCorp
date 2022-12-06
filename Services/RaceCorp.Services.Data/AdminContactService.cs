namespace RaceCorp.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Messaging;
    using RaceCorp.Web.ViewModels.Common;

    public class AdminContactService : IAdminContactService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<AdminContact> adminContactRepo;
        private readonly IRepository<ApplicationRole> roleRepo;
        private readonly IEmailSender emailSender;

        public AdminContactService(
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<AdminContact> adminContactRepo,
            IRepository<ApplicationRole> roleRepo,
            IEmailSender emailSender)
        {
            this.userRepo = userRepo;
            this.adminContactRepo = adminContactRepo;
            this.roleRepo = roleRepo;
            this.emailSender = emailSender;
        }

        public async Task ReceiveMessage(ContactFormModel model)
        {
            var adminRoleId = this.roleRepo
                .All()
                .FirstOrDefault(r => r.Name == GlobalConstants.AdministratorRoleName)?.Id;

            var admin = this.userRepo
                .All()
                .Include(u => u.AdminContacts)
                .Where(u => u.Roles.Any(r => r.RoleId == adminRoleId))
                .FirstOrDefault();

            await this.emailSender.SendEmailAsync(GlobalConstants.AdminEmail, GlobalConstants.ServiceAccountName, GlobalConstants.AdminEmail, model.Subject, model.Content + " " + "Sender" + model.Email);

            var adminContact = new AdminContact
            {
                Admin = admin,
                Content = model.Content,
                Subject = model.Subject,
                ContactEmail = model.Email,
                CreatedOn = System.DateTime.UtcNow,
                ContactName = model.Name,
            };

            admin.AdminContacts.Add(adminContact);

            await this.adminContactRepo.AddAsync(adminContact);
            await this.adminContactRepo.SaveChangesAsync();
        }
    }
}
