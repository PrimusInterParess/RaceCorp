namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.ApplicationUsers;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IFileService fileService;


        public UserService(
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Town> townRepo,
            IFileService fileService)
        {
            this.userRepo = userRepo;
            this.townRepo = townRepo;
            this.fileService = fileService;
        }

        public async Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath, ClaimsPrincipal claimPrincipal)
        {
            var user = this.userRepo.All().Include(u => u.ProfilePicture).Include(u => u.Town).FirstOrDefault(u => u.Id == inputModel.Id);

            if (user.FirstName != inputModel.FirstName || user.LastName != inputModel.LastName)
            {
                user.FirstName = inputModel.FirstName;
                user.LastName = inputModel.LastName;
            }

            if (user.Gender != inputModel.Gender)
            {
                user.Gender = inputModel.Gender;
            }

            if (inputModel.UserProfilePicture != null)
            {
                var image = await this.fileService.ProccessingProfilePictureData(inputModel.UserProfilePicture, inputModel.Id, roothPath, inputModel.FirstName + inputModel.LastName + GlobalConstants.ProfilePicterPostFix);
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
    }
}
