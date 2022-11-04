namespace RaceCorp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Contracts;

    public class ClaimTransformationService : IClaimTransformationService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ClaimTransformationService(
                       SignInManager<ApplicationUser> signInManager,
                       UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<bool> AddRegistrationClaims(ApplicationUser user, ClaimsPrincipal claimsPrincipal)
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return false;
            }

            var fullName = $"{user.FirstName} {user.LastName}";
            var gender = user.Gender;

            await this.userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, fullName));
            await this.userManager.AddClaimAsync(user, new Claim(ClaimTypes.Gender, gender));

            return true;
        }

        public async Task<bool> UpdateClaim(ApplicationUser user, ClaimsPrincipal claimsPrincipal, string claimType, string value)
        {
            // Get User and a claims-based identity

            var identity = new ClaimsIdentity(claimsPrincipal.Identity);

            // Remove existing claim and replace with a new value
            await this.userManager.RemoveClaimAsync(user, identity.FindFirst(claimType));
            await this.userManager.AddClaimAsync(user, new Claim(claimType, value));

            // Re-Signin User to reflect the change in the Identity cookie
            await this.signInManager.SignInAsync(user, isPersistent: false);

            return true;
        }

    }
}
