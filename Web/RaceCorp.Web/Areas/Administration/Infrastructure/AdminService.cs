namespace RaceCorp.Web.Areas.Administration.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Messaging;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;
    using RaceCorp.Web.Areas.Administration.Models;
    using RaceCorp.Web.Areas.Administration.Models.Admin;
    using RaceCorp.Web.Areas.Administration.Models.Message;

    using static RaceCorp.Services.Constants.Common;

    public class AdminService : IAdminService
    {
        private readonly IFileService fileService;
        private readonly IRepository<Image> imageRepo;
        private readonly IAdminRideService adminRideService;
        private readonly IAdminRaceService adminRaceService;
        private readonly IRaceService raceService;
        private readonly IDeletableEntityRepository<AdminContact> adminContactRepo;
        private readonly IDeletableEntityRepository<AdminContactReply> adminContacReplyRepo;
        private readonly IEmailSender emailSender;

        public AdminService(
            IFileService fileService,
            IRepository<Image> imageRepo,
            IAdminRideService adminRideService,
            IAdminRaceService adminRaceService,
            IRaceService raceService,
            IDeletableEntityRepository<AdminContact> adminContactRepo,
            IDeletableEntityRepository<AdminContactReply> adminContacReplyRepo,
            IEmailSender emailSender)
        {
            this.fileService = fileService;
            this.imageRepo = imageRepo;
            this.adminRideService = adminRideService;
            this.adminRaceService = adminRaceService;
            this.raceService = raceService;
            this.adminContactRepo = adminContactRepo;
            this.adminContacReplyRepo = adminContacReplyRepo;
            this.emailSender = emailSender;
        }

        public DashboardIndexViewModel GetIndexModel()
        {
            return new DashboardIndexViewModel
            {
                NoOwnerRaces = this.adminRaceService.GetNoOwnerRaces(),
                NoOwnerRides = this.adminRideService.GetNoOwnerRides(),
            };
        }

        public MessageProfileModel GetMessage(int id)
        {
            var contactAdminDto = this
                .adminContactRepo
                .All()
                .Include(c => c.AdminContactReply)
                .Select(m => new MessageProfileModel
                {
                    Id = m.Id,
                    ContactEmail = m.ContactEmail,
                    ContactName = m.ContactName,
                    Content = m.Content,
                    CreatedOn = m.CreatedOn.ToString(GlobalConstants.DateMessageFormat),
                    Subject = m.Subject,
                    ReplyContent = m.AdminContactReply.Content,
                    ReplyDate = m.AdminContactReply.CreatedOn.ToString(GlobalConstants.DateMessageFormat),
                })
                .FirstOrDefault(m => m.Id == id);

            if (contactAdminDto == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.NotExistingContent);
            }

            return contactAdminDto;
        }

        public ICollection<AdminContactMessage> GetMessages()
        {
            return this.adminContactRepo
                .All()
                .Include(c => c.Admin)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new AdminContactMessage
                {
                    Id = c.Id,
                    Email = c.ContactEmail,
                    Subject = c.Subject,
                    CreatedOn = c.CreatedOn.ToString(GlobalConstants.DateMessageFormat),
                    IsReplied = c.IsReplied,
                })
                .ToList();
        }

        public async Task SaveReply(MessageProfileModel inputModel)
        {
            var adminContact = this.adminContactRepo
                .All()
                .Include(c => c.AdminContactReply)
                .Include(c => c.Admin)
                .FirstOrDefault(c => c.Id == inputModel.Id);

            var admin = adminContact.Admin;

            if (adminContact == null ||
                inputModel.ReplyContent == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.NotExistingContent);
            }

            var reply = new AdminContactReply
            {
                Admin = admin,
                Content = inputModel.ReplyContent,
                CreatedOn = DateTime.UtcNow,
                AdminContact = adminContact,
            };

            adminContact.AdminContactReply = reply;
            adminContact.IsReplied = true;

            var replySubject = $"Reply about {adminContact.Subject}";

            await this.emailSender.SendEmailAsync(admin.Email, GlobalConstants.AdminName, adminContact.ContactEmail, replySubject, inputModel.ReplyContent);

            await this.adminContacReplyRepo.AddAsync(reply);
            await this.adminContacReplyRepo.SaveChangesAsync();
        }

        public async Task UploadingPicture(PictureUploadModel inputModel, string roothPath, string userId)
        {
            var image = await this.fileService.ProccessingImageData(inputModel.Picture, userId, roothPath, SystemImageFolderName);
            image.Name = inputModel.Type;

            await this.imageRepo.SaveChangesAsync();
        }
    }
}
