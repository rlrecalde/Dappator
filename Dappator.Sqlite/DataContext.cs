#if NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif
using System.Threading.Tasks;

namespace Dappator.Sqlite
{
    public class DataContext : DataContextBase
    {
        private bool _preventClosing;

        public DataContext(string connectionString, bool preventClosing = false)
        {
            this._preventClosing = preventClosing;
            base._connectionString = connectionString;
#if NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
            base._dbConnection = new SqliteConnection(base._connectionString);
#else
            base._dbConnection = new SQLiteConnection(base._connectionString);
#endif

            base._queryBuilderMain = new Internal.QueryBuilderMainSqlite(base._dbConnection);
        }

#if NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
        public DataContext(SqliteConnection sqliteConnection, bool preventClosing = false)
        {
            this._preventClosing = preventClosing;
            base._connectionString = sqliteConnection.ConnectionString;
            base._dbConnection = sqliteConnection;

            base._queryBuilderMain = new Internal.QueryBuilderMainSqlite(base._dbConnection);
        }
#else
        public DataContext(SQLiteConnection sqliteConnection, bool preventClosing = false)
        {
            this._preventClosing = preventClosing;
            base._connectionString = sqliteConnection.ConnectionString;
            base._dbConnection = sqliteConnection;

            base._queryBuilderMain = new Internal.QueryBuilderMainSqlite(base._dbConnection);
        }
#endif

        public override void Close()
        {
            if (this._preventClosing || base._dbConnection == null)
                return;

            base.BasicClose();
        }

        public override async Task CloseAsync()
        {
            if (this._preventClosing || base._dbConnection == null)
                return;

            await base.BasicCloseAsync();
        }

        public override void Dispose()
        {
            if (this._preventClosing || base._dbConnection == null)
                return;

            base.BasicClose();
            base._dbConnection.Dispose();
        }
    }
}
