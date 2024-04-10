using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace Dappator.Internal
{
    internal abstract class QueryBuilderMain : QueryBuilderBase, IQueryBuilderMain
    {
        public QueryBuilderMain()
        {
        }

        public void SetCommandTimeout(int commandTimeout)
        {
            base._commandTimeout = commandTimeout;
        }

        public DbTransaction GetDbTransaction()
        {
            return base._dbTransaction;
        }

        public Interfaces.IQueryBuilderExecutable SetQuery(string query, params object[] values)
        {
            base._query = query;
            base._values = values;

            var executable = new QueryBuilderExecutable(this);

            return executable;
        }

        public abstract Interfaces.IQueryBuilderJoin Select<T>(Expression<Func<T, dynamic>> properties, bool distinct = false, string alias = null, Action<Interfaces.IQueryBuilderAggregate> aggregate = null);

        public abstract Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, params object[] values);

        public abstract Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, T entity);

        public abstract Interfaces.IQueryBuilderReturning Insert<T>(Expression<Func<T, dynamic>> properties, IEnumerable<T> entities);

        public abstract Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, params object[] values);

        public abstract Interfaces.IQueryBuilderWhere Update<T>(Expression<Func<T, dynamic>> properties, T entity);

        public abstract Interfaces.IQueryBuilderWhere Delete<T>();

        public abstract Interfaces.IQueryBuilderSpExecutable StoredProcedure<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);

        public abstract Interfaces.IQueryBuilderTableFunctionExecutable TableFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);

        public abstract Interfaces.IQueryBuilderScalarFunctionExecutable ScalarFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values);

        protected Interfaces.IQueryBuilderJoin BasicSelect<T>(Expression<Func<T, dynamic>> properties, bool distinct = false, string alias = null, Action<Interfaces.IQueryBuilderAggregate> aggregate = null)
        {
            if (properties == null)
                throw new ArgumentException(Constants.PropertiesNotNull);

            base.ValidatePropertiesIsAnonymous<T>(properties);

            if (!((NewExpression)properties.Body).Arguments.Any() && aggregate == null)
                throw new ArgumentException(Constants.PropertiesEmptyAggregateNotNull);

            this.CleanInternalVariables();

            base.SetEntityInfo<T>();
            EntityInfo entityInfo = base.GetEntityInfoFromDynamicExpression<T>(properties.Body);

            string table = string.IsNullOrEmpty(alias) ? entityInfo.EntityDbName : alias;
            string fields = base.GetFields(table, entityInfo.PropertyDbNames);
            string distinctString = distinct ? "DISTINCT " : "";

            base._query = $"SELECT {distinctString}{fields}";

            if (aggregate != null)
            {
                var queryBuilderAggregate = new QueryBuilderAggregate();
                aggregate.Invoke(queryBuilderAggregate);
                string aggregateQuery = queryBuilderAggregate.AggregateQuery;

                if (entityInfo.PropertyDbNames.Any())
                    base._query += ", ";

                base._query += aggregateQuery;
            }

            string aliasString = string.IsNullOrEmpty(alias) ? "" : (" AS " + alias);
            base._query += $" FROM {entityInfo.EntityDbName}{aliasString}";

            var queryBuilderFilterOrderBy = new QueryBuilderFilterOrderBy(this);

            queryBuilderFilterOrderBy.TransactionEvent += (sender, dbTransaction) =>
            {
                base._dbTransaction = dbTransaction;
            };

            return queryBuilderFilterOrderBy;
        }

        protected void BasicInsert<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            if (properties == null)
                throw new ArgumentException(Constants.PropertiesNotNull);

            base.ValidatePropertiesIsAnonymousAndNotEmpty<T>(properties);

            if (values == null || !values.Any())
                throw new ArgumentException(Constants.ValuesSupplied);

            this.CleanInternalVariables();

            base.SetEntityInfo<T>();
            EntityInfo entityInfo = base.GetEntityInfoFromDynamicExpression<T>(properties.Body);

            bool isBulk = values.All(x => x != null && x.GetType().IsArray && x.GetType().GetElementType() == typeof(object));
            string[] stringValues = base.GetStringValues(values, entityInfo.PropertyTypes, isBulk);

            this.ValidatePropertiesAndValuesTypes(entityInfo.PropertyTypes, values, isBulk);

            base._query = $"INSERT INTO {entityInfo.EntityDbName} ({string.Join(", ", entityInfo.PropertyDbNames)}) VALUES ({string.Join(", ", stringValues)})";

            if (isBulk)
                base._query = $"INSERT INTO {entityInfo.EntityDbName} ({string.Join(", ", entityInfo.PropertyDbNames)}) VALUES {string.Join(", ", stringValues)}";

            base.SetTypes(entityInfo.PropertyTypes);
            base.SetValues(values);
        }

        protected void BasicInsert<T>(Expression<Func<T, dynamic>> properties, T entity)
        {
            if (properties == null)
                throw new ArgumentException(Constants.PropertiesNotNull);

            base.ValidatePropertiesIsAnonymousAndNotEmpty<T>(properties);

            if (entity == null)
                throw new ArgumentException(Constants.EntitySupplied);

            this.CleanInternalVariables();

            base.SetEntityInfo<T>();
            EntityInfo entityInfo = base.GetEntityInfoFromDynamicExpression<T>(properties.Body);

            object[] values = base.GetValuesFromEntity<T>(entity, entityInfo.PropertyNames);
            string[] stringValues = this.GetStringValues(values, entityInfo.PropertyTypes);

            base._query = $"INSERT INTO {entityInfo.EntityDbName} ({string.Join(", ", entityInfo.PropertyDbNames)}) VALUES ({string.Join(", ", stringValues)})";

            base.SetTypes(entityInfo.PropertyTypes);
            base.SetValues(values);
        }

        protected void BasicInsert<T>(Expression<Func<T, dynamic>> properties, IEnumerable<T> entities)
        {
            if (properties == null)
                throw new ArgumentException(Constants.PropertiesNotNull);

            base.ValidatePropertiesIsAnonymousAndNotEmpty<T>(properties);

            if (entities == null || !entities.Any())
                throw new ArgumentException(Constants.EntitySupplied);

            this.CleanInternalVariables();

            base.SetEntityInfo<T>();
            EntityInfo entityInfo = base.GetEntityInfoFromDynamicExpression<T>(properties.Body);

            object[] values = new object[entities.Count()];

            T[] aEntities = entities.ToArray();
            for (int i = 0; i < entities.Count(); i++)
                values[i] = base.GetValuesFromEntity<T>(aEntities[i], entityInfo.PropertyNames);

            string[] stringValues = this.GetStringValues(values, entityInfo.PropertyTypes, true);

            base._query = $"INSERT INTO {entityInfo.EntityDbName} ({string.Join(", ", entityInfo.PropertyDbNames)}) VALUES {string.Join(", ", stringValues)}";

            base.SetTypes(entityInfo.PropertyTypes);
            base.SetValues(values);
        }

        protected Interfaces.IQueryBuilderWhere BasicUpdate<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            if (properties == null)
                throw new ArgumentException(Constants.PropertiesNotNull);

            base.ValidatePropertiesIsAnonymousAndNotEmpty<T>(properties);

            if (values == null || !values.Any())
                throw new ArgumentException(Constants.ValuesSupplied);

            this.CleanInternalVariables();

            base.SetEntityInfo<T>();
            EntityInfo entityInfo = base.GetEntityInfoFromDynamicExpression<T>(properties.Body);
            string[] stringValues = base.GetStringValues(values, entityInfo.PropertyTypes);

            this.ValidatePropertiesAndValuesTypes(entityInfo.PropertyTypes, values);

            string[] sentences = new string[] { };

            for (int i = 0; i < entityInfo.PropertyDbNames.Length; i++)
            {
#if NET471_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
                sentences = sentences.Append(entityInfo.PropertyDbNames[i] + " = " + stringValues[i]).ToArray();
#else
                IList<string> sentencesCopy = sentences.ToList();
                sentencesCopy.Add(entityInfo.PropertyDbNames[i] + " = " + stringValues[i]);
                sentences = sentencesCopy.ToArray();
#endif
            }

            base._query = $"UPDATE {entityInfo.EntityDbName} SET {string.Join(", ", sentences)}";

            base.SetTypes(entityInfo.PropertyTypes);
            base.SetValues(values);

            var queryBuilderFilter = new QueryBuilderFilter(this);
            queryBuilderFilter.TransactionEvent += (sender, dbTransaction) =>
            {
                base._dbTransaction = dbTransaction;
            };

            return queryBuilderFilter;
        }

        protected Interfaces.IQueryBuilderWhere BasicUpdate<T>(Expression<Func<T, dynamic>> properties, T entity)
        {
            if (properties == null)
                throw new ArgumentException(Constants.PropertiesNotNull);

            base.ValidatePropertiesIsAnonymousAndNotEmpty<T>(properties);

            if (entity == null)
                throw new ArgumentException(Constants.EntitySupplied);

            this.CleanInternalVariables();

            base.SetEntityInfo<T>();
            EntityInfo entityInfo = base.GetEntityInfoFromDynamicExpression<T>(properties.Body);

            object[] values = base.GetValuesFromEntity<T>(entity, entityInfo.PropertyNames);
            string[] stringValues = this.GetStringValues(values, entityInfo.PropertyTypes);

            string[] sentences = new string[] { };

            for (int i = 0; i < entityInfo.PropertyDbNames.Length; i++)
            {
#if NET471_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
                sentences = sentences.Append(entityInfo.PropertyDbNames[i] + " = " + stringValues[i]).ToArray();
#else
                IList<string> sentencesCopy = sentences.ToList();
                sentencesCopy.Add(entityInfo.PropertyDbNames[i] + " = " + stringValues[i]);
                sentences = sentencesCopy.ToArray();
#endif
            }

            base._query = $"UPDATE {entityInfo.EntityDbName} SET {string.Join(", ", sentences)}";

            base.SetTypes(entityInfo.PropertyTypes);
            base.SetValues(values);

            var queryBuilderFilter = new QueryBuilderFilter(this);
            queryBuilderFilter.TransactionEvent += (sender, dbTransaction) =>
            {
                base._dbTransaction = dbTransaction;
            };

            return queryBuilderFilter;
        }

        protected Interfaces.IQueryBuilderWhere BasicDelete<T>()
        {
            this.CleanInternalVariables();

            base.SetEntityInfo<T>();
            EntityInfo entityInfo = base.GetEntity<T>();

            base._query = $"DELETE FROM {entityInfo.EntityDbName}";

            var queryBuilderFilter = new QueryBuilderFilter(this);
            queryBuilderFilter.TransactionEvent += (sender, dbTransaction) =>
            {
                base._dbTransaction = dbTransaction;
            };

            return queryBuilderFilter;
        }

        protected Interfaces.IQueryBuilderTableFunctionExecutable BasicTableFunction<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            this.CleanInternalVariables();

            PropertiesInfo propertiesInfo = this.GetBasicSentences<T>(properties, values);

            base._query = $"SELECT * FROM {propertiesInfo.EntityName} ({string.Join(", ", propertiesInfo.Sentences)})";

            var executable = this.GetQueryBuilderTableFunctionExecutable();

            return executable;
        }

        protected Interfaces.IQueryBuilderScalarFunctionExecutable BasicScalarFunction<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            this.CleanInternalVariables();

            PropertiesInfo propertiesInfo = this.GetBasicSentences<T>(properties, values);

            base._query = $"SELECT {propertiesInfo.EntityName} ({string.Join(", ", propertiesInfo.Sentences)})";

            var executable = this.GetQueryBuilderScalarFunctionExecutable();

            return executable;
        }

        protected PropertiesInfo GetSentences<T>(Expression<Func<T, dynamic>> properties = null, params object[] values)
        {
            this.CleanInternalVariables();

            PropertiesInfo propertiesInfo = this.GetBasicSentences<T>(properties, values);

            return propertiesInfo;
        }

        protected Interfaces.IQueryBuilderReturning GetQueryBuilderReturning()
        {
            var queryBuilderReturning = new QueryBuilderReturning(this);
            queryBuilderReturning.TransactionEvent += (sender, dbTransaction) =>
            {
                base._dbTransaction = dbTransaction;
            };

            return queryBuilderReturning;
        }

        protected Interfaces.IQueryBuilderSpExecutable GetQueryBuilderSpExecutable()
        {
            var queryBuilderSpExecutable = new QueryBuilderSpExecutable(this);
            queryBuilderSpExecutable.TransactionEvent += (sender, dbTransaction) =>
            {
                base._dbTransaction = dbTransaction;
            };

            return queryBuilderSpExecutable;
        }

        protected Interfaces.IQueryBuilderTableFunctionExecutable GetQueryBuilderTableFunctionExecutable()
        {
            var queryBuilderTableFunctionExecutable = new QueryBuilderTableFunctionExecutable(this);
            queryBuilderTableFunctionExecutable.TransactionEvent += (sender, dbTransaction) =>
            {
                base._dbTransaction = dbTransaction;
            };

            return queryBuilderTableFunctionExecutable;
        }

        protected Interfaces.IQueryBuilderScalarFunctionExecutable GetQueryBuilderScalarFunctionExecutable()
        {
            var queryBuilderScalarFunctionExecutable = new QueryBuilderScalarFunctionExecutable(this);
            queryBuilderScalarFunctionExecutable.TransactionEvent += (sender, dbTransaction) =>
            {
                base._dbTransaction = dbTransaction;
            };

            return queryBuilderScalarFunctionExecutable;
        }

        protected class PropertiesInfo
        {
            public string EntityName { get; set; }

            public string[] Sentences { get; set; }
        }

        #region Private Methods

        private void ValidatePropertiesAndValuesTypes(Type[] propertyTypes, object[] values, bool isBulk = false)
        {
            if (!isBulk)
            {
                this.ValidateTypes(propertyTypes, values, isBulk);
            }
            else
            {
                foreach (object[] value in values)
                    this.ValidateTypes(propertyTypes, value, isBulk);
            }
        }

        private void ValidateTypes(Type[] propertyTypes, object[] values, bool isBulk)
        {
            if (propertyTypes.Length != values.Length)
                throw new ArgumentException(isBulk ? Constants.ValuesArrayMatchProperties : Constants.ValuesMatchProperties);

            for (int i = 0; i < propertyTypes.Length; i++)
            {
                bool propertyIsArray = propertyTypes[i].IsArray;
                bool valueIsArray = values[i] != null && values[i].GetType().IsArray;

                if ((propertyIsArray && !valueIsArray && values[i] != null) || (!propertyIsArray && valueIsArray))
                    throw new ArgumentException(Constants.ValuesType);

                if (propertyIsArray && valueIsArray)
                {
                    Type propertyElementType = propertyTypes[i].GetElementType();
                    Type valueElementType = values[i].GetType().GetElementType();

                    if (propertyElementType != typeof(byte) || valueElementType != typeof(byte))
                        throw new ArgumentException(Constants.PropertiesAndValuesByteArray);
                }
            }
        }

        private PropertiesInfo GetBasicSentences<T>(Expression<Func<T, dynamic>> properties = null, params object[] values)
        {
            base.ValidatePropertiesIsAnonymous<T>(properties);

            base.SetEntityInfo<T>(validateDescriptionAttribute: false);
            EntityInfo entityInfo = base.GetEntityInfoFromDynamicExpression<T>(properties?.Body);

            string[] sentences = Array.Empty<string>();

            if (properties != null)
            {
                if (!entityInfo.PropertyDbNames.Any())
                    throw new ArgumentException(Constants.PropertiesNotEmptyButNull);

                if (values == null || !values.Any())
                    throw new ArgumentException(Constants.ValuesSupplied);

                string[] stringValues = base.GetStringValues(values, entityInfo.PropertyTypes);

                if (entityInfo.PropertyDbNames.Length != values.Length)
                    throw new ArgumentException(Constants.ValuesMatchProperties);

                sentences = stringValues;

                base.SetTypes(entityInfo.PropertyTypes);
                base.SetValues(values);
            }

            var propertiesInfo = new PropertiesInfo
            {
                EntityName = entityInfo.EntityDbName,
                Sentences = sentences,
            };

            return propertiesInfo;
        }

        private void CleanInternalVariables()
        {
            base._parameterCounter = 0;
            base._values = null;
            base._types = null;
        }

        #endregion
    }
}
