using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderTableFunctionExecutable : IQueryBuilderGetQuery
    {
        /// <summary>
        /// Executes the built function call query against Dapper -> DbConnection.Query&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <returns>A collection of <typeparamref name="T"/></returns>
        IEnumerable<T> ExecuteAndQuery<T>();

        /// <summary>
        /// Executes the built function call query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <returns>A collection of <typeparamref name="T"/></returns>
        Task<IEnumerable<T>> ExecuteAndQueryAsync<T>();
    }
}
