using IdentityService.Domain.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Identity;

public sealed class ApplicationUserLoginConfiguration : IEntityTypeConfiguration<ApplicationUserLogin>
{
    public void Configure(EntityTypeBuilder<ApplicationUserLogin> b)
    {
        b.ToTable("UserLogins", ApplicationDbContext.IdentitySchema);
        b.HasKey(x => new { x.LoginProvider, x.ProviderKey });
    }
}
