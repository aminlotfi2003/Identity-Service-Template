using IdentityService.Domain.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Iam;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> b)
    {
        b.ToTable("RolePermissions", ApplicationDbContext.IamSchema);
        b.HasKey(x => x.Id);
        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique();
    }
}
