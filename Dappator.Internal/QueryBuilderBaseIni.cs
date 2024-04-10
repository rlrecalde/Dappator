namespace Dappator.Internal
{
    internal class QueryBuilderBaseIni : QueryBuilderBase
    {
        public QueryBuilderBaseIni(QueryBuilderBase queryBuilderBase)
        {
            base._query = queryBuilderBase.StringQuery;
            base._parameterCounter = queryBuilderBase.ParameterCounter;
            base._values = queryBuilderBase.Values;
            base._types = queryBuilderBase.Types;
            base._dbType = queryBuilderBase.DbConnectionType;
            base._entityInfos = queryBuilderBase.EntityInfos;
            base._dbConnection = queryBuilderBase.DbConnection;
            base._dbTransaction = queryBuilderBase.DbTransaction;
            base._commandTimeout = queryBuilderBase.CommandTimeout;
            base._executeInTransaction = queryBuilderBase.ExecuteInTransaction;
            base._buffered = queryBuilderBase.Buffered;
            base._transactionIsolationLevel = queryBuilderBase.TransactionIsolationLevel;
        }
    }
}
