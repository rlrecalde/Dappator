using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Dappator.Test.Providers
{
    public class SqlProvider : IProvider
    {
        public DbConnection GetOpenConnection(string connectionString)
        {
            var SqlConnection = new SqlConnection(connectionString);
            SqlConnection.Open();

            return SqlConnection;
        }

        public DbCommand GetCommand(string query, DbConnection dbConnection)
        {
            var SqlCommand = new SqlCommand(query, (SqlConnection)dbConnection);

            return SqlCommand;
        }

        public string GetInsertUserQuery(string nick, string password)
        {
            string query = $"" +
                $"INSERT INTO [User] ([Nick], [Password]) VALUES ('{nick}', '{password}'); " +
                $"SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            return query;
        }

        public string GetInsertUserValueQuery(int userId, double value)
        {
            string query = $"" +
                $"INSERT INTO [UserValue] ([UserId], [Value]) VALUES ({userId}, {value}); " +
                $"SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            return query;
        }

        public string GetCreateUserTableQuery()
        {
            string query = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'User')
                    CREATE TABLE [User] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [Nick] [VARCHAR](50) NOT NULL,
                        [Password] [VARCHAR](50) NOT NULL,
                        CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED
                        ( [Id] ASC )
                    )";

            return query;
        }

        public string GetDeleteUserQuery()
        {
            string query = "DELETE FROM [User]";

            return query;
        }

        public string GetCreateUserValueTableQuery()
        {
            string query = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'UserValue')
                    CREATE TABLE [UserValue] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [UserId] [INT] NOT NULL,
                        [Value] [DECIMAL](10,5) NOT NULL,
                        CONSTRAINT [PK_UserValue] PRIMARY KEY CLUSTERED ([Id] ASC),
                        CONSTRAINT [FK_UserValueUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
                    )";

            return query;
        }

        public string GetDeleteUserValueQuery()
        {
            string query = "DELETE FROM [UserValue]";

            return query;
        }

        public string GetUsersQuery()
        {
            string query = "SELECT [Id] AS '[Id]', [Nick] AS '[Nick]', [Password] AS '[Password]' FROM [User]";

            return query;
        }

        public string GetCreateSpInsertUserQuery()
        {
            string query = "" +
                "CREATE OR ALTER PROCEDURE InsertUser (\n" +
                "   @Nick VARCHAR(50),\n" +
                "   @Password VARCHAR(50)\n" +
                ")\n" +
                "AS\n" +
                "BEGIN\n" +
                "   INSERT INTO [User] ([Nick], [Password]) VALUES (@Nick, @Password)\n" +
                "END;";

            return query;
        }

        public string GetCreateSpInsertUserAndGetIdQuery()
        {
            string query = "" +
                "CREATE OR ALTER PROCEDURE InsertUserAndGetId (\n" +
                "   @Nick VARCHAR(50),\n" +
                "   @Password VARCHAR(50)\n" +
                ")\n" +
                "AS\n" +
                "BEGIN\n" +
                "   INSERT INTO [User] ([Nick], [Password]) VALUES (@Nick, @Password)\n" +
                "   SELECT CAST(SCOPE_IDENTITY() AS BIGINT)\n" +
                "END;";

            return query;
        }

        public string GetCreateSpGetUserByIdQuery()
        {
            string query = "" +
                "CREATE OR ALTER PROCEDURE GetUserById (\n" +
                "   @Id INT\n" +
                ")\n" +
                "AS\n" +
                "BEGIN\n" +
                "   SELECT TOP 1 * FROM [User] WHERE Id = @Id\n" +
                "END;";

            return query;
        }

        public string GetCreateSpGetUsersQuery()
        {
            string query = "" +
                "CREATE OR ALTER PROCEDURE GetUsers\n" +
                "AS\n" +
                "BEGIN\n" +
                "   SELECT * FROM [User]\n" +
                "END;";

            return query;
        }

        public string GetCreateFnGetUsersQuery()
        {
            string query = "" +
                "CREATE OR ALTER FUNCTION FnGetUsers ()\n" +
                "RETURNS\n" +
                "@TableToReturn TABLE\n" +
                "(\n" +
                "   [Id] [INT] NOT NULL,\n" +
                "   [Nick] [VARCHAR](50) NOT NULL,\n" +
                "   [Password] [VARCHAR](50) NOT NULL\n" +
                ")\n" +
                "AS\n" +
                "BEGIN\n" +
                "   INSERT INTO @TableToReturn SELECT * FROM [User]\n" +
                "   RETURN\n" +
                "END;";

            return query;
        }

        public string GetCreateFnGetUserIdByNickQuery()
        {
            string query = "" +
                "CREATE OR ALTER FUNCTION FnGetUserIdByNick\n" +
                "(\n" +
                "   @Nick VARCHAR(50)\n" +
                ")\n" +
                "RETURNS INT\n" +
                "AS\n" +
                "BEGIN\n" +
                "   DECLARE @id INT\n" +
                "   SELECT TOP 1 @id = [Id] FROM [User] WHERE [Nick] = @Nick\n" +
                "   RETURN @id\n" +
                "END;";

            return query;
        }
    }
}
