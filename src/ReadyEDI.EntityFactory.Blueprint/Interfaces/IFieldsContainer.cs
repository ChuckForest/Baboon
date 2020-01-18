using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.Blueprint.Interfaces
{
    public interface IFieldsContainer : IGeneratable
    {
        List<Field> Fields { get; set; }
        Extension Extended { get; set; }
        Guid FieldContainerGuid { get; set; }
        string SchemaName { get; set; }
    }
}
