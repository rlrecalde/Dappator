using System;

namespace Dappator.Internal
{
    internal class DappatorException : Exception, Interfaces.IException
    {
        private string _message;
        private string _stackTrace;
        private string _query;

        public override string Message { get { return this._message; } }

        public override string StackTrace { get { return this._stackTrace; } }

        public string Query { get { return this._query; } }

        public DappatorException(Exception exception, string query)
        {
            this._message = exception.Message;
            this._stackTrace = exception.StackTrace;
            this._query = query;
        }
    }
}
