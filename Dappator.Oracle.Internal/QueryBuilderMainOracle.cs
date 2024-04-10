using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace Dappator.Oracle.Internal
{
    internal class QueryBuilderMainOracle : Dappator.Internal.QueryBuilderMain
    {
        public QueryBuilderMainOracle(DbConnection dbConnection)
        {
            base._dbConnection = dbConnection;
            base._dbType = DbType.Oracle;
        }

        public override Interfaces.IQueryBuilderJoin Select<T>(Expression<Func<T, dynamic>> properties, bool distinct = false, string alias = null, Action<Interfaces.IQueryBuilderAggregate> aggregate = null)
        {
            return base.BasicSelect<T>(properties, distinct, alias, aggregate);
        }

        public override Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            base.BasicInsert<T>(properties, values);

            base._query = "BEGIN " + base._query;

            bool isBulk = values.All(x => x != null && x.GetType().IsArray && x.GetType().GetElementType() == typeof(object));
            if (!isBulk)
                base._query += " RETURNING CAST(Id AS NUMBER) INTO :generated_id";

            base._query += "; END;";

            var executable = base.GetQueryBuilderReturning();

            return executable;
        }

        public override Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, T entity)
        {
            base.BasicInsert<T>(properties, entity);

            base._query = $"BEGIN {base._query} RETURNING CAST(Id AS NUMBER) INTO :generated_id; END;";

            var executable = base.GetQueryBuilderReturning();

            return executable;
        }

        public override Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, IEnumerable<T> entities)
        {
            base.BasicInsert<T>(properties, entities);

            var executable = base.GetQueryBuilderReturning();

            return executable;
        }

        public override Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            return base.BasicUpdate<T>(properties, values);
        }

        public override Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, T entity)
        {
            return base.BasicUpdate<T>(properties, entity);
        }

        public override Interfaces.IQueryBuilderWhere Delete<T>()
        {
            return base.BasicDelete<T>();
        }

        public override Interfaces.IQueryBuilderSpExecutable StoredProcedure<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            PropertiesInfo propertiesInfo = base.GetSentences<T>(properties, values);

            base._query = $"CALL {propertiesInfo.EntityName} ({string.Join(", ", propertiesInfo.Sentences)})";

            var executable = base.GetQueryBuilderSpExecutable();

            return executable;
        }

        public override Interfaces.IQueryBuilderTableFunctionExecutable TableFunction<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            PropertiesInfo propertiesInfo = base.GetSentences<T>(properties, values);

            base._query = $"SELECT * FROM TABLE({propertiesInfo.EntityName} ({string.Join(", ", propertiesInfo.Sentences)}))";

            var executable = base.GetQueryBuilderTableFunctionExecutable();

            return executable;
        }

        public override Interfaces.IQueryBuilderScalarFunctionExecutable ScalarFunction<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            PropertiesInfo propertiesInfo = base.GetSentences<T>(properties, values);

            base._query = $"SELECT {propertiesInfo.EntityName} ({string.Join(", ", propertiesInfo.Sentences)}) FROM DUAL";

            var executable = base.GetQueryBuilderScalarFunctionExecutable();

            return executable;
        }
    }
}
