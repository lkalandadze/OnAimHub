using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing;

class GloballyVisibleClassGeneratorParams : IGloballyVisibleClassGeneratorParams
{
    public string Namespace { get; set; }
    public bool CopyBaseClasses { get; set; }
}