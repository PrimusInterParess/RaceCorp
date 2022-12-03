namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class RideConfiguration : IEntityTypeConfiguration<Ride>
    {
        public void Configure(EntityTypeBuilder<Ride> ride)
        {
            ride
                 .HasOne(l => l.ApplicationUser)
                 .WithMany(u => u.CreatedRides)
                 .HasForeignKey(l => l.ApplicationUserId)
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
