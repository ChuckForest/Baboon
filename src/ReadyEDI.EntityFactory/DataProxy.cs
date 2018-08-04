using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory
{
    [Serializable]
    public class DataProxy
    {
        private int _elementId = 0;
        private object _data = null;

        public DataProxy()
        {

        }

        public DataProxy(int elementId, object data)
        {
            _elementId = elementId;
            _data = data;
        }

        public int ElementId
        {
            get { return _elementId; }
            set { _elementId = value; }
        }

        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

    }
}
