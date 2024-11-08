using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public abstract class PrizeGroup : BaseEntity
    {
        public int SegmentId { get; set; }

        public int test { get; set; }
    }

    public abstract class PrizeGroup<TPrize> : PrizeGroup
        where TPrize : Prize
    {
        public ICollection<TPrize> Prizes { get; set; }
        public override int Id { get; set; }
    }

    public abstract class Prize : BaseEntity
    {
        
    }

    public class WheelPrize : Prize
    {
        public override int Id { get; set; }
    }

    public class WheelPrizeGroup : PrizeGroup<WheelPrize>
    {
        public int test1 { get; set; }
    }


    public class Generator
    {
        private static List<PrizeGroup> PrizeGroups = [];

        public static void AddGroup(PrizeGroup group) //where T : TPrizeGroup
        {
            PrizeGroups.Add(group);
        }

        public static PrizeGroup GetGroup()
        {
            return PrizeGroups.First();
        }

        public static int GetPrize<TPrizeGroup>() where TPrizeGroup : PrizeGroup
        {
            return PrizeGroups.First(x => x.GetType() == typeof(TPrizeGroup)).test;
        }

        internal void AddGroup(WheelPrizeGroup p)
        {
            throw new NotImplementedException();
        }
    }
}
