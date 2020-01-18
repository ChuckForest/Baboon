using System;

namespace ReadyEDI.EntityFactory.Blueprint.Models
{
    public class Constraint
    {
        private string _parentTableName = String.Empty;
        private string _fieldName = String.Empty;
        private string _defaultValue = String.Empty;

        public Constraint()
        {

        }

        public string ParentTableName
        {
            get { return _parentTableName; }
            set { _parentTableName = value; }
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
    }
}
