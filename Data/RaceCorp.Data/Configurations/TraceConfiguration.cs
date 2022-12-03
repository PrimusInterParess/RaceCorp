namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class TraceConfiguration : IEntityTypeConfiguration<Trace>
    {
        public void Configure(EntityTypeBuilder<Trace> trace)
        {
            trace
                .HasOne(l => l.Ride)
               .WithOne(r => r.Trace)
               .HasForeignKey<Ride>(r => r.TraceId);

            trace
                .HasOne(t => t.Gpx)
                .WithOne(g => g.Trace)
                .HasForeignKey<Trace>(t => t.GpxId);
        }
    }
}
