using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dappator.Internal
{
    internal class QueryBuilderAggregate : QueryBuilderBase, Interfaces.IQueryBuilderAggregate
    {
        private string[] _aggregateQueries;

        public string AggregateQuery { get { return string.Join(", ", this._aggregateQueries); } }

        public QueryBuilderAggregate()
        {
            this._aggregateQueries = new string[] { };
        }

        public void Count<T>(Expression<Func<T, object>> property, bool distinct = false, string cast = null, string alias = null, string tableAlias = null)
        {
            base.SetEntityInfo<T>();
            EntityInfo entityInfo = this.ValidateParametersAndGetEntityInfo<T>(property);

            string distinctString = distinct ? "DISTINCT " : "";
            string aliasString = string.IsNullOrEmpty(alias) ? "" : (" AS " + alias);
            string table = string.IsNullOrEmpty(tableAlias) ? entityInfo.EntityDbName : tableAlias;
            
            string aggregateSentence = $"COUNT({distinctString}{table}.{entityInfo.PropertyDbNames[0]})";
            if (!string.IsNullOrEmpty(cast))
                aggregateSentence = $"CAST({aggregateSentence} AS {cast})";

            string aggregateQuery = $"{aggregateSentence}{aliasString}";

            this.AddAggregateQueryToAggregateQueries(aggregateQuery);
        }

        public void Max<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null)
        {
            base.SetEntityInfo<T>();
            EntityInfo entityInfo = this.ValidateParametersAndGetEntityInfo<T>(property);

            string aliasString = string.IsNullOrEmpty(alias) ? "" : (" AS " + alias);
            string table = string.IsNullOrEmpty(tableAlias) ? entityInfo.EntityDbName : tableAlias;

            string aggregateSentence = $"MAX({table}.{entityInfo.PropertyDbNames[0]})";
            if (!string.IsNullOrEmpty(cast))
                aggregateSentence = $"CAST({aggregateSentence} AS {cast})";

            string aggregateQuery = $"{aggregateSentence}{aliasString}";

            this.AddAggregateQueryToAggregateQueries(aggregateQuery);
        }

        public void Min<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null)
        {
            base.SetEntityInfo<T>();
            EntityInfo entityInfo = this.ValidateParametersAndGetEntityInfo<T>(property);

            string aliasString = string.IsNullOrEmpty(alias) ? "" : (" AS " + alias);
            string table = string.IsNullOrEmpty(tableAlias) ? entityInfo.EntityDbName : tableAlias;

            string aggregateSentence = $"MIN({table}.{entityInfo.PropertyDbNames[0]})";
            if (!string.IsNullOrEmpty(cast))
                aggregateSentence = $"CAST({aggregateSentence} AS {cast})";

            string aggregateQuery = $"{aggregateSentence}{aliasString}";

            this.AddAggregateQueryToAggregateQueries(aggregateQuery);
        }

        public void Sum<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null)
        {
            base.SetEntityInfo<T>();
            EntityInfo entityInfo = this.ValidateParametersAndGetEntityInfo<T>(property);

            string aliasString = string.IsNullOrEmpty(alias) ? "" : (" AS " + alias);
            string table = string.IsNullOrEmpty(tableAlias) ? entityInfo.EntityDbName : tableAlias;

            string aggregateSentence = $"SUM({table}.{entityInfo.PropertyDbNames[0]})";
            if (!string.IsNullOrEmpty(cast))
                aggregateSentence = $"CAST({aggregateSentence} AS {cast})";

            string aggregateQuery = $"{aggregateSentence}{aliasString}";

            this.AddAggregateQueryToAggregateQueries(aggregateQuery);
        }

        public void Avg<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null)
        {
            base.SetEntityInfo<T>();
            EntityInfo entityInfo = this.ValidateParametersAndGetEntityInfo<T>(property);

            string aliasString = string.IsNullOrEmpty(alias) ? "" : (" AS " + alias);
            string table = string.IsNullOrEmpty(tableAlias) ? entityInfo.EntityDbName : tableAlias;

            string aggregateSentence = $"AVG({table}.{entityInfo.PropertyDbNames[0]})";
            if (!string.IsNullOrEmpty(cast))
                aggregateSentence = $"CAST({aggregateSentence} AS {cast})";

            string aggregateQuery = $"{aggregateSentence}{aliasString}";

            this.AddAggregateQueryToAggregateQueries(aggregateQuery);
        }

        #region Private Methods

        private EntityInfo ValidateParametersAndGetEntityInfo<T>(Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentException(Constants.PropertyNotNull);

            base.ValidatePropertyIsObject<T>("property", property);

            EntityInfo entityInfo = base.GetEntityInfoFromObjectExpression<T>(property.Body);

            return entityInfo;
        }

        private void AddAggregateQueryToAggregateQueries(string aggregateQuery)
        {
#if NET471_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
            this._aggregateQueries = this._aggregateQueries.Append(aggregateQuery).ToArray();
#else
            IList<string> aggregateQueriesCopy = this._aggregateQueries.ToList();
            aggregateQueriesCopy.Add(aggregateQuery);
            this._aggregateQueries = aggregateQueriesCopy.ToArray();
#endif
        }

        #endregion
    }
}
