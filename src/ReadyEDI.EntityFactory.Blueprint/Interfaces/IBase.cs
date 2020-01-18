using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.Blueprint.Interfaces
{
    public interface IBase
    {
        string Comment { get; set; }
        string InternalNote { get; set; }
        Guid BaseGuid { get; set; }
        bool DirtyFlag { get; set; }
        int Sequence { get; set; }
        string Name { get; set; }
    }
}
