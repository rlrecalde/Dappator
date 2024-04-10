using System.Threading.Tasks;

namespace Dappator.Internal
{
    internal class QueryBuilderExecuteAndQuery : QueryBuilderQuery, Interfaces.IQueryBuilderExecute, Interfaces.IQueryBuilderQuery
    {
        public QueryBuilderExecuteAndQuery(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
        }

        public int Execute()
        {
            return base.BasicExecute();
        }

        public async Task<int> ExecuteAsync()
        {
            return await base.BasicExecuteAsync();
        }
    }
}
