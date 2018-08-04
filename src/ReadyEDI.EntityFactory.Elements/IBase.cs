using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.Elements
{
    public interface IBase
    {
        int? ID { get; set; }
        string Name { get; set; }
        long? Bitwise { get; set; }
        bool UseForMatching { get; set; }
        //int? MaxLength { get; set; }
        //int? MinLength { get; set; }
        ElementBase.ControlType Control { get; set; }
        long? ParameterIn { get; set; }
        long? ParameterOut { get; set; }
    }
}
