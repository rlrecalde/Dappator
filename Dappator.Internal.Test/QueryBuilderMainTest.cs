using System.Collections.Generic;

namespace Dappator.Internal.Test
{
    internal class QueryBuilderMainTest : QueryBuilderMain
    {
        public override Interfaces.IQueryBuilderJoin Select<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties, bool distinct = false, string alias = null, System.Action<Interfaces.IQueryBuilderAggregate> aggregate = null)
        {
            return base.BasicSelect<T>(properties, distinct, alias, aggregate);
        }

        public override Interfaces.IQueryBuilderReturning Insert<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties, params object[] values)
        {
            base.BasicInsert<T>(properties, values);

            base._query += " RETURNING CAST(Id AS BIGINT)";

            var executable = base.GetQueryBuilderReturning();

            return executable;
        }

        public override Interfaces.IQueryBuilderReturning Insert<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties, T entity)
        {
            base.BasicInsert<T>(properties, entity);

            base._query += " RETURNING CAST(Id AS BIGINT)";

            var executable = base.GetQueryBuilderReturning();

            return executable;
        }

        public override Interfaces.IQueryBuilderReturning Insert<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties, IEnumerable<T> entities)
        {
            base.BasicInsert<T>(properties, entities);

            base._query += " RETURNING CAST(Id AS BIGINT)";

            var executable = base.GetQueryBuilderReturning();

            return executable;
        }

        public override Interfaces.IQueryBuilderWhere Update<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties, params object[] values)
        {
            return base.BasicUpdate<T>(properties, values);
        }

        public override Interfaces.IQueryBuilderWhere Update<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties, T entity)
        {
            return base.BasicUpdate<T>(properties, entity);
        }

        public override Interfaces.IQueryBuilderWhere Delete<T>()
        {
            return base.BasicDelete<T>();
        }

        public override Interfaces.IQueryBuilderSpExecutable StoredProcedure<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties = null, params object[] values)
        {
            PropertiesInfo propertiesInfo = base.GetSentences<T>(properties, values);

            base._query = $"EXECUTE {propertiesInfo.EntityName} ({string.Join(", ", propertiesInfo.Sentences)})";

            var executable = base.GetQueryBuilderSpExecutable();

            return executable;
        }

        public override Interfaces.IQueryBuilderTableFunctionExecutable TableFunction<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties = null, params object[] values)
        {
            return base.BasicTableFunction<T>(properties, values);
        }

        public override Interfaces.IQueryBuilderScalarFunctionExecutable ScalarFunction<T>(System.Linq.Expressions.Expression<System.Func<T, dynamic>> properties = null, params object[] values)
        {
            return base.BasicScalarFunction<T>(properties, values);
        }
    }
}
