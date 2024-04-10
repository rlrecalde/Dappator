using FirebirdSql.Data.FirebirdClient;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

namespace Dappator.Test
{
    public class See_Firebird_TestBase<TProvider> where TProvider : Providers.IProvider, new()
    {
        protected TProvider _provider;
        protected string _connectionString;

        public See_Firebird_TestBase()
        {
            this._provider = new TProvider();
        }

        protected void CreateTables()
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);

            string createUser = this._provider.GetCreateUserTableQuery();
            DbCommand dbCommand = this._provider.GetCommand(createUser, dbConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            string createUserValue = this._provider.GetCreateUserValueTableQuery();
            dbCommand = this._provider.GetCommand(createUserValue, dbConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            string deleteUserValue = this._provider.GetDeleteUserValueQuery();
            dbCommand = this._provider.GetCommand(deleteUserValue, dbConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            string deleteUser = this._provider.GetDeleteUserQuery();
            dbCommand = this._provider.GetCommand(deleteUser, dbConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            dbConnection.Close();
            dbConnection.Dispose();
        }

        protected void InsertUser<T>(T userModel) where T : Model.IUser
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);
            string query = this._provider.GetInsertUserQuery(userModel.Nick, userModel.Password);
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);

            if (this._provider is Providers.OracleProvider)
            {
                var oracleParameter = new OracleParameter("generated_id", OracleDbType.Int64, System.Data.ParameterDirection.ReturnValue);
                ((OracleCommand)dbCommand).Parameters.Add(oracleParameter);
            }

            object id = dbCommand.ExecuteScalar();
            
            if (this._provider is Providers.OracleProvider)
                id = ((OracleCommand)dbCommand).Parameters["generated_id"].Value;

            userModel.Id = (int)Convert.ToInt64(id.ToString());
            
            this.Dispose(dbCommand, dbConnection);
        }

        protected IEnumerable<T> GetUsers<T>() where T : Model.IUser
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);
            string query = this._provider.GetUsersQuery();
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);

            IEnumerable<T> users = this.GetUsers<T>(dbCommand);

            return users;
        }

        protected void InsertUserValue<T>(T userValueModel) where T : Model.IUserValue
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);
            string query = this._provider.GetInsertUserValueQuery(userValueModel.UserId, userValueModel.Value);
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);

            if (this._provider is Providers.OracleProvider)
            {
                var oracleParameter = new OracleParameter("generated_id", OracleDbType.Int64, System.Data.ParameterDirection.ReturnValue);
                ((OracleCommand)dbCommand).Parameters.Add(oracleParameter);
            }

            object id = dbCommand.ExecuteScalar();
            
            if (this._provider is Providers.OracleProvider)
                id = ((OracleCommand)dbCommand).Parameters["generated_id"].Value;

            userValueModel.Id = (int)Convert.ToInt64(id.ToString());

            this.Dispose(dbCommand, dbConnection);
        }

        protected void CreateSpInsertUser()
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);

            FbTransaction fbTransaction = this.GetFirebirdTransaction(dbConnection);

            string query = this._provider.GetCreateSpInsertUserQuery();
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);
            dbCommand.Transaction = fbTransaction;
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            this.CommitFirebirdTransaction(fbTransaction);
        }

        protected void CreateSpInsertUserAndGetId()
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);

            FbTransaction fbTransaction = this.GetFirebirdTransaction(dbConnection);

            string query = this._provider.GetCreateSpInsertUserAndGetIdQuery();
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);
            dbCommand.Transaction = fbTransaction;
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            this.CommitFirebirdTransaction(fbTransaction);
        }

        protected void CreateSpGetUserById()
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);

            FbTransaction fbTransaction = this.GetFirebirdTransaction(dbConnection);

            string query = this._provider.GetCreateSpGetUserByIdQuery();
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);
            dbCommand.Transaction = fbTransaction;
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            this.CommitFirebirdTransaction(fbTransaction);
        }

        protected void CreateSpGetUsers()
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);

            FbTransaction fbTransaction = this.GetFirebirdTransaction(dbConnection);

            string query = this._provider.GetCreateSpGetUsersQuery();
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);
            dbCommand.Transaction = fbTransaction;
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            this.CommitFirebirdTransaction(fbTransaction);
        }

        protected void CreateFnGetUsers()
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);

            FbTransaction fbTransaction = this.GetFirebirdTransaction(dbConnection);

            if (this._provider is Providers.OracleProvider)
                this.CreateOracleTypes(dbConnection);

            string query = this._provider.GetCreateFnGetUsersQuery();
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);
            dbCommand.Transaction = fbTransaction;
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            this.CommitFirebirdTransaction(fbTransaction);
        }

        protected void CreateFnGetUserIdByNick()
        {
            DbConnection dbConnection = this._provider.GetOpenConnection(this._connectionString);

            FbTransaction fbTransaction = this.GetFirebirdTransaction(dbConnection);

            string query = this._provider.GetCreateFnGetUserIdByNickQuery();
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);
            dbCommand.Transaction = fbTransaction;
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            this.CommitFirebirdTransaction(fbTransaction);
        }

        protected void InsertUserSqlite(Model.Firebirdite.User user, DbConnection dbConnection)
        {
            string query = this._provider.GetInsertUserQuery(user.Nick, user.Password);
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);

            long id = (long)dbCommand.ExecuteScalar();
            user.Id = (int)id;

            this.Dispose(dbCommand);
        }

        protected IEnumerable<Model.Firebirdite.User> GetUsersSqlite(DbConnection dbConnection)
        {
            string query = this._provider.GetUsersQuery();
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);

            IEnumerable<Model.Firebirdite.User> users = this.GetUsers<Model.Firebirdite.User>(dbCommand);

            return users;
        }

        protected void InsertUserValueSqlite(Model.Firebirdite.UserValue userValue, DbConnection dbConnection)
        {
            string query = this._provider.GetInsertUserValueQuery(userValue.UserId, userValue.Value);
            DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);

            long id = (long)dbCommand.ExecuteScalar();
            userValue.Id = (int)id;

            this.Dispose(dbCommand);
        }

        #region Private Methods

        private IEnumerable<T> GetUsers<T>(DbCommand dbCommand) where T : Model.IUser
        {
            string idPropertyName = string.Empty;
            string nickPropertyName = string.Empty;
            string passwordPropertyName = string.Empty;

            var userModelReference = Activator.CreateInstance(typeof(T));
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

            var userModels = new List<T>();
            DbDataReader dataReader = dbCommand.ExecuteReader();
            while (dataReader.Read())
            {
                var userModel = Activator.CreateInstance(typeof(T));

                int id = Convert.ToInt32(dataReader[idPropertyName]);
                string nick = dataReader[nickPropertyName].ToString();
                string password = dataReader[passwordPropertyName].ToString();

                ((T)userModel).Id = id;
                ((T)userModel).Nick = nick;
                ((T)userModel).Password = password;

                userModels.Add((T)userModel);
            }

            dataReader.Close();

            return userModels;
        }

        private FbTransaction GetFirebirdTransaction(DbConnection dbConnection)
        {
            FbTransaction fbTransaction = null;
            if (this._provider is Providers.FirebirdProvider)
            {
                var fbTransactionOptions = new FbTransactionOptions();
                fbTransactionOptions.TransactionBehavior = FbTransactionBehavior.Wait;
                fbTransaction = ((FbConnection)dbConnection).BeginTransaction(fbTransactionOptions);
            }

            return fbTransaction;
        }

        private void CommitFirebirdTransaction(FbTransaction fbTransaction)
        {
            if (this._provider is Providers.FirebirdProvider)
            {
                fbTransaction.Commit();
                fbTransaction.Dispose();
            }
        }

        private void CreateOracleTypes(DbConnection dbConnection)
        {
            try
            {
                string query = "CREATE OR REPLACE TYPE t_user_record AS OBJECT (Id INT, Nick VARCHAR(50), Password VARCHAR(50));";
                DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
            }
            catch (Exception) { }

            try
            {
                string query = "CREATE OR REPLACE TYPE t_user AS TABLE OF t_user_record;";
                DbCommand dbCommand = this._provider.GetCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
            }
            catch (Exception) { }
        }

        private void Dispose(DbCommand dbCommand, DbConnection dbConnection = null)
        {
            dbCommand.Dispose();

            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }

        #endregion
    }
}
