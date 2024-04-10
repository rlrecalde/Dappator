using MySqlConnector;
using System.Data.Common;

namespace Dappator.Test.Providers
{
    public class MySqlProvider : IProvider
    {
        public DbConnection GetOpenConnection(string connectionString)
        {
            var FbConnection = new MySqlConnection(connectionString);
            FbConnection.Open();

            return FbConnection;
        }

        public DbCommand GetCommand(string query, DbConnection dbConnection)
        {
            var FbCommand = new MySqlCommand(query, (MySqlConnection)dbConnection);

            return FbCommand;
        }

        public string GetInsertUserQuery(string nick, string password)
        {
            string query = $"" +
                $"INSERT INTO `User` (`Nick`, `Password`) VALUES ('{nick}', '{password}'); " +
                $"SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            return query;
        }

        public string GetInsertUserValueQuery(int userId, double value)
        {
            string query = $"" +
                $"INSERT INTO `UserValue` (`UserId`, `Value`) VALUES ({userId}, {value}); " +
                $"SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            return query;
        }

        public string GetCreateUserTableQuery()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS `User` (
                    Id INT AUTO_INCREMENT NOT NULL,
                    Nick VARCHAR(50) NOT NULL,
                    Password VARCHAR(50) NOT NULL,
                    CONSTRAINT PK_User PRIMARY KEY (Id)
                )";

            return query;
        }

        public string GetDeleteUserQuery()
        {
            string query = "DELETE FROM `User`";

            return query;
        }

        public string GetCreateUserValueTableQuery()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS `UserValue` (
                    Id INT AUTO_INCREMENT NOT NULL,
                    UserId INT NOT NULL,
                    Value DECIMAL(10,5) NOT NULL,
                    CONSTRAINT PK_User PRIMARY KEY (Id),
                    CONSTRAINT FK_UserValueUser FOREIGN KEY (UserId) REFERENCES `User`(Id)
                )";

            return query;
        }

        public string GetDeleteUserValueQuery()
        {
            string query = "DELETE FROM `UserValue`";

            return query;
        }

        public string GetUsersQuery()
        {
            string query = "SELECT `Id` AS '`Id`', `Nick` AS '`Nick`', `Password` AS '`Password`' FROM `User`";

            return query;
        }

        public string GetCreateSpInsertUserQuery()
        {
            string query = "" +
                "CREATE PROCEDURE IF NOT EXISTS InsertUser (\n" +
                "   IN nick VARCHAR(50),\n" +
                "   IN password VARCHAR(50)\n" +
                ")\n" +
                "BEGIN\n" +
                "   INSERT INTO `User` (Nick, `Password`) VALUES (nick, password);\n" +
                "END";

            return query;
        }

        public string GetCreateSpInsertUserAndGetIdQuery()
        {
            string query = "" +
                "CREATE PROCEDURE IF NOT EXISTS InsertUserAndGetId (\n" +
                "   IN nick VARCHAR(50),\n" +
                "   IN password VARCHAR(50)\n" +
                ")\n" +
                "BEGIN\n" +
                "   INSERT INTO `User` (Nick, `Password`) VALUES (nick, password);\n" +
                "   SELECT CAST(LAST_INSERT_ID() AS SIGNED);\n" +
                "END";

            return query;
        }

        public string GetCreateSpGetUserByIdQuery()
        {
            string query = "" +
                "CREATE PROCEDURE IF NOT EXISTS GetUserById (\n" +
                "   IN id INT\n" +
                ")\n" +
                "BEGIN\n" +
                "   SELECT * FROM `User` WHERE Id = id LIMIT 1;\n" +
                "END";

            return query;
        }

        public string GetCreateSpGetUsersQuery()
        {
            string query = "" +
                "CREATE PROCEDURE IF NOT EXISTS GetUsers ()\n" +
                "BEGIN\n" +
                "   SELECT * FROM `User`;\n" +
                "END";

            return query;
        }

        public string GetCreateFnGetUsersQuery()
        {
            string query = "" +
                "";

            return query;
        }

        public string GetCreateFnGetUserIdByNickQuery()
        {
            string query = "" +
                "CREATE FUNCTION IF NOT EXISTS FnGetUserIdByNick\n" +
                "(\n" +
                "   p_nick VARCHAR(50)\n" +
                ")\n" +
                "RETURNS INT DETERMINISTIC\n" +
                "BEGIN\n" +
                "   DECLARE userId INT;\n" +
                "   SELECT Id INTO userId FROM `User` WHERE Nick = p_nick LIMIT 1;\n" +
                "   RETURN userId;\n" +
                "END";

            return query;
        }
    }
}
