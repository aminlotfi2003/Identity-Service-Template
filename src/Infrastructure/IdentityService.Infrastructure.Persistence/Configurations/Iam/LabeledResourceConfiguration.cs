using IdentityService.Domain.Classification;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Configurations.Iam;

public sealed class LabeledResourceConfiguration : IEntityTypeConfiguration<LabeledResource>
{
    public void Configure(EntityTypeBuilder<LabeledResource> b)
    {
        b.ToTable("LabeledResources", ApplicationDbContext.IamSchema);
        b.HasKey(x => x.Id);
        b.Property(x => x.ResourceType).HasMaxLength(128).IsRequired();
        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => new { x.ResourceType, x.ResourceId, x.LabelId }).IsUnique();
        b.HasIndex(x => x.TenantId);

        b.HasOne<DataLabel>()
         .WithMany()
         .HasForeignKey(x => x.LabelId)
         .OnDelete(DeleteBehavior.Restrict)
         .HasConstraintName("FK_LabeledResource_DataLabel");
    }
}
