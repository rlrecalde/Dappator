using System.Data.Common;

namespace Dappator.Test.Providers
{
    public class SqliteProvider : IProvider
    {
        public DbConnection GetOpenConnection(string connectionString)
        {
            throw new NotImplementedException();
        }

        public DbCommand GetCommand(string query, DbConnection dbConnection)
        {
            var FbCommand = dbConnection.CreateCommand();
            FbCommand.CommandText = query;

            return FbCommand;
        }

        public string GetInsertUserQuery(string nick, string password)
        {
            string query = $"" +
                $"INSERT INTO [User] ([Nick], [Password]) VALUES ('{nick}', '{password}'); " +
                $"SELECT CAST(last_insert_rowid() AS BIGINT)";

            return query;
        }

        public string GetInsertUserValueQuery(int userId, double value)
        {
            string query = $"" +
                $"INSERT INTO [UserValue] ([UserId], [Value]) VALUES ({userId}, {value}); " +
                $"SELECT CAST(last_insert_rowid() AS BIGINT)";

            return query;
        }

        public string GetCreateUserTableQuery()
        {
            string query = "" +
                "CREATE TABLE User (" +
                "   Id INTEGER PRIMARY KEY, " +
                "   Nick TEXT NOT NULL, " +
                "   Password TEXT NOT NULL" +
                ");";

            return query;
        }

        public string GetDeleteUserQuery()
        {
            string query = "DELETE FROM User";

            return query;
        }

        public string GetCreateUserValueTableQuery()
        {
            string query = "" +
                "CREATE TABLE UserValue (" +
                "   Id INTEGER PRIMARY KEY, " +
                "   UserId INTEGER NOT NULL, " +
                "   [Value] REAL NOT NULL, " +
                "   FOREIGN KEY(UserId) REFERENCES [User](Id)" +
                ");";

            return query;
        }

        public string GetDeleteUserValueQuery()
        {
            string query = "DELETE FROM UserValue";

            return query;
        }

        public string GetUsersQuery()
        {
            string query = "SELECT [Id] AS '[Id]', [Nick] AS '[Nick]', [Password] AS '[Password]' FROM User";

            return query;
        }

        public string GetCreateSpInsertUserQuery()
        {
            return string.Empty;
        }

        public string GetCreateSpInsertUserAndGetIdQuery()
        {
            return string.Empty;
        }

        public string GetCreateSpGetUserByIdQuery()
        {
            return string.Empty;
        }

        public string GetCreateSpGetUsersQuery()
        {
            return string.Empty;
        }

        public string GetCreateFnGetUsersQuery()
        {
            return string.Empty;
        }

        public string GetCreateFnGetUserIdByNickQuery()
        {
            return string.Empty;
        }
    }
}
