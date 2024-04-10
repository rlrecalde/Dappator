using System.Threading.Tasks;

namespace Dappator.Internal
{
    internal class QueryBuilderExecuteScalar : QueryBuilderExecutableBase, Interfaces.IQueryBuilderExecuteScalar
    {
        public QueryBuilderExecuteScalar(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
        }

        public long ExecuteScalar()
        {
            return base.BasicExecuteScalar();
        }

        public async Task<long> ExecuteScalarAsync()
        {
            return await base.BasicExecuteScalarAsync();
        }
    }
}
