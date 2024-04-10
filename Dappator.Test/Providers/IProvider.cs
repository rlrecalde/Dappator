using System.Data.Common;

namespace Dappator.Test.Providers
{
    public interface IProvider
    {
        DbConnection GetOpenConnection(string connectionString);

        DbCommand GetCommand(string query, DbConnection dbConnection);

        string GetInsertUserQuery(string nick, string password);

        string GetInsertUserValueQuery(int userId, double value);

        string GetCreateUserTableQuery();

        string GetDeleteUserQuery();

        string GetCreateUserValueTableQuery();

        string GetDeleteUserValueQuery();

        string GetUsersQuery();

        string GetCreateSpInsertUserQuery();

        string GetCreateSpInsertUserAndGetIdQuery();

        string GetCreateSpGetUserByIdQuery();

        string GetCreateSpGetUsersQuery();

        string GetCreateFnGetUsersQuery();

        string GetCreateFnGetUserIdByNickQuery();
    }
}
