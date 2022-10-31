// ReSharper disable VirtualMemberCallInConstructor
namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    using RaceCorp.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public string Country { get; set; }

        public string PictureId { get; set; }

        public virtual Image ProfilePicture { get; set; }

        public string TeamId { get; set; }

        public virtual Team Team { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<ApplicationUserRace> Races { get; set; } = new HashSet<ApplicationUserRace>();

        public virtual ICollection<ApplicationUserRide> Rides { get; set; } = new HashSet<ApplicationUserRide>();

        public virtual ICollection<ApplicationUserTrace> Traces { get; set; } = new HashSet<ApplicationUserTrace>();
    }
}
