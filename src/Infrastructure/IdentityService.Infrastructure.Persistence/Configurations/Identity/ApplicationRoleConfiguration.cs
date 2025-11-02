using IdentityService.Domain.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Identity;

public sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> b)
    {
        b.ToTable("Roles", ApplicationDbContext.IdentitySchema);
        b.Property(p => p.Description).HasMaxLength(256);
        b.Property(p => p.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        b.HasIndex(r => r.NormalizedName).IsUnique();
    }
}
