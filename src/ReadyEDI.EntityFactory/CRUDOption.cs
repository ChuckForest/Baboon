using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory
{
    public class CRUDOption
    {
        public enum CRUDOptionType
        {
            None,
            SaveAll,
            SaveNonConflicts,
            Cancel
        }

        private string _message = String.Empty;
        private CRUDOptionType _optionType = CRUDOptionType.None;

        public CRUDOption()
        {

        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public CRUDOptionType OptionType
        {
            get { return _optionType; }
            set { _optionType = value; }
        }

    }
}
