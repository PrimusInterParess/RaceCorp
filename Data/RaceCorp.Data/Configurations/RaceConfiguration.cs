namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class RaceConfiguration : IEntityTypeConfiguration<Race>
    {
        public void Configure(EntityTypeBuilder<Race> race)
        {
            race
                .HasOne(l => l.ApplicationUser)
                .WithMany(u => u.CreatedRaces)
                .HasForeignKey(l => l.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
