namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.User;

    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IFileService fileService;
        private readonly IDeletableEntityRepository<Request> requestRepo;

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Town> townRepo,
            IFileService fileService,
            IDeletableEntityRepository<Request> requestRepo)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.townRepo = townRepo;
            this.fileService = fileService;
            this.requestRepo = requestRepo;
        }

        public async Task<bool> EditAsync(UserEditViewModel inputModel, string roothPath)
        {
            var user = this.userRepo.All()
                .Include(u => u.Images).Include(u => u.Town).FirstOrDefault(u => u.Id == inputModel.Id);

            if (inputModel.UserProfilePicture != null)
            {
                var image = await this.fileService
                    .ProccessingImageData(
                    inputModel.UserProfilePicture,
                    inputModel.Id,
                    roothPath,
                    inputModel.FirstName + inputModel.LastName + GlobalConstants.ProfilePicterPostFix);

                user.ProfilePicturePath = $"\\{image.ParentFolderName}\\{image.ChildFolderName}\\{image.Id}.{image.Extension}";
                image.Name = GlobalConstants.ProfilePictire;
                user.Images.Add(image);
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

            user.FacoBookLink = inputModel.FacoBookLink;
            user.LinkedInLink = inputModel.LinkedInLink;
            user.TwitterLink = inputModel.TwitterLink;
            user.GitHubLink = inputModel.GitHubLink;

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

            await this.userManager.UpdateAsync(user);
            await this.signInManager.RefreshSignInAsync(user);
            await this.userRepo.SaveChangesAsync();

            return true;
        }

        public List<T> GetAllAsync<T>()
        {
            return this.userRepo.AllAsNoTracking().To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            return this.userRepo
                .All()
                .Where(u => u.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public List<T> GetRequest<T>(string userId)
        {
            return this.requestRepo
                .AllAsNoTracking()
                .Include(r => r.TargetUser)
                .Where(r => r.TargetUserId == userId)
                .OrderBy(r => r.IsApproved)
                .To<T>()
                .ToList();
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
        }

        public bool RequestedConnection(string currentUserId, string targetUserId)
        {
            return this.userRepo
                .AllAsNoTracking()
                .Include(u => u.SendRequests)
                .FirstOrDefault(u => u.Id == currentUserId)
                .SendRequests
                .Any(r => r.RequesterId == targetUserId);
        }

        public string GetUserEmail(string userId)
        {
            return this.userRepo.AllAsNoTracking().FirstOrDefault(u => u.Id == userId)?.Email;
        }
    }
}
