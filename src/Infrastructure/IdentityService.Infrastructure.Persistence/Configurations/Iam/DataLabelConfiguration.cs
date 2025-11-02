using IdentityService.Domain.Classification;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Iam;

public sealed class DataLabelConfiguration : IEntityTypeConfiguration<DataLabel>
{
    public void Configure(EntityTypeBuilder<DataLabel> b)
    {
        b.ToTable("DataLabels", ApplicationDbContext.IamSchema);
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(128).IsRequired();
        b.Property(x => x.Description).HasMaxLength(512);
        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => x.Name).IsUnique();
    }
}
