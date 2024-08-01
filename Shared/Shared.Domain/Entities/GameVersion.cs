using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities;

public partial class Base
{
    public class GameVersion : BaseEntity
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Configuration> Configurations { get; set; }
    }
}