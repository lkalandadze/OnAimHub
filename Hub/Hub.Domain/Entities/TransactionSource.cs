using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class TransactionSource : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }
}