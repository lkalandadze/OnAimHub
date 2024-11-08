using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing
{
    public interface IGloballyVisibleClassGeneratorParams
    {
        public string Namespace { get; set; }
        public bool CopyBaseClasses { get; set; }
    }
}