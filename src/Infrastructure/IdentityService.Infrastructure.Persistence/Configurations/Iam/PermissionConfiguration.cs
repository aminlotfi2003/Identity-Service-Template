using IdentityService.Domain.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Iam;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> b)
    {
        b.ToTable("Permissions", ApplicationDbContext.IamSchema);
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).HasMaxLength(1024);
        b.Property(x => x.Category).HasMaxLength(128);
        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => x.Name).IsUnique();
    }
}
