using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory
{
    [Serializable]
    public class DataProxyCollection : List<DataProxy>
    {
        private List<Exception> _exceptions = new List<Exception>();

        public DataProxyCollection()
        {

        }

        public List<Exception> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }

    }
}
