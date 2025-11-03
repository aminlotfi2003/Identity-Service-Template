using IdentityService.Domain.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Iam;

public sealed class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> b)
    {
        b.ToTable("UserPermissions", ApplicationDbContext.IamSchema);
        b.HasKey(x => x.Id);
        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => new { x.UserId, x.PermissionId }).IsUnique();

        b.HasOne<Permission>()
         .WithMany()
         .HasForeignKey(x => x.PermissionId)
         .OnDelete(DeleteBehavior.Restrict)
         .HasConstraintName("FK_UserPermission_Permission");
    }
}
