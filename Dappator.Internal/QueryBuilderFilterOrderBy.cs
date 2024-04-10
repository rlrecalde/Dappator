using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Dappator.Internal
{
    internal class QueryBuilderFilterOrderBy : QueryBuilderFilterBase, Interfaces.IQueryBuilderJoin, Interfaces.IQueryBuilderWhereGroupByOrderBy, Interfaces.IQueryBuilderAndOrGroupByOrderBy, Interfaces.IQueryBuilderAndByOrderBy, Interfaces.IQueryBuilderLimitOffsetThenBy
    {
        private IDictionary<DbType, Func<int, int, Interfaces.IQueryBuilderQuery>> _limitOffsetTypes;

        public QueryBuilderFilterOrderBy(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
            this._limitOffsetTypes = new Dictionary<DbType, Func<int, int, Interfaces.IQueryBuilderQuery>>
            {
                { DbType.Sql, this.GetQueryBuilderLimitOffsetSql },
                { DbType.Sqlite, this.GetQueryBuilderLimitOffsetSqlite },
                { DbType.Npgsql, this.GetQueryBuilderLimitOffsetSqlite },
                { DbType.MySql, this.GetQueryBuilderLimitOffsetMySql },
                { DbType.Oracle, this.GetQueryBuilderLimitOffsetSql },
            };
        }

        public Interfaces.IQueryBuilderJoin Cast<T>(Expression<Func<T, object>> property, string castType, string alias = null)
        {
            if (string.IsNullOrEmpty(castType))
                throw new ArgumentException(Constants.CastNotNullOrEmpty);

            EntityInfo entityInfo = this.GetEntityInfoFromObjectExpression<T>("property", property);

            string table = string.IsNullOrEmpty(alias) ? entityInfo.EntityDbName : alias;
            string field = $"{table}.{entityInfo.PropertyDbNames[0]}";

            if (base._query.IndexOf(field) == -1)
                throw new ArgumentException(Constants.CastMatchProperty);

            string fieldWithCast = $"CAST({field} AS {castType}) AS {entityInfo.PropertyDbNames[0]}";

            var regex = new Regex(Regex.Escape(field));
            base._query = regex.Replace(base._query, fieldWithCast, 1);

            return this;
        }

        public Interfaces.IQueryBuilderJoin InnerJoin<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>> properties = null, string sourceAlias = null, string targetAlias = null)
        {
            JoinData joinData = this.GetJoinData<T1, T2>(propertyEntity1, propertyEntity2, properties, sourceAlias, targetAlias);

            base._query += $" INNER JOIN {joinData.EntityName2}{joinData.Table2Alias} ON {joinData.Table1}.{joinData.PropertyDbName1} = {joinData.Table2}.{joinData.PropertyDbName2}";

            return this;
        }

        public Interfaces.IQueryBuilderJoin LeftJoin<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>> properties = null, string sourceAlias = null, string targetAlias = null)
        {
            JoinData joinData = this.GetJoinData<T1, T2>(propertyEntity1, propertyEntity2, properties, sourceAlias, targetAlias);

            this._query += $" LEFT JOIN {joinData.EntityName2}{joinData.Table2Alias} ON {joinData.Table1}.{joinData.PropertyDbName1} = {joinData.Table2}.{joinData.PropertyDbName2}";

            return this;
        }

        public Interfaces.IQueryBuilderAndOrGroupByOrderBy Where<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, string alias = null)
        {
            base.BasicWhere<T>(property, op, value, valueTo, alias);

            return this;
        }

        public Interfaces.IQueryBuilderAndOrGroupByOrderBy And<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false, string alias = null)
        {
            base.BasicAnd<T>(property, op, value, valueTo, openParenthesisBeforeCondition, closeParenthesisAfterCondition, alias);

            return this;
        }

        public Interfaces.IQueryBuilderAndOrGroupByOrderBy Or<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false, string alias = null)
        {
            base.BasicOr<T>(property, op, value, valueTo, openParenthesisBeforeCondition, closeParenthesisAfterCondition, alias);

            return this;
        }

        public Interfaces.IQueryBuilderAndOrGroupByOrderBy CloseParenthesis()
        {
            base.BasicCloseParenthesis();

            return this;
        }

        public Interfaces.IQueryBuilderAndByOrderBy GroupBy<T>(Expression<Func<T, object>> property, string alias = null)
        {
            EntityInfo entityInfo = this.GetEntityInfoFromObjectExpression<T>("property", property);

            string table = string.IsNullOrEmpty(alias) ? entityInfo.EntityDbName : alias;

            base._query += $" GROUP BY {table}.{entityInfo.PropertyDbNames[0]}";

            return this;
        }

        public Interfaces.IQueryBuilderAndByOrderBy AndBy<T>(Expression<Func<T, object>> property, string alias = null)
        {
            EntityInfo entityInfo = this.GetEntityInfoFromObjectExpression<T>("property", property);

            string table = string.IsNullOrEmpty(alias) ? entityInfo.EntityDbName : alias;

            base._query += $", {table}.{entityInfo.PropertyDbNames[0]}";

            return this;
        }

        public Interfaces.IQueryBuilderLimitOffsetThenBy OrderBy<T>(Expression<Func<T, object>> property, Common.Orders order, string alias = null)
        {
            OrderByData orderByData = this.GetOrderByData<T>(property, order);
            string table = string.IsNullOrEmpty(alias) ? orderByData.EntityName : alias;

            base._query += $" ORDER BY {table}.{orderByData.PropertyDbName} {orderByData.Order}";

            return this;
        }

        public Interfaces.IQueryBuilderLimitOffsetThenBy OrderBy<T>(string alias, Common.Orders order)
        {
            if (string.IsNullOrEmpty(alias))
                throw new ArgumentException(Constants.AliasNotNullOrEmpty);

            string orderString = (order == Common.Orders.Ascending) ? "ASC" : "DESC";

            base._query += $" ORDER BY {alias} {orderString}";

            return this;
        }

        public Interfaces.IQueryBuilderLimitOffsetThenBy ThenBy<T>(Expression<Func<T, object>> property, Common.Orders order, string alias = null)
        {
            OrderByData orderByData = this.GetOrderByData<T>(property, order);
            string table = string.IsNullOrEmpty(alias) ? orderByData.EntityName : alias;

            base._query += $", {table}.{orderByData.PropertyDbName} {orderByData.Order}";

            return this;
        }

        public Interfaces.IQueryBuilderLimitOffsetThenBy ThenBy<T>(string alias, Common.Orders order)
        {
            if (string.IsNullOrEmpty(alias))
                throw new ArgumentException(Constants.AliasNotNullOrEmpty);

            string orderString = (order == Common.Orders.Ascending) ? "ASC" : "DESC";

            base._query += $", {alias} {orderString}";

            return this;
        }

        public Interfaces.IQueryBuilderQuery LimitOffset(int limit, int offset = 0)
        {
            var limitOffsetType = this._limitOffsetTypes[base._dbType];
            Interfaces.IQueryBuilderQuery queryBuilderExecutable = limitOffsetType.Invoke(limit, offset);

            return queryBuilderExecutable;
        }

        #region Private Methods

        private JoinData GetJoinData<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>> properties = null, string sourceAlias = null, string targetAlias = null)
        {
            base.SetEntityInfo<T2>();

            EntityInfo entityInfo1 = this.GetEntityInfoFromObjectExpression<T1>("propertyEntity1", propertyEntity1);
            EntityInfo entityInfo2 = this.GetEntityInfoFromObjectExpression<T2>("propertyEntity2", propertyEntity2);

            base.ValidatePropertiesIsAnonymous<T2>(properties);

            string table1 = string.IsNullOrEmpty(sourceAlias) ? entityInfo1.EntityDbName : sourceAlias;
            string table2 = string.IsNullOrEmpty(targetAlias) ? entityInfo2.EntityDbName : targetAlias;

            if (properties != null && ((NewExpression)properties.Body).Arguments.Any())
            {
                EntityInfo entityInfo = base.GetEntityInfoFromDynamicExpression<T2>(properties.Body);

                string fields = base.GetFields(table2, entityInfo.PropertyDbNames);
                int fromIndex = base._query.IndexOf("FROM");
                string fieldsToInsert = $", {fields}";
                base._query = base._query.Insert(fromIndex - 1, fieldsToInsert);
            }

            string table2Alias = string.IsNullOrEmpty(targetAlias) ? "" : (" AS " + targetAlias);

            var joinData = new JoinData
            {
                EntityName2 = entityInfo2.EntityDbName,
                Table2Alias = table2Alias,
                Table1 = table1,
                Table2 = table2,
                PropertyDbName1 = entityInfo1.PropertyDbNames[0],
                PropertyDbName2 = entityInfo2.PropertyDbNames[0],
            };

            return joinData;
        }

        private OrderByData GetOrderByData<T>(Expression<Func<T, object>> property, Common.Orders order)
        {
            EntityInfo entityInfo = this.GetEntityInfoFromObjectExpression<T>("property", property);

            string orderString = (order == Common.Orders.Ascending) ? "ASC" : "DESC";

            var orderByData = new OrderByData
            {
                EntityName = entityInfo.EntityDbName,
                PropertyDbName = entityInfo.PropertyDbNames[0],
                Order = orderString,
            };

            return orderByData;
        }

        private EntityInfo GetEntityInfoFromObjectExpression<T>(string propertyName, Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentException(Constants.PropertyForNameNotNull.Replace("{propertyName}", propertyName));

            base.ValidatePropertyIsObject<T>(propertyName, property);

            EntityInfo entityInfo = base.GetEntityInfoFromObjectExpression<T>(property.Body);

            return entityInfo;
        }

        private Interfaces.IQueryBuilderQuery GetQueryBuilderLimitOffsetSql(int limit, int offset = 0)
        {
            base._query += $" OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";

            return this;
        }

        private Interfaces.IQueryBuilderQuery GetQueryBuilderLimitOffsetSqlite(int limit, int offset = 0)
        {
            string offsetString = offset > 0 ? $" OFFSET {offset}" : "";
            
            base._query += $" LIMIT {limit}{offsetString}";

            return this;
        }

        private Interfaces.IQueryBuilderQuery GetQueryBuilderLimitOffsetMySql(int limit, int offset = 0)
        {
            string offsetLimit = $"{limit}";
            if (offset > 0)
                offsetLimit = $"{offset}, {limit}";

            base._query += $" LIMIT {offsetLimit}";

            return this;
        }

        #endregion

        class OrderByData
        {
            public string EntityName { get; set; }

            public string PropertyDbName { get; set; }

            public string Order { get; set; }
        }

        class JoinData
        {
            public string EntityName2 { get; set; }

            public string Table2Alias { get; set; }

            public string Table1 { get; set; }

            public string Table2 { get; set; }

            public string PropertyDbName1 { get; set; }

            public string PropertyDbName2 { get; set; }
        }
    }
}
