using System.Threading.Tasks;

namespace Dappator.Internal
{
    internal class QueryBuilderScalarFunctionExecutable : QueryBuilderExecutableBase, Interfaces.IQueryBuilderScalarFunctionExecutable
    {
        public QueryBuilderScalarFunctionExecutable(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
        }

        public T ExecuteAndReadScalar<T>()
        {
            return base.BasicExecuteAndReadScalar<T>();
        }

        public async Task<T> ExecuteAndReadScalarAsync<T>()
        {
            return await base.BasicExecuteAndReadScalarAsync<T>();
        }
    }
}
