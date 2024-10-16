using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain.Entities;

public abstract class BaseEntity<T> : BaseEntity
{
    private T _id;

    [Column(Order = 1)]
    public new T Id
    {
        get => _id;
        set
        {
            _id = value;
            base.Id = value;
        }
    }
}

public abstract class BaseEntity
{
    public dynamic Id { get; internal set; } = null!;
}