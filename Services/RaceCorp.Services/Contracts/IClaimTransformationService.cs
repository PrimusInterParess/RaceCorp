namespace RaceCorp.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;

    public interface IClaimTransformationService
    {
        Task<bool> AddRegistrationClaims(ApplicationUser user, ClaimsPrincipal claimsPrincipal);

        Task<bool> UpdateClaim(ApplicationUser user, ClaimsPrincipal claimsPrincipal, string claimType, string value);
    }
}
