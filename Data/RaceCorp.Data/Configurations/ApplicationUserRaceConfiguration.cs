namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class ApplicationUserRaceConfiguration : IEntityTypeConfiguration<ApplicationUserRace>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRace> appUserRace)
        {
            appUserRace
               .HasOne(l => l.ApplicationUser)
               .WithMany(u => u.Races)
               .HasForeignKey(l => l.ApplicationUserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
