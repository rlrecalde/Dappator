using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dappator.Internal
{
    internal class QueryBuilderExecutable : QueryBuilderExecuteAndQuery, Interfaces.IQueryBuilderExecutable
    {
        public QueryBuilderExecutable(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
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

        public T ExecuteAndRead<T>()
        {
            return base.BasicExecuteAndRead<T>();
        }

        public async Task<T> ExecuteAndReadAsync<T>()
        {
            return await base.BasicExecuteAndReadAsync<T>();
        }

        public IEnumerable<T> ExecuteAndQuery<T>()
        {
            return base.BasicExecuteAndQuery<T>();
        }

        public async Task<IEnumerable<T>> ExecuteAndQueryAsync<T>()
        {
            return await base.BasicExecuteAndQueryAsync<T>();
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
