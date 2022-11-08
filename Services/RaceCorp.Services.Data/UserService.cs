﻿namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.ApplicationUsers;

    public class UserService : IUserService
    {
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IFileService fileService;


        public UserService(
             IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Town> townRepo,
            IFileService fileService)
        {
            this.userStore = userStore;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.townRepo = townRepo;
            this.fileService = fileService;
        }

        public async Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath)
        {
            var user = this.userRepo.All().Include(u => u.ProfilePicture).Include(u => u.Town).FirstOrDefault(u => u.Id == inputModel.Id);

            if (inputModel.UserProfilePicture != null)
            {
                var image = await this.fileService
                    .ProccessingProfilePictureData(
                    inputModel.UserProfilePicture,
                    inputModel.Id,
                    roothPath,
                    inputModel.FirstName + inputModel.LastName + GlobalConstants.ProfilePicterPostFix);

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

            user.About = inputModel.About;

            if (user.FirstName != inputModel.FirstName ||
               user.LastName != inputModel.LastName)
            {
                user.FirstName = inputModel.FirstName;
                user.LastName = inputModel.LastName;

                await this.UpdateClaim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}", user);
            }

            if (user.Gender != inputModel.Gender)
            {
                user.Gender = inputModel.Gender;
                await this.UpdateClaim(ClaimTypes.Gender, inputModel.Gender.ToString(), user);
            }

            if (user.Country != inputModel.Country)
            {
                user.Country = inputModel.Country;
                await this.UpdateClaim(ClaimTypes.Country, inputModel.Gender.ToString(), user);
            }

            if (user.DateOfBirth != inputModel.DateOfBirth)
            {
                user.DateOfBirth = inputModel.DateOfBirth;
                await this.UpdateClaim(ClaimTypes.DateOfBirth, inputModel.Gender.ToString(), user);
            }

            await this.userRepo.SaveChangesAsync();

            return true;
        }

        public T GetById<T>(string id)
        {
            return this.userRepo.All().Where(u => u.Id == id).To<T>().FirstOrDefault();
        }

        private async Task UpdateClaim(string claimType, string value, ApplicationUser user)
        {
            var claim = this.userManager.GetClaimsAsync(user).Result.FirstOrDefault(c => c.Type == claimType);

            if (claim == null)
            {
                await this.userManager.AddClaimAsync(user, new Claim(claimType, value));
            }
            else
            {
                user.Claims.Where(c => c.ClaimType == claimType).FirstOrDefault().ClaimValue = value;
            }

            await this.userManager.UpdateAsync(user);
            await this.signInManager.SignInAsync(user, isPersistent: false);
        }
    }
}
