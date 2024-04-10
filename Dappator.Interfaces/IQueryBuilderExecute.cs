using System.Threading.Tasks;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderExecute : IQueryBuilderGetQuery
    {
        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Execute() method
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int Execute();

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.ExecuteAsync() method
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        Task<int> ExecuteAsync();
    }
}
