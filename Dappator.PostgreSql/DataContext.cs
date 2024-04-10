using Npgsql;

namespace Dappator.PostgreSql
{
    public class DataContext : DataContextBase
    {
        public DataContext(string connectionString)
        {
            base._connectionString = connectionString;
            base._dbConnection = new NpgsqlConnection(base._connectionString);

            base._queryBuilderMain = new Internal.QueryBuilderMainNpgsql(base._dbConnection);
        }
    }
}
