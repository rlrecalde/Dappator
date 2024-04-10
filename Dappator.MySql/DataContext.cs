using MySqlConnector;

namespace Dappator.MySql
{
    public class DataContext : DataContextBase
    {
        public DataContext(string connectionString)
        {
            base._connectionString = connectionString;
            base._dbConnection = new MySqlConnection(base._connectionString);

            base._queryBuilderMain = new Internal.QueryBuilderMainMySql(base._dbConnection);
        }
    }
}
