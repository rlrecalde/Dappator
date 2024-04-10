using Oracle.ManagedDataAccess.Client;

namespace Dappator.Oracle
{
    public class DataContext : DataContextBase
    {
        public DataContext(string connectionString)
        {
            base._connectionString = connectionString;
            base._dbConnection = new OracleConnection(base._connectionString);

            base._queryBuilderMain = new Internal.QueryBuilderMainOracle(base._dbConnection);
        }
    }
}
