using Microsoft.Data.SqlClient;

namespace Dappator.Sql
{
    public class DataContext : DataContextBase
    {
        public DataContext(string connectionString)
        {
            base._connectionString = connectionString;
            base._dbConnection = new SqlConnection(base._connectionString);

            base._queryBuilderMain = new Internal.QueryBuilderMainSql(base._dbConnection);
        }
    }
}
