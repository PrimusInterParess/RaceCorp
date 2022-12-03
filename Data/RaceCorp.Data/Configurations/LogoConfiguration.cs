namespace RaceCorp.Data.Configurations
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class LogoConfiguration : IEntityTypeConfiguration<Logo>
    {
        public void Configure(EntityTypeBuilder<Logo> logo)
        {
            logo
              .HasOne(l => l.Race)
              .WithOne(r => r.Logo)
              .HasForeignKey<Race>(r => r.LogoId);

            logo
               .HasOne(l => l.ApplicationUser)
               .WithMany(u => u.Logos)
               .HasForeignKey(l => l.ApplicationUserId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
