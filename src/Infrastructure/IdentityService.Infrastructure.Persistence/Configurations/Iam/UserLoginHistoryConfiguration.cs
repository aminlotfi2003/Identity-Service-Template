using IdentityService.Domain.Audit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Iam;

public sealed class UserLoginHistoryConfiguration : IEntityTypeConfiguration<UserLoginHistory>
{
    public void Configure(EntityTypeBuilder<UserLoginHistory> b)
    {
        b.ToTable("UserLoginHistory", ApplicationDbContext.IamSchema);
        b.HasKey(x => x.Id);
        b.Property(x => x.IpAddress).HasMaxLength(64);
        b.Property(x => x.Host).HasMaxLength(255);
        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => new { x.UserId, x.OccurredAt });
    }
}
