using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory
{
    public class CriteriaParameter
    {
        private string _columnName = String.Empty;
        private string _constraint = String.Empty;

        public CriteriaParameter()
        {

        }

        public CriteriaParameter(string columnName, string constraint)
        {
            _columnName = columnName;
            _constraint = constraint;
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public string Constraint
        {
            get { return _constraint; }
            set { _constraint = value; }
        }

    }
}
