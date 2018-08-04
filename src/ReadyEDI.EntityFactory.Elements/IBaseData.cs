using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.Elements
{
    public interface IBaseData : IBase
    {
        object Data { get; set; }

        object Clone();
        List<Notification> Validate();
    }
}

