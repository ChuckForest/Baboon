using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Elements
{
    public class ActionParameter
    {
        public enum ActionParameterType
        {
            In,
            Out,
            InOut
        }

        private ActionParameterType _type = ActionParameterType.In;
        private List<int> _depth = new List<int>();
        private int _customAction = 0;

        public ActionParameter()
        {

        }

        public ActionParameterType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public List<int> Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        public int CustomerAction
        {
            get { return _customAction; }
            set { _customAction = value; }
        }

    }
}
