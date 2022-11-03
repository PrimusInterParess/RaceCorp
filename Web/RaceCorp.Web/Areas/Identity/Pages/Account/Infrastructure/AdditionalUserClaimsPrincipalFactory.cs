namespace RaceCorp.Web.Areas.Identity.Pages.Account.Infrastructure
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using RaceCorp.Data.Models;

    public class AdditionalUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public AdditionalUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            var fullName = $"{user.FirstName} {user.LastName}";

            var gender = user.Gender;
            var claims = new List<Claim>();

            claims.Add(new Claim("FullName", fullName));
            claims.Add(new Claim("Gender", gender));

            identity.AddClaims(claims);
            return principal;
        }
    }
}
