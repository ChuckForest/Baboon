using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory
{
    public class CriteriaParameter
    {
        private string _columnName = String.Empty;
        private object _constraint = null;

        public CriteriaParameter()
        {

        }

        public CriteriaParameter(string columnName, object constraint)
        {
            _columnName = columnName;
            _constraint = constraint;
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public object Constraint
        {
            get { return _constraint; }
            set { _constraint = value; }
        }

    }
}
