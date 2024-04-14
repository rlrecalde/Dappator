using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Dappator.MySql.Test
{
    public class TestBase
    {
        protected string _connectionString;

        protected void CreateTables()
        {
            var dbConnection = this.GetOpenConnection();

            string createUser = @"
                CREATE TABLE IF NOT EXISTS `User` (
                    Id INT AUTO_INCREMENT NOT NULL,
                    Nick VARCHAR(50) NOT NULL,
                    Password VARCHAR(50) NOT NULL,
                    CONSTRAINT PK_User PRIMARY KEY (Id)
                )";

            this.ExecuteNonQuery(createUser, dbConnection);

            string createUserValue = @"
                CREATE TABLE IF NOT EXISTS `UserValue` (
                    Id INT AUTO_INCREMENT NOT NULL,
                    UserId INT NOT NULL,
                    Value DECIMAL(10,5) NOT NULL,
                    CONSTRAINT PK_User PRIMARY KEY (Id),
                    CONSTRAINT FK_UserValueUser FOREIGN KEY (UserId) REFERENCES `User`(Id)
                )";

            this.ExecuteNonQuery(createUserValue, dbConnection);

            string createDataType = @"
                CREATE TABLE IF NOT EXISTS `DataType` (
                    Id INT AUTO_INCREMENT NOT NULL,
                    `Byte` TINYINT UNSIGNED NULL,
                    `Sbyte` TINYINT SIGNED NULL,
                    `Short` SMALLINT SIGNED NULL,
                    `Ushort` SMALLINT UNSIGNED NULL,
                    `Int` INT SIGNED NULL,
                    `Uint` INT UNSIGNED NULL,
                    `Long` BIGINT SIGNED NULL,
                    `Ulong` BIGINT UNSIGNED NULL,
                    `Float` FLOAT(40) NULL,
                    `Double` DOUBLE NULL,
                    `Decimal` DECIMAL(29,10) NULL,
                    `Currency` DECIMAL(29,3) NULL,
                    `Bool` BOOL NULL,
                    `String` TEXT NULL,
                    `Char` CHAR(1) NULL,
                    `Guid` VARCHAR(100) NULL,
                    `DateTime` DATETIME NULL,
                    `DateTimeOffset` TIMESTAMP NULL,
                    `TimeSpan` TIME NULL,
                    `Bytes` VARBINARY(100) NULL,
                    CONSTRAINT PK_DataType PRIMARY KEY (Id)
                )";

            this.ExecuteNonQuery(createDataType, dbConnection);

            string createDateAndTime = @"
                CREATE TABLE IF NOT EXISTS DateAndTime (
                    Id INT AUTO_INCREMENT NOT NULL,
                    `DateTime` DATETIME NOT NULL,
                    DateOnly DATE NOT NULL,
                    TimeOnly TIME NOT NULL,
                    CONSTRAINT PK_DateAndTime PRIMARY KEY (Id)
                )";

            this.ExecuteNonQuery(createDateAndTime, dbConnection);

            string deleteUserValue = "DELETE FROM `UserValue`";

            this.ExecuteNonQuery(deleteUserValue, dbConnection);

            string deleteUser = "DELETE FROM `User`";

            this.ExecuteNonQuery(deleteUser, dbConnection);

            string deleteDataType = "DELETE FROM `DataType`";

            this.ExecuteNonQuery(deleteDataType, dbConnection);

            string deleteDateAndTime = "DELETE FROM DateAndTime";

            this.ExecuteNonQuery(deleteDateAndTime, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void InsertUser(Model.User userModel)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO `User` (`Nick`, `Password`) VALUES ('{userModel.Nick}', '{userModel.Password}'); " +
                $"SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            object id = this.ExecuteScalar(query, dbConnection);
            userModel.Id = (int)(long)id;

            this.Dispose(dbConnection);
        }

        protected void InsertUserValue(Model.UserValue userValueModel)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO `UserValue` (`UserId`, `Value`) VALUES ({userValueModel.UserId}, {userValueModel.Value}); " +
                $"SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            object id = this.ExecuteScalar(query, dbConnection);
            userValueModel.Id = (int)(long)id;

            this.Dispose(dbConnection);
        }

        protected void InsertDataType(Model.DataTypeNullable dataType)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO `DataType` (`Float`, `Double`) " +
                $"VALUES ({dataType.Float}, {dataType.Double}); " +
                $"SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            object id = this.ExecuteScalar(query, dbConnection);
            dataType.Id = (int)(long)id;

            this.Dispose(dbConnection);
        }

        protected IEnumerable<Model.User> GetUsers()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "SELECT `Id` AS '`Id`', `Nick` AS '`Nick`', `Password` AS '`Password`' FROM `User`";

            IEnumerable<Model.User> users = this.GetUsers(query, dbConnection);

            return users;
        }

        protected void CreateSpInsertUser()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE PROCEDURE IF NOT EXISTS InsertUser (\n" +
                "   IN nick VARCHAR(50),\n" +
                "   IN password VARCHAR(50)\n" +
                ")\n" +
                "BEGIN\n" +
                "   INSERT INTO `User` (Nick, `Password`) VALUES (nick, password);\n" +
                "END";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpInsertUserAndGetId()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE PROCEDURE IF NOT EXISTS InsertUserAndGetId (\n" +
                "   IN nick VARCHAR(50),\n" +
                "   IN password VARCHAR(50)\n" +
                ")\n" +
                "BEGIN\n" +
                "   INSERT INTO `User` (Nick, `Password`) VALUES (nick, password);\n" +
                "   SELECT CAST(LAST_INSERT_ID() AS SIGNED);\n" +
                "END";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpGetUserById()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE PROCEDURE IF NOT EXISTS GetUserById (\n" +
                "   IN id INT\n" +
                ")\n" +
                "BEGIN\n" +
                "   SELECT * FROM `User` WHERE Id = id LIMIT 1;\n" +
                "END";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpGetUsers()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE PROCEDURE IF NOT EXISTS GetUsers ()\n" +
                "BEGIN\n" +
                "   SELECT * FROM `User`;\n" +
                "END";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateFnGetUsers()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateFnGetUserIdByNick()
        {
            var dbConnection = this.GetOpenConnection();

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

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        #region Private Methods

        private MySqlConnection GetOpenConnection()
        {
            var dbConnection = new MySqlConnection(this._connectionString);
            dbConnection.Open();

            return dbConnection;
        }

        private void ExecuteNonQuery(string query, MySqlConnection sqlConnection)
        {
            var dbCommand = new MySqlCommand(query, sqlConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();
        }

        private object ExecuteScalar(string query, MySqlConnection sqlConnection)
        {
            var dbCommand = new MySqlCommand(query, sqlConnection);
            object id = dbCommand.ExecuteScalar();
            dbCommand.Dispose();

            return id;
        }

        private IEnumerable<Model.User> GetUsers(string query, MySqlConnection sqlConnection)
        {
            string idPropertyName = string.Empty;
            string nickPropertyName = string.Empty;
            string passwordPropertyName = string.Empty;

            var userModelReference = Activator.CreateInstance(typeof(Model.User));
            var properties = userModelReference.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                    idPropertyName = ((System.ComponentModel.DescriptionAttribute[])userModelReference.GetType().GetProperty(property.Name).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false))[0].Description;

                if (property.Name == "Nick")
                    nickPropertyName = ((System.ComponentModel.DescriptionAttribute[])userModelReference.GetType().GetProperty(property.Name).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false))[0].Description;

                if (property.Name == "Password")
                    passwordPropertyName = ((System.ComponentModel.DescriptionAttribute[])userModelReference.GetType().GetProperty(property.Name).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false))[0].Description;
            }

            var userModels = new List<Model.User>();
            var dbCommand = new MySqlCommand(query, sqlConnection);
            DbDataReader dataReader = dbCommand.ExecuteReader();
            while (dataReader.Read())
            {
                var userModel = (Model.User)Activator.CreateInstance(typeof(Model.User));

                int id = Convert.ToInt32(dataReader[idPropertyName]);
                string nick = dataReader[nickPropertyName].ToString();
                string password = dataReader[passwordPropertyName].ToString();

                userModel.Id = id;
                userModel.Nick = nick;
                userModel.Password = password;

                userModels.Add(userModel);
            }

            dataReader.Close();
            dbCommand.Dispose();

            return userModels;
        }

        private void Dispose(DbConnection dbConnection)
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }

        #endregion
    }
}
