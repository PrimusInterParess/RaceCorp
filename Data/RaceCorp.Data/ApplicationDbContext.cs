namespace RaceCorp.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Data.Common.Models;
    using RaceCorp.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Gpx> Gpxs { get; set; }

        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Ride> Rides { get; set; }

        public DbSet<Race> Races { get; set; }

        public DbSet<Trace> Traces { get; set; }

        public DbSet<Format> Formats { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Mountain> Mountains { get; set; }

        public DbSet<Logo> Logos { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<Gpx>().HasOne(g => g.ApplicationUser).WithMany().HasForeignKey(g => g.ApplicationUserId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Logo>().HasOne(l => l.ApplicationUser).WithMany(u => u.Logos).HasForeignKey(l => l.ApplicationUserId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Image>().HasOne(l => l.ApplicationUser).WithMany(u => u.Images).HasForeignKey(l => l.ApplicationUserId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Ride>().HasOne(l => l.ApplicationUser).WithMany(u => u.CreatedRides).HasForeignKey(l => l.ApplicationUserId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Race>().HasOne(l => l.ApplicationUser).WithMany(u => u.CreatedRaces).HasForeignKey(l => l.ApplicationUserId).OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ApplicationUserRace>().HasOne(l => l.ApplicationUser).WithMany(u => u.Races).HasForeignKey(l => l.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUserRide>().HasOne(l => l.ApplicationUser).WithMany(u => u.Rides).HasForeignKey(l => l.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUserTrace>().HasOne(l => l.ApplicationUser).WithMany(u => u.Traces).HasForeignKey(l => l.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>().HasMany(u => u.Requests).WithOne(r => r.ApplicationUser).HasForeignKey(r => r.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>().HasMany(u => u.Requests).WithOne(r => r.ApplicationUser).HasForeignKey(r => r.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>().HasMany(u => u.Connections).WithOne(c => c.ApplicationUser).HasForeignKey(c => c.ApplicationUserId);

            builder.Entity<Message>().HasOne(m => m.Receiver).WithMany(r => r.InboxMessages).HasForeignKey(m => m.RevceiverId);
            builder.Entity<Message>().HasOne(m => m.Sender).WithMany(r => r.SentMessages).HasForeignKey(m => m.SenderId);

            builder.Entity<Image>().HasOne(i => i.Team).WithMany(t => t.Images).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Gpx>()
                .HasOne(g => g.ApplicationUser)
                .WithMany(u => u.Gpxs)
                .HasForeignKey(g => g.ApplicationUserId);

            builder.Entity<ApplicationUser>()
               .HasOne(u => u.MemberInTeam)
              .WithMany(t => t.TeamMembers)
              .HasForeignKey(u => u.MemberInTeamId);

            builder.Entity<ApplicationUser>()
          .HasMany(a => a.Images)
          .WithOne(i => i.ApplicationUser)
          .HasForeignKey(i => i.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Team>()
                .HasOne(t => t.ApplicationUser)
                .WithOne(u => u.Team)
                .HasForeignKey<Team>(t => t.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Logo>()
                .HasOne(l => l.Race)
                .WithOne(r => r.Logo)
                .HasForeignKey<Race>(r => r.LogoId);

            builder.Entity<Trace>()
                            .HasOne(l => l.Ride)
                            .WithOne(r => r.Trace)
                            .HasForeignKey<Ride>(r => r.TraceId);

            builder.Entity<Trace>().HasOne(t => t.Gpx).WithOne(g => g.Trace).HasForeignKey<Trace>(t => t.GpxId);
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
