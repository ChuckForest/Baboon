using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory
{
    public class TransactionStep
    {
	    private IEntity _entity = null;
        private Collection<IEntity> _collection = null;
	    private StepType _stepType;
	    private string _memberName = String.Empty;
	    private	object _propertyValue = null;
	    private IEntity _propertyValueEntity = null;
        private string _propertyValueElementName = String.Empty;
	    private Dictionary<string, object> _methodArguments = new Dictionary<string, object>();

        public TransactionStep()
        {

        }

        public IEntity Entity
        {
            get { return _entity;  }
            set { _entity = value; }
        }

        public Collection<IEntity> Collection
        {
            get { return _collection; }
            set { _collection = value; }
        }

        public StepType Type
        {
            get { return _stepType; }
            set { _stepType = value; }
        }

        public string MemberName
        {
            get { return _memberName; }
            set { _memberName = value; }
        }

        public object PropertyValue
        {
            get { return _propertyValue; }
            set { _propertyValue = value; }
        }

        public IEntity PropertyValueEntity
        {
            get { return _propertyValueEntity; }
            set { _propertyValueEntity = value; }
        }

        public string PropertyValueElementName
        {
            get { return _propertyValueElementName; }
            set { _propertyValueElementName = value; }
        }

        public Dictionary<string, object> MethodArguments
        {
            get { return _methodArguments; }
            set { _methodArguments = value; }
        }

    }

    public enum StepType
    {
        PropertyChange,
        MethodCall
    }
}
