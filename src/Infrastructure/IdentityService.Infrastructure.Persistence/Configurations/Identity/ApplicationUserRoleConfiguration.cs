using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IdentityService.Domain.Identity;

namespace IdentityService.Infrastructure.Persistence.Configurations.Identity;

public sealed class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> b)
    {
        b.ToTable("UserRoles", ApplicationDbContext.IdentitySchema);
        b.HasKey(x => new { x.UserId, x.RoleId });
    }
}
