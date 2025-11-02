using IdentityService.Domain.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Identity;

public sealed class ApplicationUserTokenConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserToken> b)
    {
        b.ToTable("UserTokens", ApplicationDbContext.IdentitySchema);
        b.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
    }
}
