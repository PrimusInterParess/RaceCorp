namespace RaceCorp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RaceCorp.Data.Models;

    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> image)
        {
            image
                 .HasOne(i => i.Team)
                 .WithMany(t => t.Images)
                 .OnDelete(DeleteBehavior.Cascade);

            image
               .HasOne(l => l.ApplicationUser)
               .WithMany(u => u.Images)
               .HasForeignKey(l => l.ApplicationUserId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
