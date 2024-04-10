#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Dappator.Sqlite.Test
{
    public class TestBase
    {
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        protected SqliteConnection _sqliteConnection;
#else
        protected SQLiteConnection _sqliteConnection;
#endif

        protected void CreateTables()
        {
            string createUser = "" +
                "CREATE TABLE User (" +
                "   Id INTEGER PRIMARY KEY, " +
                "   Nick TEXT NOT NULL, " +
                "   Password TEXT NOT NULL" +
                ");";

            this.ExecuteNonQuery(createUser, this._sqliteConnection);

            string createUserValue = "" +
                "CREATE TABLE UserValue (" +
                "   Id INTEGER PRIMARY KEY, " +
                "   UserId INTEGER NOT NULL, " +
                "   [Value] REAL NOT NULL, " +
                "   FOREIGN KEY(UserId) REFERENCES [User](Id)" +
                ");";

            this.ExecuteNonQuery(createUserValue, this._sqliteConnection);

            string createDataType = "" +
                "CREATE TABLE DataType (" +
                "   Id INTEGER PRIMARY KEY, " +
                "   Byte TINYINT NULL, " +
                "   Sbyte SMALLINT NULL, " +
                "   Short SMALLINT NULL, " +
                "   Ushort INT NULL, " +
                "   Int INT NULL, " +
                "   Uint BIGINT NULL, " +
                "   Long BIGINT NULL, " +
                "   Ulong DECIMAL NULL, " +
                "   Float REAL NULL, " +
                "   Double FLOAT NULL, " +
                "   Decimal DECIMAL NULL, " +
                "   Currency MONEY NULL, " +
                "   Bool BIT NULL, " +
                "   String VARCHAR NULL, " +
                "   Char CHAR NULL, " +
                "   Guid VARCHAR NULL, " +
                "   DateTime DATETIME NULL, " +
                "   DateTimeOffset DATETIMEOFFSET NULL, " +
                "   TimeSpan TIME NULL, " +
                "   Bytes BINARY NULL, " +
                "   DateOnly DATE NULL, " +
                "   TimeOnly TIME NULL " +
                ");";

            this.ExecuteNonQuery(createDataType, this._sqliteConnection);

            string deleteUserValue = "DELETE FROM UserValue";

            this.ExecuteNonQuery(deleteUserValue, this._sqliteConnection);

            string deleteUser = "DELETE FROM User";

            this.ExecuteNonQuery(deleteUser, this._sqliteConnection);

            string deleteDataType = "DELETE FROM [DataType]";

            this.ExecuteNonQuery(deleteDataType, this._sqliteConnection);
        }

        protected void InsertUser(Model.User userModel)
        {
            string query = $"" +
                $"INSERT INTO [User] ([Nick], [Password]) VALUES ('{userModel.Nick}', '{userModel.Password}'); " +
                $"SELECT CAST(last_insert_rowid() AS BIGINT)";

            object id = this.ExecuteScalar(query, this._sqliteConnection);
            userModel.Id = (int)(long)id;
        }

        protected void InsertUserValue(Model.UserValue userValueModel)
        {
            string query = $"" +
                $"INSERT INTO [UserValue] ([UserId], [Value]) VALUES ({userValueModel.UserId}, {userValueModel.Value}); " +
                $"SELECT CAST(last_insert_rowid() AS BIGINT)";

            object id = this.ExecuteScalar(query, this._sqliteConnection);
            userValueModel.Id = (int)(long)id;
        }

        protected void InsertDataType(Model.DataTypeNullable dataType)
        {
            string query = $"" +
                $"INSERT INTO [DataType] ([Float], [Double]) " +
                $"VALUES ({dataType.Float}, {dataType.Double}); " +
                $"SELECT CAST(last_insert_rowid() AS BIGINT)";

            object id = this.ExecuteScalar(query, this._sqliteConnection);
            dataType.Id = (int)(long)id;
        }

        protected IEnumerable<Model.User> GetUsers()
        {
            string query = "SELECT [Id] AS '[Id]', [Nick] AS '[Nick]', [Password] AS '[Password]' FROM User";

            IEnumerable<Model.User> users = this.GetUsers(query, this._sqliteConnection);

            return users;
        }

        protected void CreateSpInsertUser()
        {
            string query = "" +
                "";

            this.ExecuteNonQuery(query, this._sqliteConnection);
        }

        protected void CreateSpInsertUserAndGetId()
        {
            string query = "" +
                "";

            this.ExecuteNonQuery(query, this._sqliteConnection);
        }

        protected void CreateSpGetUserById()
        {
            string query = "" +
                "";

            this.ExecuteNonQuery(query, this._sqliteConnection);
        }

        protected void CreateSpGetUsers()
        {
            string query = "" +
                "";

            this.ExecuteNonQuery(query, this._sqliteConnection);
        }

        protected void CreateFnGetUsers()
        {
            string query = "" +
                "";

            this.ExecuteNonQuery(query, this._sqliteConnection);
        }

        protected void CreateFnGetUserIdByNick()
        {
            string query = "" +
                "";

            this.ExecuteNonQuery(query, this._sqliteConnection);
        }

        #region Private Methods

#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER

        private void ExecuteNonQuery(string query, SqliteConnection sqlConnection)
        {
            var dbCommand = sqlConnection.CreateCommand();
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();
        }

        private object ExecuteScalar(string query, SqliteConnection sqlConnection)
        {
            var dbCommand = sqlConnection.CreateCommand();
            dbCommand.CommandText = query;
            object id = dbCommand.ExecuteScalar();
            dbCommand.Dispose();

            return id;
        }

        private IEnumerable<Model.User> GetUsers(string query, SqliteConnection sqlConnection)
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
            var dbCommand = sqlConnection.CreateCommand();
            dbCommand.CommandText = query;
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

#else

        private void ExecuteNonQuery(string query, SQLiteConnection sqlConnection)
        {
            var dbCommand = sqlConnection.CreateCommand();
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();
        }

        private object ExecuteScalar(string query, SQLiteConnection sqlConnection)
        {
            var dbCommand = sqlConnection.CreateCommand();
            dbCommand.CommandText = query;
            object id = dbCommand.ExecuteScalar();
            dbCommand.Dispose();

            return id;
        }

        private IEnumerable<Model.User> GetUsers(string query, SQLiteConnection sqlConnection)
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
            var dbCommand = sqlConnection.CreateCommand();
            dbCommand.CommandText = query;
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

#endif

        #endregion
    }
}
