using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Infrastructure.Persistence.Configurations;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.FullName).IsRequired().HasMaxLength(150);

        builder.Property(user => user.Email).IsRequired().HasMaxLength(200);

        builder.Property(user => user.PhoneNumber).HasMaxLength(30);

        builder.Property(user => user.UserType).IsRequired();

        builder.Property(user => user.IsActive).IsRequired();

        builder.Property(user => user.CreatedAt).IsRequired();

        builder.HasIndex(user => user.Email).IsUnique();

        builder.HasMany(user => user.NotificationPreferences).WithOne(preference => preference.AppUser).HasForeignKey(preference => preference.AppUserId).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(user => user.NotificationLogs).WithOne(log => log.AppUser).HasForeignKey(log => log.AppUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
