using IdentityService.Domain.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Identity;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> b)
    {
        b.ToTable("Users", ApplicationDbContext.IdentitySchema);
        b.Property(p => p.TenantId);
        b.Property(p => p.PasswordLastChangedAt);
        b.Property(p => p.MustChangePasswordOnFirstLogin).HasDefaultValue(false);
        b.Property(p => p.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        b.Property(p => p.ModifiedAt);
        b.Property(p => p.IsActive).HasDefaultValue(true);

        b.HasIndex(u => u.NormalizedUserName).IsUnique();
        b.HasIndex(u => u.NormalizedEmail);
    }
}
