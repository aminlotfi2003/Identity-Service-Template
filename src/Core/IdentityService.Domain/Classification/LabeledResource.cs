using IdentityService.Domain.Common;

namespace IdentityService.Domain.Classification;

public sealed class LabeledResource : EntityBase<Guid>
{
    public string ResourceType { get; set; } = default!;
    public Guid ResourceId { get; set; }
    public Guid LabelId { get; set; }
    public Guid? TenantId { get; set; }
}
