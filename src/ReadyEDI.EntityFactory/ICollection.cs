using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory
{
    public interface ICollection<T> where T : IEntity
    {
        List<T> ToList();

        string ConnectionString { get; set; }
        List<EntityException> Exceptions { get; set; }
    }
}
