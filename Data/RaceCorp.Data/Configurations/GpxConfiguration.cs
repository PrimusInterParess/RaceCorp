namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RaceCorp.Data.Models;

    public class GpxConfiguration : IEntityTypeConfiguration<Gpx>
    {
        public void Configure(EntityTypeBuilder<Gpx> gpx)
        {
            gpx
              .HasOne(g => g.ApplicationUser)
              .WithMany()
              .HasForeignKey(g => g.ApplicationUserId)
              .OnDelete(DeleteBehavior.SetNull);

            gpx
              .HasOne(g => g.ApplicationUser)
              .WithMany(u => u.Gpxs)
              .HasForeignKey(g => g.ApplicationUserId);
        }
    }
}
