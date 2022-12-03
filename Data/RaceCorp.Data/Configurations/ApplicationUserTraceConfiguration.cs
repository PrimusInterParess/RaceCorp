namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class ApplicationUserTraceConfiguration : IEntityTypeConfiguration<ApplicationUserTrace>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserTrace> appUserTrace)
        {
            appUserTrace
                 .HasOne(l => l.ApplicationUser)
                 .WithMany(u => u.Traces)
                 .HasForeignKey(l => l.ApplicationUserId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
