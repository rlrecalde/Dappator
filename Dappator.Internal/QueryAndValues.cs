namespace Dappator.Internal
{
    internal class QueryAndValues : Interfaces.IQueryAndValues
    {
        private string _query;
        private object[] _values;

        public string Query { get { return this._query; } }

        public object[] Values { get { return this._values; } }

        public QueryAndValues(string query, object[] values)
        {
            this._query = query;
            this._values = values;
        }
    }
}
