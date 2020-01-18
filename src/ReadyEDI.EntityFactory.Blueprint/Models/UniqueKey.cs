using System;
using System.Collections.Generic;

namespace ReadyEDI.EntityFactory.Blueprint.Models
{
    public class UniqueKey
    {
        private string _bluePrintName = String.Empty;
        private string _tableName = String.Empty;
        private List<string> _columnNames = new List<string>();

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        public List<string> ColumnNames
        {
            get { return _columnNames; }
            set { _columnNames = value; }
        }

        public string BluePrintName
        {
            get { return _bluePrintName; }
            set { _bluePrintName = value; }
        }
    }
}
