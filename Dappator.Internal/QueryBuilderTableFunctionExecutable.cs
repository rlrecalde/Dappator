using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dappator.Internal
{
    internal class QueryBuilderTableFunctionExecutable : QueryBuilderExecutableBase, Interfaces.IQueryBuilderTableFunctionExecutable
    {
        public QueryBuilderTableFunctionExecutable(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
        }

        public IEnumerable<T> ExecuteAndQuery<T>()
        {
            return base.BasicExecuteAndQuery<T>();
        }

        public async Task<IEnumerable<T>> ExecuteAndQueryAsync<T>()
        {
            return await base.BasicExecuteAndQueryAsync<T>();
        }
    }
}
