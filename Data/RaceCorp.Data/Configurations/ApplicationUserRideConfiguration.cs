namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RaceCorp.Data.Models;

    public class ApplicationUserRideConfiguration : IEntityTypeConfiguration<ApplicationUserRide>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRide> appUserRide)
        {
            appUserRide
                 .HasOne(l => l.ApplicationUser)
                 .WithMany(u => u.Rides)
                 .HasForeignKey(l => l.ApplicationUserId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
