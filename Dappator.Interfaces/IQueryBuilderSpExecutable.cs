using System.Threading.Tasks;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderSpExecutable : IQueryBuilderExecute
    {
        /// <summary>
        /// Executes the built stored procedure call query against Dapper -> DbConnection.Query&lt;<typeparamref name="T"/>&gt;() method or Dapper -> DbConnection.QueryFirstOrDefault&lt;<typeparamref name="T"/>&gt;() method, whether <typeparamref name="T"/> is a collection or not
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <returns>The <typeparamref name="T"/> object</returns>
        T ExecuteAndRead<T>();

        /// <summary>
        /// Executes the built stored procedure call query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="T"/>&gt;() method or Dapper -> DbConnection.QueryFirstOrDefaultAsync&lt;<typeparamref name="T"/>&gt;() method, whether <typeparamref name="T"/> is a collection or not
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <returns>The <typeparamref name="T"/> object</returns>
        Task<T> ExecuteAndReadAsync<T>();
    }
}
