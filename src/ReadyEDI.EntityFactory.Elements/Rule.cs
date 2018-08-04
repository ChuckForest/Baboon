using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Elements
{
    public class Rule
    {
        public enum RuleTypeOption
        {
            None,
            IsRequired,
            Match,
            Floor,
            Ceiling,
            MaxLength,
            MinLength,
            MaxDate,
            MinDate
        }

        private Guid _ruleGuid = Guid.Empty;
        private RuleTypeOption _ruleType = RuleTypeOption.None;
        private string _constraint = String.Empty;
        private string _message = String.Empty;

        public Rule()
        {

        }

        public Guid RuleGuid
        {
            get { return _ruleGuid; }
            set { _ruleGuid = value; }
        }

        public RuleTypeOption RuleType
        {
            get { return _ruleType; }
            set { _ruleType = value; }
        }

        public string Constraint
        {
            get { return _constraint; }
            set { _constraint = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

    }
}
