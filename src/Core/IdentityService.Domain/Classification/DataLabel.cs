using IdentityService.Domain.Common;

namespace IdentityService.Domain.Classification;

public sealed class DataLabel : EntityBase<Guid>
{
    public string Name { get; set; } = default!;
    public ConfidentialityLevel Confidentiality { get; set; } = ConfidentialityLevel.Internal;
    public IntegrityLevel Integrity { get; set; } = IntegrityLevel.Medium;
    public string? Description { get; set; }
}
