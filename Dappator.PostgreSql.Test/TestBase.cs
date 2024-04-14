using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Dappator.PostgreSql.Test
{
    public class TestBase
    {
        protected string _connectionString;

        protected void CreateTables()
        {
            var dbConnection = this.GetOpenConnection();

            string createUser = @"
                CREATE TABLE IF NOT EXISTS public.user (
                    id INT GENERATED ALWAYS AS IDENTITY NOT NULL,
                    nick VARCHAR NOT NULL,
                    ""password"" VARCHAR NOT NULL,
                    CONSTRAINT user_pk PRIMARY KEY (id)
                )";

            this.ExecuteNonQuery(createUser, dbConnection);

            string createUserValue = @"
                CREATE TABLE IF NOT EXISTS public.uservalue (
                    id INT GENERATED ALWAYS AS IDENTITY NOT NULL,
                    userid INT NOT NULL,
                    ""value"" DECIMAL(10,5) NOT NULL,
                    CONSTRAINT uservalue_pk PRIMARY KEY (id),
                    CONSTRAINT uservalueuser_fk FOREIGN KEY (userid) REFERENCES public.user(id)
                )";

            this.ExecuteNonQuery(createUserValue, dbConnection);

            string createDataType = @"
                CREATE TABLE IF NOT EXISTS public.datatype (
                    id INT GENERATED ALWAYS AS IDENTITY NOT NULL,
                    ""byte"" SMALLINT NULL,
                    ""sbyte"" SMALLINT NULL,
                    ""short"" SMALLINT NULL,
                    ""ushort"" INT NULL,
                    ""int"" INT NULL,
                    ""uint"" BIGINT NULL,
                    ""long"" BIGINT NULL,
                    ""ulong"" DECIMAL NULL,
                    ""float"" REAL NULL,
                    ""double"" FLOAT NULL,
                    ""decimal"" DECIMAL NULL,
                    ""currency"" MONEY NULL,
                    ""bool"" BOOLEAN NULL,
                    ""string"" VARCHAR NULL,
                    ""char"" CHAR NULL,
                    ""guid"" VARCHAR NULL,
                    ""dateTime"" TIMESTAMP NULL,
                    ""dateTimeOffset"" TIMESTAMP with time zone NULL,
                    ""timeSpan"" TIME NULL,
                    ""bytes"" BYTEA NULL,
                    CONSTRAINT datatype_pk PRIMARY KEY (id)
                )";

            this.ExecuteNonQuery(createDataType, dbConnection);

            string createDateAndTime = @"
                CREATE TABLE IF NOT EXISTS public.dateandtime (
                    id INT GENERATED ALWAYS AS IDENTITY NOT NULL,
                    ""datetime"" TIMESTAMP NOT NULL,
                    dateonly DATE NOT NULL,
                    timeonly TIME NOT NULL,
                    CONSTRAINT dateandtime_pk PRIMARY KEY (id)
                )";

            this.ExecuteNonQuery(createDateAndTime, dbConnection);

            string deleteUserValue = "DELETE FROM public.uservalue";

            this.ExecuteNonQuery(deleteUserValue, dbConnection);

            string deleteUser = "DELETE FROM public.user";

            this.ExecuteNonQuery(deleteUser, dbConnection);

            string deleteDataType = "DELETE FROM public.datatype";

            this.ExecuteNonQuery(deleteDataType, dbConnection);

            string deleteDateAndTime = "DELETE FROM dateandtime";

            this.ExecuteNonQuery(deleteDateAndTime, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void InsertUser(Model.User userModel)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO \"user\" (nick, \"password\") VALUES ('{userModel.Nick}', '{userModel.Password}') " +
                $"RETURNING CAST(id AS BIGINT)";

            object id = this.ExecuteScalar(query, dbConnection);
            userModel.Id = (int)(long)id;
            
            this.Dispose(dbConnection);
        }

        protected void InsertUserValue(Model.UserValue userValueModel)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO uservalue (userid, \"value\") VALUES ({userValueModel.UserId}, {userValueModel.Value}) " +
                $"RETURNING CAST(id AS BIGINT)";

            object id = this.ExecuteScalar(query, dbConnection);
            userValueModel.Id = (int)(long)id;

            this.Dispose(dbConnection);
        }

        protected void InsertDataType(Model.DataTypeNullable dataType)
        {
            var dbConnection = this.GetOpenConnection();

            string query = $"" +
                $"INSERT INTO \"datatype\" (\"float\", \"double\") " +
                $"VALUES ({dataType.Float}, {dataType.Double}) " +
                $"RETURNING CAST(id AS BIGINT)";

            object id = this.ExecuteScalar(query, dbConnection);
            dataType.Id = (int)(long)id;

            this.Dispose(dbConnection);
        }

        protected IEnumerable<Model.User> GetUsers()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "SELECT id, nick, \"password\" AS \"\"\"password\"\"\" FROM public.user";

            IEnumerable<Model.User> users = this.GetUsers(query, dbConnection);

            return users;
        }

        protected void CreateSpInsertUser()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE OR REPLACE PROCEDURE insert_user (\n" +
                "   nick VARCHAR,\n" +
                "   pass VARCHAR\n" +
                ")\n" +
                "LANGUAGE SQL\n" +
                "AS $$\n" +
                "   INSERT INTO \"user\" (nick, \"password\") VALUES (nick, pass);\n" +
                "$$;";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpInsertUserAndGetId()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE OR REPLACE PROCEDURE insert_user_and_get_id (\n" +
                "   nick VARCHAR,\n" +
                "   pass VARCHAR,\n" +
                "   INOUT user_id BIGINT DEFAULT NULL\n" +
                ")\n" +
                "LANGUAGE SQL\n" +
                "AS $$\n" +
                "   INSERT INTO \"user\" (nick, \"password\") VALUES (nick, pass) RETURNING CAST(id AS BIGINT);\n" +
                "$$;";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpGetUserById()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE OR REPLACE PROCEDURE get_user_by_id (\n" +
                "   IN p_id INT,\n" +
                "   INOUT id INT DEFAULT NULL,\n" +
                "   INOUT nick VARCHAR DEFAULT NULL,\n" +
                "   INOUT password VARCHAR DEFAULT NULL\n" +
                ")\n" +
                "LANGUAGE SQL\n" +
                "AS $$\n" +
                "   SELECT id, nick, \"password\" FROM \"user\" WHERE id = p_id LIMIT 1\n" +
                "$$;";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateSpGetUsers()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateFnGetUsers()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE OR REPLACE FUNCTION get_users ()\n" +
                "RETURNS SETOF \"user\"\n" +
                "LANGUAGE SQL\n" +
                "AS $$\n" +
                "   SELECT * FROM \"user\"\n" +
                "$$;";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        protected void CreateFnGetUserIdByNick()
        {
            var dbConnection = this.GetOpenConnection();

            string query = "" +
                "CREATE OR REPLACE FUNCTION get_userid_by_nick\n" +
                "(\n" +
                "   p_nick VARCHAR\n" +
                ")\n" +
                "RETURNS INT\n" +
                "LANGUAGE SQL\n" +
                "AS $$\n" +
                "   SELECT id FROM \"user\" WHERE nick = p_nick LIMIT 1\n" +
                "$$;";

            this.ExecuteNonQuery(query, dbConnection);

            this.Dispose(dbConnection);
        }

        #region Private Methods

        private NpgsqlConnection GetOpenConnection()
        {
            var dbConnection = new NpgsqlConnection(this._connectionString);
            dbConnection.Open();

            return dbConnection;
        }

        private void ExecuteNonQuery(string query, NpgsqlConnection sqlConnection)
        {
            var dbCommand = new NpgsqlCommand(query, sqlConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();
        }

        private object ExecuteScalar(string query, NpgsqlConnection sqlConnection)
        {
            var dbCommand = new NpgsqlCommand(query, sqlConnection);
            object id = dbCommand.ExecuteScalar();
            dbCommand.Dispose();

            return id;
        }

        private IEnumerable<Model.User> GetUsers(string query, NpgsqlConnection sqlConnection)
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
            var dbCommand = new NpgsqlCommand(query, sqlConnection);
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
