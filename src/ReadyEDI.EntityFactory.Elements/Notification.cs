using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Elements
{
    [Serializable]
    public class Notification
    {
        public enum NoticeType
        {
            None,
            Warning,
            Error,
            Exception
        }

        private int _elementId = 0;
        private NoticeType _severity = NoticeType.None;
        private string _message = String.Empty;
        private Guid _ruleGuid = Guid.Empty;

        public Notification()
        {

        }

        public int ElementId
        {
            get { return _elementId; }
            set { _elementId = value; }
        }

        public NoticeType Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public Guid RuleGuid
        {
            get { return _ruleGuid; }
            set { _ruleGuid = value; }
        }

        public override string ToString()
        {
            return String.Format("Notification: ElementId {0}, Severity {1}, Message {2}", ElementId, Severity, Message);
        }

    }
}
