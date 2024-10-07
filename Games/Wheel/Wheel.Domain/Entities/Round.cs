using GameLib.Domain.Entities;
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace Wheel.Domain.Entities;

public class Round : BaseEntity<int>
{
    public Round()
    {
    }

    public string Name { get; set; }
}