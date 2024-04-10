using System.Threading.Tasks;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderExecuteScalar : IQueryBuilderGetQuery
    {
        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.ExecuteScalar() method
        /// </summary>
        /// <returns>The Id of the last inserted row</returns>
        long ExecuteScalar();

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.ExecuteScalarAsync() method
        /// </summary>
        /// <returns>The Id of the last inserted row</returns>
        Task<long> ExecuteScalarAsync();
    }
}
