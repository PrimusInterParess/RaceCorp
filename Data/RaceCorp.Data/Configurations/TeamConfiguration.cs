namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> team)
        {
            team
                .HasOne(t => t.ApplicationUser)
                .WithOne(u => u.Team)
                .HasForeignKey<Team>(t => t.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
