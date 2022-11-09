// ReSharper disable VirtualMemberCallInConstructor
namespace RaceCorp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;

    using RaceCorp.Data.Common.Models;
    using RaceCorp.Data.Models.Enums;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser() =>
            this.Id = Guid.NewGuid().ToString();

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string LinkedInLink { get; set; }

        public string FacoBookLink { get; set; }

        public string GitHubLink { get; set; }

        public string TwitterLink { get; set; }

        public Gender Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public int? TownId { get; set; }

        public virtual Town Town { get; set; }

        public string Country { get; set; }

        public string ProfilePicturePath { get; set; }

        public string? TeamId { get; set; }

        public virtual Team Team { get; set; }

        public string? MemberInTeamId { get; set; }

        public string About { get; set; }

        public virtual Team MemberInTeam { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();

        public virtual ICollection<Logo> Logos { get; set; } = new HashSet<Logo>();

        public virtual ICollection<Gpx> Gpxs { get; set; } = new HashSet<Gpx>();

        public virtual ICollection<Ride> CreatedRides { get; set; } = new HashSet<Ride>();

        public virtual ICollection<Race> CreatedRaces { get; set; } = new HashSet<Race>();

        public virtual ICollection<ApplicationUserRace> Races { get; set; } = new HashSet<ApplicationUserRace>();

        public virtual ICollection<ApplicationUserRide> Rides { get; set; } = new HashSet<ApplicationUserRide>();

        public virtual ICollection<ApplicationUserTrace> Traces { get; set; } = new HashSet<ApplicationUserTrace>();

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = new HashSet<IdentityUserRole<string>>();

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new HashSet<IdentityUserClaim<string>>();

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; } = new HashSet<IdentityUserLogin<string>>();
    }
}
