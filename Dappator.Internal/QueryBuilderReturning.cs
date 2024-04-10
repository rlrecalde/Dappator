using System;
using System.Linq.Expressions;

namespace Dappator.Internal
{
    internal class QueryBuilderReturning : QueryBuilderExecuteScalar, Interfaces.IQueryBuilderReturning
    {
        public QueryBuilderReturning(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
        }

        public Interfaces.IQueryBuilderExecuteScalar Returning<T>(Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentException(Constants.PropertyNotNull);

            base.ValidatePropertyIsObject<T>("property", property);

            string oldReturning = "RETURNING CAST(Id";
            if (base._query.IndexOf(oldReturning) == -1)
                return this;

            EntityInfo entityInfo = base.GetEntityInfoFromObjectExpression<T>(property.Body);
            string newReturning = $"RETURNING CAST({entityInfo.PropertyDbNames[0]}";

            base._query = base._query.Replace(oldReturning, newReturning);

            return this;
        }
    }
}
