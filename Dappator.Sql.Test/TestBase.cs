using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Dappator.Sql.Test
{
    public class TestBase
    {
        protected string _connectionString;

        protected void CreateTables()
        {
            var dbConnection = this.GetOpenConnection();

            string createUser = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'User')
                    CREATE TABLE [User] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [Nick] [VARCHAR](50) NOT NULL,
                        [Password] [VARCHAR](50) NOT NULL,
                        CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
                    )";
            
            this.ExecuteNonQuery(createUser, dbConnection);

            string createUserValue = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'UserValue')
                    CREATE TABLE [UserValue] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [UserId] [INT] NOT NULL,
                        [Value] [DECIMAL](10,5) NOT NULL,
                        CONSTRAINT [PK_UserValue] PRIMARY KEY CLUSTERED ([Id] ASC),
                        CONSTRAINT [FK_UserValueUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
                    )";
            
            this.ExecuteNonQuery(createUserValue, dbConnection);

            string createDataType = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'DataType')
                    CREATE TABLE [DataType] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [Byte] [TINYINT] NULL,
                        [Sbyte] [SMALLINT] NULL,
                        [Short] [SMALLINT] NULL,
                        [Ushort] [INT] NULL,
                        [Int] [INT] NULL,
                        [Uint] [BIGINT] NULL,
                        [Long] [BIGINT] NULL,
                        [Ulong] [DECIMAL](20,0) NULL,
                        [Float] [REAL] NULL,
                        [Double] [FLOAT] NULL,
                        [Decimal] [DECIMAL](29,10) NULL,
                        [Currency] [MONEY] NULL,
                        [Bool] [BIT] NULL,
                        [String] [VARCHAR](MAX) NULL,
                        [Char] [CHAR](1) NULL,
                        [Guid] [VARCHAR](100) NULL,
                        [DateTime] [DATETIME] NULL,
                        [DateTimeOffset] [DATETIMEOFFSET] NULL,
                        [TimeSpan] [TIME] NULL,
                        [Bytes] [BINARY](100) NULL,
                        [DateOnly] [DATE] NULL,
                        [TimeOnly] [TIME] NULL,
                        CONSTRAINT [PK_DataType] PRIMARY KEY CLUSTERED ([Id] ASC)
                    )";

            this.ExecuteNonQuery(createDataType, dbConnection);

            string deleteUserValue = "DELETE FROM [UserValue]";
            
            this.ExecuteNonQuery(deleteUserValue, dbConnection);

            string deleteUser = "DELETE FROM [User]";
            
            this.ExecuteNonQuery(deleteUser, dbConnection);

            string deleteDataType = "DELETE FROM [DataType]";

            this.ExecuteNonQuery(deleteDataType, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void InsertUser(Model.User userModel)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO [User] ([Nick], [Password]) VALUES ('{userModel.Nick}', '{userModel.Password}'); " +
                $"SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            object id = this.ExecuteScalar(query, dbConnection);
            userModel.Id = (int)(long)id;
            
            this.Dispose(dbConnection);
        }

        protected void InsertUserValue(Model.UserValue userValueModel)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO [UserValue] ([UserId], [Value]) VALUES ({userValueModel.UserId}, {userValueModel.Value}); " +
                $"SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            object id = this.ExecuteScalar(query, dbConnection);
            userValueModel.Id = (int)(long)id;

            this.Dispose(dbConnection);
        }

        protected void InsertDataType(Model.DataTypeNullable dataType)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO [DataType] ([Float], [Double]) " +
                $"VALUES ({dataType.Float}, {dataType.Double}); " +
                $"SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            object id = this.ExecuteScalar(query, dbConnection);
            dataType.Id = (int)(long)id;

            this.Dispose(dbConnection);
        }

        protected IEnumerable<Model.User> GetUsers()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "SELECT [Id] AS '[Id]', [Nick] AS '[Nick]', [Password] AS '[Password]' FROM [User]";

            IEnumerable<Model.User> users = this.GetUsers(query, dbConnection);

            return users;
        }

        protected void CreateSpInsertUser()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE OR ALTER PROCEDURE InsertUser (\n" +
                "   @Nick VARCHAR(50),\n" +
                "   @Password VARCHAR(50)\n" +
                ")\n" +
                "AS\n" +
                "BEGIN\n" +
                "   INSERT INTO [User] ([Nick], [Password]) VALUES (@Nick, @Password)\n" +
                "END;";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpInsertUserAndGetId()
        {
            var dbConnection = this.GetOpenConnection();

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

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpGetUserById()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE OR ALTER PROCEDURE GetUserById (\n" +
                "   @Id INT\n" +
                ")\n" +
                "AS\n" +
                "BEGIN\n" +
                "   SELECT TOP 1 * FROM [User] WHERE Id = @Id\n" +
                "END;";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpGetUsers()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE OR ALTER PROCEDURE GetUsers\n" +
                "AS\n" +
                "BEGIN\n" +
                "   SELECT * FROM [User]\n" +
                "END;";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateFnGetUsers()
        {
            var dbConnection = this.GetOpenConnection();

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

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateFnGetUserIdByNick()
        {
            var dbConnection = this.GetOpenConnection();

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

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        #region Private Methods

        private SqlConnection GetOpenConnection()
        {
            var dbConnection = new SqlConnection(this._connectionString);
            dbConnection.Open();

            return dbConnection;
        }

        private void ExecuteNonQuery(string query, SqlConnection sqlConnection)
        {
            var dbCommand = new SqlCommand(query, sqlConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();
        }

        private object ExecuteScalar(string query, SqlConnection sqlConnection)
        {
            var dbCommand = new SqlCommand(query, sqlConnection);
            object id = dbCommand.ExecuteScalar();
            dbCommand.Dispose();

            return id;
        }

        private IEnumerable<Model.User> GetUsers(string query, SqlConnection sqlConnection)
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
            var dbCommand = new SqlCommand(query, sqlConnection);
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
