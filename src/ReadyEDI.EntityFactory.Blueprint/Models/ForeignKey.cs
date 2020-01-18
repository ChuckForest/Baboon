using System;

namespace ReadyEDI.EntityFactory.Blueprint.Models
{
    public class ForeignKey
    {
        private string _parentTableName = String.Empty;
        private string _foreignTableName = String.Empty;
        private string _parentColumnName = String.Empty;
        private string _foreignColumnName = String.Empty;
        private string _tableName = String.Empty;
        private string _bluePrintName = String.Empty;
        private string _foreignBluePrintName = String.Empty;

        public ForeignKey()
        {

        }

        public string ParentTableName
        {
            get { return _parentTableName; }
            set { _parentTableName = value; }
        }

        public string ForeignTableName
        {
            get { return _foreignTableName; }
            set { _foreignTableName = value; }
        }

        public string ParentColumnName
        {
            get { return _parentColumnName; }
            set { _parentColumnName = value; }
        }

        public string ForeignColumnName
        {
            get { return _foreignColumnName; }
            set { _foreignColumnName = value; }
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public string BluePrintName
        {
            get { return _bluePrintName; }
            set { _bluePrintName = value; }
        }

        public string ForeignBluePrintName
        {
            get { return _foreignBluePrintName; }
            set { _foreignBluePrintName = value; }
        }

    }
}
