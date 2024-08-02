using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities;

public partial class Base
{
    public class Configuration : BaseEntity
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public int GameVersionId { get; set; }
        public GameVersion GameVersion { get; set; }

        public ICollection<PrizeGroup> PrizeGroups { get; set; }
        public ICollection<Prize> Prizes { get; set; }
    }
}