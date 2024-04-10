using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dappator
{
    public abstract class DataContextBase : IDataContext
    {
        protected string _connectionString;
        protected DbConnection _dbConnection;
        internal Internal.IQueryBuilderMain _queryBuilderMain;

        public string ConnectionString { get { return this._connectionString; } }

        public DbConnection DbConnection { get { return this._dbConnection; } }

        public bool ExecuteInTransaction { get { return this._queryBuilderMain.ExecuteInTransaction; } set { this._queryBuilderMain.ExecuteInTransaction = value; } }

        public IsolationLevel? TransactionIsolationLevel { get { return this._queryBuilderMain.TransactionIsolationLevel; } set { this._queryBuilderMain.TransactionIsolationLevel = value; } }

        public bool Buffered { get { return this._queryBuilderMain.Buffered; } set { this._queryBuilderMain.Buffered = value; } }

        public void SetCommandTimeout(int commandTimeout)
        {
            this._queryBuilderMain.SetCommandTimeout(commandTimeout);
        }

        public DbTransaction GetDbTransaction()
        {
            DbTransaction dbTransaction = this._queryBuilderMain.GetDbTransaction();

            return dbTransaction;
        }

        public Interfaces.IQueryBuilderExecutable SetQuery(string query, params object[] values)
        {
            return this._queryBuilderMain.SetQuery(query, values);
        }

        public Interfaces.IQueryBuilderJoin Select<T>(Expression<Func<T, dynamic>> properties, bool distinct = false, string alias = null, Action<Interfaces.IQueryBuilderAggregate> aggregate = null)
        {
            return this._queryBuilderMain.Select<T>(properties, distinct, alias, aggregate);
        }

        public Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            return this._queryBuilderMain.Insert<T>(properties, values);
        }

        public Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, T entity)
        {
            return this._queryBuilderMain.Insert<T>(properties, entity);
        }

        public Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, IEnumerable<T> entities)
        {
            return this._queryBuilderMain.Insert<T>(properties, entities);
        }

        public Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            return this._queryBuilderMain.Update<T>(properties, values);
        }

        public Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, T entity)
        {
            return this._queryBuilderMain.Update<T>(properties, entity);
        }

        public Interfaces.IQueryBuilderWhere Delete<T>()
        {
            return this._queryBuilderMain.Delete<T>();
        }

        public Interfaces.IQueryBuilderSpExecutable StoredProcedure<T>(Expression<Func<T, dynamic>> properties = null, params object[] values)
        {
            return this._queryBuilderMain.StoredProcedure<T>(properties, values);
        }

        public Interfaces.IQueryBuilderTableFunctionExecutable TableFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values)
        {
            return this._queryBuilderMain.TableFunction<T>(properties, values);
        }

        public Interfaces.IQueryBuilderScalarFunctionExecutable ScalarFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values)
        {
            return this._queryBuilderMain.ScalarFunction<T>(properties, values);
        }

        public virtual void Close()
        {
            if (this._dbConnection == null)
                return;

            this.BasicClose();
        }

        public virtual async Task CloseAsync()
        {
            if (this._dbConnection == null)
                return;

            await this.BasicCloseAsync();
        }

        public virtual void Dispose()
        {
            if (this._dbConnection == null)
                return;

            this.BasicClose();
            this._dbConnection.Dispose();
        }

        protected void BasicClose()
        {
            if (this._dbConnection.State != ConnectionState.Closed)
                this._dbConnection.Close();
        }

        protected async Task BasicCloseAsync()
        {
            if (this._dbConnection.State != ConnectionState.Closed)
            {
#if NETCOREAPP2_1_OR_GREATER
                await this._dbConnection.CloseAsync();
#else
                await Task.Run(() => this._dbConnection.Close());
#endif
            }
        }
    }
}
