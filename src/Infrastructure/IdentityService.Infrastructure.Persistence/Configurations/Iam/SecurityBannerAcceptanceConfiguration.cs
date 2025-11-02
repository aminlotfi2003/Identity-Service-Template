using IdentityService.Domain.Security;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Iam;

public sealed class SecurityBannerAcceptanceConfiguration : IEntityTypeConfiguration<SecurityBannerAcceptance>
{
    public void Configure(EntityTypeBuilder<SecurityBannerAcceptance> b)
    {
        b.ToTable("SecurityBannerAcceptances", ApplicationDbContext.IamSchema);
        b.HasKey(x => x.Id);
        b.Property(x => x.Version).HasMaxLength(32).IsRequired();
        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => new { x.UserId, x.Version }).IsUnique();
    }
}
