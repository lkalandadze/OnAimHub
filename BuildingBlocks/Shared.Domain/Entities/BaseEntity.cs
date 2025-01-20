using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain.Entities;

public abstract class BaseEntity<T> : BaseEntity
{
    private T _id;

    [IgnoreIncludeAll]
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
    [IgnoreIncludeAll]
    public dynamic Id { get; internal set; } = null!;
}