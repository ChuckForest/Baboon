using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory
{
    [Serializable]
    public class Exception
    {
        private string _message = String.Empty;
        private string _source = String.Empty;
        private string _stackTrace = String.Empty;
        private Exception _innerException = null;

        public Exception()
        {

        }

        public Exception(System.Exception exception)
        {
            _message = exception.Message;
            _source = exception.Source;
            //_stackTrace = exception.StackTrace;
            //if (exception.InnerException != null)
            //    _innerException = new Exception(exception);
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        public Exception InnerException
        {
            get { return _innerException; }
            set { _innerException = value; }
        }

    }
}
