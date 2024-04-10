using System.Threading.Tasks;

namespace Dappator.Internal
{
    internal class QueryBuilderSpExecutable : QueryBuilderExecuteScalar, Interfaces.IQueryBuilderSpExecutable
    {
        public QueryBuilderSpExecutable(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
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

        public T ExecuteAndRead<T>()
        {
            return base.BasicExecuteAndRead<T>();
        }

        public async Task<T> ExecuteAndReadAsync<T>()
        {
            return await base.BasicExecuteAndReadAsync<T>();
        }
    }
}
