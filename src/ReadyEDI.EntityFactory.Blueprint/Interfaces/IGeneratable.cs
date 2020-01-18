using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.Blueprint.Interfaces
{
    public interface IGeneratable : IBase
    {
        Guid BluePrintGuid { get; set; }
        string Version { get; set; }
    }
}
