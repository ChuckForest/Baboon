using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.Elements
{
    [Serializable]
    public class ElementBaseData : ElementBase, IBaseData, ICloneable
    {
        private object _data = null;
        private List<Rule> _rules = new List<Rule>();

        public ElementBaseData()
            : base()
        {

        }

        public ElementBaseData(ElementBase elementBase)
            : base(elementBase)
        {

        }

        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public List<Rule> Rules
        {
            get { return _rules; }
            set { _rules = value; }
        }

        public object Clone()
        {
            ElementBaseData elementBaseData = this.MemberwiseClone() as ElementBaseData;
            elementBaseData.Hash = Hash.Clone() as Hashtable;

            return elementBaseData;
        }

        public List<Notification> Validate()
        {
            List<Notification> exceptions = new List<Notification>();

            _rules.ForEach(r =>
            {
                switch (r.RuleType)
                {
                    case Rule.RuleTypeOption.IsRequired:
                        if (_data.ToString().Trim().Equals(String.Empty))
                            exceptions.Add(new Notification()
                            {
                                ElementId = this.ID ?? default(int),
                                Severity = Notification.NoticeType.Error,
                                Message = "Data is required",
                                RuleGuid = r.RuleGuid
                            });
                        break;
                    case Rule.RuleTypeOption.Match:
                        System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(r.Constraint);
                        if (!rx.IsMatch(_data.ToString()))
                        {
                            exceptions.Add(new Notification()
                            {
                                ElementId = this.ID ?? default(int),
                                Severity = Notification.NoticeType.Error,
                                Message = "Data does not match the expression",
                                RuleGuid = r.RuleGuid
                            });
                        }
                        break;
                }
            });

            /*if (MaxLength != null && _data.ToString().Length > MaxLength)
                exceptions.Add(new Notification()
                {
                    ElementId = this.ID ?? default(int),
                    Severity = Notification.NoticeType.Error,
                    Message = "Data is too long"
                });
            if (MinLength != null && _data.ToString().Length < MinLength)
                exceptions.Add(new Notification()
                {
                    ElementId = this.ID ?? default(int),
                    Severity = Notification.NoticeType.Error,
                    Message = "Data is too short"
                });
            if (Match != null)
            {
                System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(Match);
                if (!rx.IsMatch(_data.ToString()))
                {
                    exceptions.Add(new Notification()
                    {
                        ElementId = this.ID ?? default(int),
                        Severity = Notification.NoticeType.Error,
                        Message = "Data does not match the expression"
                    });
                }
            }
            if (IsRequired && _data.ToString().Trim().Equals(String.Empty))
                exceptions.Add(new Notification()
                {
                    ElementId = this.ID ?? default(int),
                    Severity = Notification.NoticeType.Error,
                    Message = "Data is required"
                });*/

            return exceptions;
        }

        public string QualifySqlArgument()
        {
            string ret = _data.ToString();

            switch (SqlDataType)
            {
                case System.Data.SqlDbType.Char:
                    ret = String.Format("'{0}'", ret);
                    break;
            }

            return ret;
        }

    }
}
