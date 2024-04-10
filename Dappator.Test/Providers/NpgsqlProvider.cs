using Npgsql;
using System.Data.Common;

namespace Dappator.Test.Providers
{
    public class NpgsqlProvider : IProvider
    {
        public DbConnection GetOpenConnection(string connectionString)
        {
            var FbConnection = new NpgsqlConnection(connectionString);
            FbConnection.Open();

            return FbConnection;
        }

        public DbCommand GetCommand(string query, DbConnection dbConnection)
        {
            var FbCommand = new NpgsqlCommand(query, (NpgsqlConnection)dbConnection);

            return FbCommand;
        }

        public string GetInsertUserQuery(string nick, string password)
        {
            string query = $"" +
                $"INSERT INTO \"user\" (nick, \"password\") VALUES ('{nick}', '{password}') " +
                $"RETURNING CAST(id AS BIGINT)";

            return query;
        }

        public string GetInsertUserValueQuery(int userId, double value)
        {
            string query = $"" +
                $"INSERT INTO uservalue (userid, \"value\") VALUES ({userId}, {value}) " +
                $"RETURNING CAST(id AS BIGINT)";

            return query;
        }

        public string GetCreateUserTableQuery()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS public.user (
                    id INT GENERATED ALWAYS AS IDENTITY NOT NULL,
                    nick VARCHAR NOT NULL,
                    ""password"" VARCHAR NOT NULL,
                    CONSTRAINT user_pk PRIMARY KEY (id)
                )";

            return query;
        }

        public string GetDeleteUserQuery()
        {
            string query = "DELETE FROM public.user";

            return query;
        }

        public string GetCreateUserValueTableQuery()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS public.uservalue (
                    id INT GENERATED ALWAYS AS IDENTITY NOT NULL,
                    userid INT NOT NULL,
                    ""value"" DECIMAL(10,5) NOT NULL,
                    CONSTRAINT uservalue_pk PRIMARY KEY (id),
                    CONSTRAINT uservalueuser_fk FOREIGN KEY (userid) REFERENCES public.user(id)
                )";

            return query;
        }

        public string GetDeleteUserValueQuery()
        {
            string query = "DELETE FROM public.uservalue";

            return query;
        }

        public string GetUsersQuery()
        {
            string query = "SELECT id, nick, \"password\" AS \"\"\"password\"\"\" FROM public.user";

            return query;
        }

        public string GetCreateSpInsertUserQuery()
        {
            string query = "" +
                "CREATE OR REPLACE PROCEDURE insert_user (\n" +
                "   nick VARCHAR,\n" +
                "   pass VARCHAR\n" +
                ")\n" +
                "LANGUAGE SQL\n" +
                "AS $$\n" +
                "   INSERT INTO \"user\" (nick, \"password\") VALUES (nick, pass);\n" +
                "$$;";

            return query;
        }

        public string GetCreateSpInsertUserAndGetIdQuery()
        {
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

            return query;
        }

        public string GetCreateSpGetUserByIdQuery()
        {
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

            return query;
        }

        public string GetCreateSpGetUsersQuery()
        {
            string query = "" +
                "";

            return query;
        }

        public string GetCreateFnGetUsersQuery()
        {
            string query = "" +
                "CREATE OR REPLACE FUNCTION get_users ()\n" +
                "RETURNS SETOF \"user\"\n" +
                "LANGUAGE SQL\n" +
                "AS $$\n" +
                "   SELECT * FROM \"user\"\n" +
                "$$;";

            return query;
        }

        public string GetCreateFnGetUserIdByNickQuery()
        {
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

            return query;
        }
    }
}
