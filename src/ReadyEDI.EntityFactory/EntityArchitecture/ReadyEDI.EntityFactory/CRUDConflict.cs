using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory
{
    [Serializable]
    public class CRUDConflict
    {
        private List<int> _fields = new List<int>();
        private List<string> _messages = new List<string>();
        private List<CRUDOption> _options = new List<CRUDOption>();

        public CRUDConflict()
        {

        }

        public List<int> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public List<string> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        public List<CRUDOption> Options
        {
            get { return _options; }
            set { _options = value; }
        }

        public void Clear()
        {
            Fields.Clear();
            Messages.Clear();
            Options.Clear();
        }

    }
}
