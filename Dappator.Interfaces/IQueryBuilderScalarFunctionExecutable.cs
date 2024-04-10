using System.Threading.Tasks;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderScalarFunctionExecutable : IQueryBuilderGetQuery
    {
        /// <summary>
        /// Executes the built function call query against Dapper -> DbConnection.ExecuteScalar&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of result to return. It must be a primitive type.</typeparam>
        /// <returns>The <typeparamref name="T"/> value</returns>
        T ExecuteAndReadScalar<T>();

        /// <summary>
        /// Executes the built function call query asynchronously against Dapper -> DbConnection.ExecuteScalarAsync&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of result to return. It must be a primitive type.</typeparam>
        /// <returns>The <typeparamref name="T"/> value</returns>
        Task<T> ExecuteAndReadScalarAsync<T>();
    }
}
