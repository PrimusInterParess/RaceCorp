namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RaceCorp.Data.Models;

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> appUser)
        {
            appUser
              .HasMany(e => e.Claims)
              .WithOne()
              .HasForeignKey(e => e.UserId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

            appUser
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            appUser
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            appUser
                .HasMany(u => u.SentMessages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId);

            appUser
                .HasMany(u => u.InboxMessages)
                .WithOne(m => m.Receiver)
                .HasForeignKey(m => m.RevceiverId);

            appUser
                .HasMany(u => u.Connections)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);

            appUser
                .HasMany(u => u.Conversations)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);

            appUser
                .HasOne(u => u.MemberInTeam)
               .WithMany(t => t.TeamMembers)
               .HasForeignKey(u => u.MemberInTeamId);

            appUser
                .HasMany(a => a.Images)
                .WithOne(i => i.ApplicationUser)
                .HasForeignKey(i => i.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            appUser
                .HasMany(u => u.Requests)
                .WithOne(r => r.TargetUser)
                .HasForeignKey(r => r.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
