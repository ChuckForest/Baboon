using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory
{
    [Serializable]
    public class Action
    {
        private int _id = 0;
        private string _name = String.Empty;
        private long _bitwise = 0;

        public Action()
        {

        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public long Bitwise
        {
            get { return _bitwise; }
            set { _bitwise = value; }
        }

    }
}
