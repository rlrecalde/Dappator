using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dappator
{
    public interface IDataContext : IDisposable
    {
        string ConnectionString { get; }

        DbConnection DbConnection { get; }

        /// <summary>
        /// Indicates whether the following executions must be in a SQL Transaction.<br/>
        /// In case it's true, it creates a DbTransaction object that could be get via GetDbTransaction() method after the first execution.<br/>
        /// When it's set to false, it releases the previous created DbTransaction object.<br/>
        /// </summary>
        bool ExecuteInTransaction { get; set; }

        /// <summary>
        /// Sets the IsolationLevel for the Transaction.<br/>
        /// If null, Transaction is created with default IsolationLevel value.
        /// </summary>
        System.Data.IsolationLevel? TransactionIsolationLevel { get; set; }

        /// <summary>
        /// Sets Dapper "buffered" parameter.
        /// </summary>
        bool Buffered { get; set; }

        /// <summary>
        /// The command timeout (in seconds).
        /// </summary>
        /// <param name="commandTimeout"></param>
        void SetCommandTimeout(int commandTimeout);

        /// <summary>
        /// Returns the DbTransaction object in order to do Commit, Rollback and Dispose.<br/>
        /// It has to be used when ExecuteInTransaction() method is called, otherwise this object is null.
        /// </summary>
        DbTransaction GetDbTransaction();

        /// <summary>
        /// Persists a SQL query in order to be executed later
        /// </summary>
        /// <param name="query">SQL query to be persisted</param>
        /// <param name="values">Parameter values for the query</param>
        Interfaces.IQueryBuilderExecutable SetQuery(string query, params object[] values);

        /// <summary>
        /// Constructs the SELECT statement<br/>
        /// Usage:<br/>
        /// .Select&lt;Entity&gt;(x => new { x.Prop1, x.Prop2, ... })
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the "from" SELECT statement</typeparam>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) for the SELECT statement</param>
        /// <param name="distinct">Boolean for SELECT DISTINCT statement</param>
        /// <param name="alias">The "AS" alias statement when same table is joinned more than once</param>
        /// <param name="aggregate">Action for introduce Count(), Max(), Min(), Sum() and Avg()</param>
        Interfaces.IQueryBuilderJoin Select<T>(Expression<Func<T, dynamic>> properties, bool distinct = false, string alias = null, Action<Interfaces.IQueryBuilderAggregate> aggregate = null);

        /// <summary>
        /// Constructs the INSERT statement<br/>
        /// Usage:<br/>
        /// .Insert&lt;Entity&gt;(x => new { x.Prop1, x.Prop2, x.Prop3, ... }, 5, "text", DateTime.Now)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) for the INSERT statement</param>
        /// <param name="values">Values to be inserted</param>
        Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, params object[] values);

        /// <summary>
        /// Constructs the INSERT statement<br/>
        /// Usage:<br/>
        /// .Insert&lt;Entity&gt;(x => new { x.Prop1, x.Prop2, x.Prop3, ... }, object)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) for the INSERT statement</param>
        /// <param name="entity">Object of type <typeparamref name="T"/> with the values to be inserted</param>
        Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, T entity);

        /// <summary>
        /// Constructs the INSERT statement (for bulk insertion)<br/>
        /// Usage:<br/>
        /// .Insert&lt;Entity&gt;(x => new { x.Prop1, x.Prop2, x.Prop3, ... }, collection)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) for the INSERT statement</param>
        /// <param name="entities">Collection of objects of type <typeparamref name="T"/> with the values to be inserted</param>
        Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, IEnumerable<T> entities);

        /// <summary>
        /// Constructs the UPDATE statement<br/>
        /// Usage:<br/>
        /// .Update&lt;Entity&gt;(x => new { x.Prop1, x.Prop2, x.Prop3, ... }, 5, "text", DateTime.Now)<br/>
        /// (usually used followed by a .Where())
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) to be updated</param>
        /// <param name="values">Values to update</param>
        Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, params object[] values);

        /// <summary>
        /// Constructs the UPDATE statement<br/>
        /// Usage:<br/>
        /// .Update&lt;Entity&gt;(x => new { x.Prop1, x.Prop2, x.Prop3, ... }, object)<br/>
        /// (usually used followed by a .Where())
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) to be updated</param>
        /// <param name="entity">Object of type <typeparamref name="T"/> with the values to update</param>
        Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, T entity);

        /// <summary>
        /// Constructs the DELETE statement<br/>
        /// Usage:<br/>
        /// .Delete&lt;Entity&gt;()<br/>
        /// (usually used followed by a .Where&lt;Entity&gt;())
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        Interfaces.IQueryBuilderWhere Delete<T>();

        /// <summary>
        /// Constructs the EXEC [stored procedure name] [parameters/values] statement<br/>
        /// Usage:<br/>
        /// .StoredProcedure&lt;SpEntity&gt;(x => new { x.Param1, x.Param2, x.Param3, ... }, 5, "text", DateTime.Now)
        /// </summary>
        /// <typeparam name="T">Type of the class that represents the stored procedure</typeparam>
        /// <param name="properties">Lambda Expression for the input parameters</param>
        /// <param name="values">Values of those parameters</param>
        Interfaces.IQueryBuilderSpExecutable StoredProcedure<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);

        /// <summary>
        /// Constructs the SELECT * FROM [function name] ([values]) statement<br/>
        /// Usage:<br/>
        /// .TableFunction&lt;SpEntity&gt;(x => new { x.Param1, x.Param2, x.Param3, ... }, 5, "text", DateTime.Now)
        /// </summary>
        /// <typeparam name="T">Type of the class that represents the function</typeparam>
        /// <param name="properties">Lambda Expression for the input parameters</param>
        /// <param name="values">Values of those parameters</param>
        Interfaces.IQueryBuilderTableFunctionExecutable TableFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);

        /// <summary>
        /// Constructs the SELECT [function name] ([values]) statement<br/>
        /// Usage:<br/>
        /// .ScalarFunction&lt;SpEntity&gt;(x => new { x.Param1, x.Param2, x.Param3, ... }, 5, "text", DateTime.Now)
        /// </summary>
        /// <typeparam name="T">Type of the class that represents the function</typeparam>
        /// <param name="properties">Lambda Expression for the input parameters</param>
        /// <param name="values">Values of those parameters</param>
        Interfaces.IQueryBuilderScalarFunctionExecutable ScalarFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);

        void Close();

        Task CloseAsync();
    }
}
