using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;

namespace TempGloballyVisibleClassSharing
{
    [GloballyVisible]
    public class Class1
    {
        public int Visible { get; set; }

        [GloballyHidden]
        public int Invisible { get; set; }
    }


    [GloballyVisible]
    public class Class2
    {
        public int Visible { get; set; }

        [GloballyHidden]
        public int Invisible { get; set; }
    }
}