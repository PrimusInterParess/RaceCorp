namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using static RaceCorp.Services.Constants.Common;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IFileService fileService;

        public UserService(IDeletableEntityRepository<ApplicationUser> userRepo, IFileService fileService)
        {
            this.userRepo = userRepo;
            this.fileService = fileService;
        }

        public T GetById<T>(string id)
        {
            return this.userRepo.All().Where(u => u.Id == id).To<T>().FirstOrDefault();
        }

        public async Task<bool> SaveProfileImage(IFormFile inputFile, string userId, string roothPath)
        {
            var user = this.userRepo.All().FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("Invalid operation");
            }

            try
            {
                var image = await this.fileService.ProccessingImageData(inputFile, userId, roothPath, user.Email);
                user.ProfilePicture = image;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot upload profile picture");
            }

            await this.userRepo.SaveChangesAsync();

            return true;
        }
    }
}