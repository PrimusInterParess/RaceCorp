namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Contracts;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.ApplicationUsers;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IFileService fileService;
        private readonly IClaimTransformationService claimsTransformationService;

        public UserService(
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Town> townRepo,
            IFileService fileService,
            IClaimTransformationService claimsTransformationService)
        {
            this.userRepo = userRepo;
            this.townRepo = townRepo;
            this.fileService = fileService;
            this.claimsTransformationService = claimsTransformationService;
        }

        public async Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath, ClaimsPrincipal claimPrincipal)
        {
            var user = this.userRepo.All().Include(u => u.ProfilePicture).Include(u => u.Town).FirstOrDefault(u => u.Id == inputModel.Id);

            if (user.FirstName != inputModel.FirstName || user.LastName != inputModel.LastName)
            {
                var value = $"{inputModel.FirstName} {inputModel.LastName}";

                var result = await this.claimsTransformationService.UpdateClaim(user, claimPrincipal, ClaimTypes.GivenName, value);
                user.FirstName = inputModel.FirstName;
                user.LastName = inputModel.LastName;
            }

            if (user.Gender != inputModel.Gender)
            {
                var result = await this.claimsTransformationService.UpdateClaim(user, claimPrincipal, ClaimTypes.Gender, inputModel.Gender);
                user.Gender = inputModel.Gender;
            }

            if (inputModel.UserProfilePicture != null)
            {
                var image = await this.fileService.ProccessingImageData(inputModel.UserProfilePicture, inputModel.Id, roothPath, inputModel.FirstName + inputModel.LastName);
                user.ProfilePicture = image;
            }

            var townDb = this.townRepo.All().FirstOrDefault(t => t.Name == inputModel.Town);

            if (townDb == null)
            {
                townDb = new Town
                {
                    Name = inputModel.Town,
                };

                await this.townRepo.AddAsync(townDb);
                user.Town = townDb;
            }

            user.Country = inputModel.Country;

            await this.userRepo.SaveChangesAsync();

            return true;
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
            catch (Exception)
            {
                throw new InvalidOperationException("Cannot upload profile picture");
            }

            await this.userRepo.SaveChangesAsync();

            return true;
        }
    }
}
