namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> conncetion)
        {
            conncetion
               .HasOne(c => c.ApplicationUser)
            .WithMany(u => u.Connections)
            .HasForeignKey(u => u.ApplicationUserId);

            conncetion
                .HasOne(c => c.Interlocutor)
                .WithMany(u => u.InterlocutorConnections)
                .HasForeignKey(u => u.InterlocutorId);
        }
    }
}
