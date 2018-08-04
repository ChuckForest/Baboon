using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.Elements
{
    [Serializable]
    public class BaseHash
    {
        private Hashtable _hash = new Hashtable();

        public BaseHash()
        {

        }

        public Hashtable Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        protected void InitializeProperties(Type enumerator)
        {
            int i = 0;
            Enum.GetNames(enumerator).ToList().ForEach(e => _hash.Add(i++, null));
        }

    }
}
