using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;

namespace Dappator.Internal
{
    internal interface IQueryBuilderMain
    {
        bool ExecuteInTransaction { get; set; }

        System.Data.IsolationLevel? TransactionIsolationLevel { get; set; }

        bool Buffered { get; set; }

        void SetCommandTimeout(int commandTimeout);

        DbTransaction GetDbTransaction();

        Interfaces.IQueryBuilderExecutable SetQuery(string query, params object[] values);

        Interfaces.IQueryBuilderJoin Select<T>(Expression<Func<T, dynamic>> properties, bool distinct = false, string alias = null, Action<Interfaces.IQueryBuilderAggregate> aggregate = null);

        Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, params object[] values);

        Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, T entity);

        Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, IEnumerable<T> entities);

        Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, params object[] values);

        Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, T entity);

        Interfaces.IQueryBuilderWhere Delete<T>();

        Interfaces.IQueryBuilderSpExecutable StoredProcedure<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);

        Interfaces.IQueryBuilderTableFunctionExecutable TableFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);

        Interfaces.IQueryBuilderScalarFunctionExecutable ScalarFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);
    }
}
