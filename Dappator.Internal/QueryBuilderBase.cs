using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Dappator.Internal
{
    internal abstract class QueryBuilderBase
    {
        protected Dictionary<Common.Operators, string> _operators;
        protected string _query;
        protected int _parameterCounter;
        protected object[] _values;
        protected Type[] _types;
        protected IList<EntityInfo> _entityInfos;
        protected DbType _dbType;
        protected DbConnection _dbConnection;
        protected DbTransaction _dbTransaction;
        protected int? _commandTimeout;
        protected bool _executeInTransaction;
        protected System.Data.IsolationLevel? _transactionIsolationLevel;
        protected bool _buffered;

        public enum DbType { Sql, Sqlite, Npgsql, MySql, Oracle };

        public string StringQuery { get { return this._query; } }

        public int ParameterCounter { get { return this._parameterCounter; } }

        public object[] Values { get { return this._values; } }

        public Type[] Types { get { return this._types; } }

        public IList<EntityInfo> EntityInfos { get { return this._entityInfos; } }

        public DbType DbConnectionType { get { return this._dbType; } }

        public DbConnection DbConnection { get { return this._dbConnection; } }

        public DbTransaction DbTransaction { get { return this._dbTransaction; } }

        public int? CommandTimeout { get { return this._commandTimeout; } }

        public bool ExecuteInTransaction 
        { 
            get 
            { 
                return this._executeInTransaction; 
            } 
            set 
            { 
                this._executeInTransaction = value;

                if (this._executeInTransaction == false)
                    this._dbTransaction = null;
            } 
        }

        public System.Data.IsolationLevel? TransactionIsolationLevel { get { return this._transactionIsolationLevel; } set { this._transactionIsolationLevel = value; } }

        public bool Buffered { get { return this._buffered; } set { this._buffered = value; } }

        public QueryBuilderBase()
        {
            this._query = string.Empty;
            this._buffered = true;
            this._entityInfos = new List<EntityInfo>();

            this._operators = new Dictionary<Common.Operators, string>
            {
                { Common.Operators.EqualTo, "=" },
                { Common.Operators.NotEqualTo, "<>" },
                { Common.Operators.GreaterThan, ">" },
                { Common.Operators.GreaterThanOrEqualTo, ">=" },
                { Common.Operators.LessThan, "<" },
                { Common.Operators.LessThanOrEqualTo, "<=" },
                { Common.Operators.IsNull, "IS NULL" },
                { Common.Operators.IsNotNull, "IS NOT NULL" },
                { Common.Operators.In, "IN" },
                { Common.Operators.Like, "LIKE" },
                { Common.Operators.Between, "BETWEEN" },
            };
        }

        protected T ExceptionFor<T>(Func<T> execution)
        {
            try
            {
                return execution.Invoke();
            }
            catch (Exception ex)
            {
                var dappatorException = new DappatorException(ex, this._query);

                throw dappatorException;
            }
        }

        protected async Task<T> ExceptionForAsync<T>(Func<Task<T>> execution)
        {
            try
            {
                return await execution();
            }
            catch (Exception ex)
            {
                var dappatorException = new DappatorException(ex, this._query);

                throw dappatorException;
            }
        }

        protected void SetEntityInfo<T>(bool validateDescriptionAttribute = true)
        {
            Type entityType = typeof(T);

            if (this._entityInfos.Any(x => x.EntityType == entityType))
                return;

            var entity = Activator.CreateInstance(entityType);
            string entityName = entity.GetType().Name;
            var attributes = (DescriptionAttribute[])entity.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (!attributes.Any())
                throw new ArgumentException(Constants.ClassDescriptionAttribute.Replace("{entityName}", entityName));

            string entityDbName = attributes[0].Description;

            var propertyInfos = entity.GetType().GetProperties();
            int propertyInfosLength = propertyInfos.Length;

            var entityInfo = new EntityInfo
            {
                Entity = entity,
                EntityDbName = entityDbName,
                EntityType = entityType,
                PropertyNames = new string[propertyInfosLength],
                PropertyDbNames = new string[propertyInfosLength],
                PropertyTypes = new Type[propertyInfosLength],
            };

            for (int i = 0; i < propertyInfosLength; i++)
            {
                entityInfo.PropertyNames[i] = propertyInfos[i].Name;
                entityInfo.PropertyTypes[i] = propertyInfos[i].PropertyType;

                DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])propertyInfos[i].GetCustomAttributes(typeof(DescriptionAttribute));

                if (!descriptionAttributes.Any() && validateDescriptionAttribute)
                    throw new ArgumentException(Constants.PropertyDescriptionAttribute.Replace("{propertyName}", propertyInfos[i].Name));

                entityInfo.PropertyDbNames[i] = descriptionAttributes.Any() ? descriptionAttributes[0].Description : string.Empty;
            }

            this._entityInfos.Add(entityInfo);
        }

        protected EntityInfo GetEntityInfoFromDynamicExpression<T>(Expression propertiesExpression)
        {
            EntityInfo entityInfo = this._entityInfos.FirstOrDefault(x => x.EntityType == typeof(T));

            if (propertiesExpression == null)
                return entityInfo;

            var newExpression = (NewExpression)propertiesExpression;
            int argumentsCount = newExpression.Arguments.Count();

            EntityInfo entityInfoToReturn = new EntityInfo
            {
                Entity = entityInfo.Entity,
                EntityDbName = entityInfo.EntityDbName,
                EntityType = entityInfo.EntityType,
                PropertyNames = new string[argumentsCount],
                PropertyDbNames = new string[argumentsCount],
                PropertyTypes = new Type[argumentsCount],
            };

            int propertyNamesLength = entityInfo.PropertyNames.Length;
            for (int i = 0; i < argumentsCount; i++)
            {
                var argument = (MemberExpression)newExpression.Arguments[i];

                for (int j = 0; j < propertyNamesLength; j++)
                {
                    if (argument.Member.Name != entityInfo.PropertyNames[j])
                        continue;

                    entityInfoToReturn.PropertyNames[i] = entityInfo.PropertyNames[j];
                    entityInfoToReturn.PropertyDbNames[i] = entityInfo.PropertyDbNames[j];
                    entityInfoToReturn.PropertyTypes[i] = entityInfo.PropertyTypes[j];
                    break;
                }

                this.ValidateProperty(entityInfoToReturn.PropertyTypes[i], entityInfoToReturn.PropertyNames[i]);
            }

            return entityInfoToReturn;
        }

        protected EntityInfo GetEntityInfoFromObjectExpression<T>(Expression propertyExpression)
        {
            EntityInfo entityInfo = this._entityInfos.FirstOrDefault(x => x.EntityType == typeof(T));

            MemberExpression memberExpression;
            try
            {
                var unaryExpression = (UnaryExpression)propertyExpression;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            catch (Exception)
            {
                memberExpression = (MemberExpression)propertyExpression;
            }

            EntityInfo entityInfoToReturn = new EntityInfo
            {
                Entity = entityInfo.Entity,
                EntityDbName = entityInfo.EntityDbName,
                EntityType = entityInfo.EntityType,
                PropertyNames = new string[1],
                PropertyDbNames = new string[1],
                PropertyTypes = new Type[1],
            };

            int propertyNamesLength = entityInfo.PropertyNames.Length;
            for (int j = 0; j < propertyNamesLength; j++)
            {
                if (memberExpression.Member.Name != entityInfo.PropertyNames[j])
                    continue;

                entityInfoToReturn.PropertyNames[0] = entityInfo.PropertyNames[j];
                entityInfoToReturn.PropertyDbNames[0] = entityInfo.PropertyDbNames[j];
                entityInfoToReturn.PropertyTypes[0] = entityInfo.PropertyTypes[j];
                break;
            }

            this.ValidateProperty(entityInfoToReturn.PropertyTypes[0], entityInfoToReturn.PropertyNames[0]);

            return entityInfoToReturn;
        }

        protected EntityInfo GetEntity<T>()
        {
            EntityInfo entityInfo = this._entityInfos.FirstOrDefault(x => x.EntityType == typeof(T));

            return entityInfo;
        }

        protected object[] GetValuesFromEntity<T>(T entity, string[] propertyNames)
        {
            object[] values = new object[propertyNames.Length];
            Type typeOfEntity = typeof(T);

            for (int i = 0; i < propertyNames.Length; i++)
                values[i] = typeOfEntity.GetProperty(propertyNames[i]).GetValue(entity);

            return values;
        }

        protected string GetFields(string table, string[] propertyDbNames)
        {
            string[] aFields = new string[propertyDbNames.Length];

            for (int i = 0; i < propertyDbNames.Length; i++)
                aFields[i] = $"{table}.{propertyDbNames[i]}";

            string fields = string.Join(", ", aFields);

            return fields;
        }

        protected string[] GetStringValues(object[] values, Type[] propertyTypes, bool isBulk = false)
        {
            string[] stringValues = new string[!isBulk ? propertyTypes.Length : values.Length];

            if (!isBulk)
            {
                for (int i = 0; i < propertyTypes.Length; i++)
                {
                    Type propertyType = propertyTypes[i];
                    stringValues[i] = this.GetStringValue(values[i], propertyType.IsArray);
                }
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    string[] bulkStringValues = new string[propertyTypes.Length];
                    object[] valuesInValues = (object[])values[i];

                    for (int j = 0; j < propertyTypes.Length; j++)
                    {
                        Type propertyType = propertyTypes[j];
                        bulkStringValues[j] = this.GetStringValue(valuesInValues[j], propertyType.IsArray);
                    }

                    stringValues[i] = $"({string.Join(", ", bulkStringValues)})";
                }
            }

            return stringValues;
        }

        protected string GetStringValue(object value, bool propertyIsArray = false)
        {
            Type typeOfValue = value?.GetType();

            if ((typeOfValue != null && (!typeOfValue.IsArray || (typeOfValue.IsArray && propertyIsArray))) || typeOfValue == null)
            {
                string paramChar = "@";
                if (this._dbType == DbType.Oracle)
                    paramChar = ":";

                string parameter = $"{paramChar}p{this._parameterCounter}";
                this._parameterCounter++;

                return parameter;
            }

            var valueArray = (IEnumerable)value;
            string[] stringValues = new string[((Array)valueArray).Length];
            int i = 0;

            foreach (var valueInArray in valueArray)
            {
                stringValues[i] = this.GetStringValue(valueInArray);
                i++;
            }

            string stringValue = string.Join(", ", stringValues);

            return $"({stringValue})";
        }

        protected void SetValues(object[] values)
        {
            if (this._values == null)
                this._values = new object[] { };

            foreach (object value in values)
            {
#if NET471_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
                this._values = this._values.Append(this.SetValue(value)).ToArray();
#else
                IList<object> valuesCopy = this._values.ToList();
                valuesCopy.Add(this.SetValue(value));
                this._values = valuesCopy.ToArray();
#endif
            }
        }

        protected void SetTypes(Type[] types)
        {
            if (this._types == null)
                this._types = new Type[] { };

            foreach (var type in types)
            {
#if NET471_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
                this._types = this._types.Append(type).ToArray();
#else
                IList<Type> typesCopy = this._types.ToList();
                typesCopy.Add(type);
                this._types = typesCopy.ToArray();
#endif
            }
        }

        protected void ValidatePropertiesIsAnonymous<T>(Expression<Func<T, dynamic>> properties = null)
        {
            this.BasicValidatePropertiesIsAnonymous<T>(properties);
        }

        protected void ValidatePropertiesIsAnonymousAndNotEmpty<T>(Expression<Func<T, dynamic>> properties = null)
        {
            this.BasicValidatePropertiesIsAnonymous<T>(properties);

            if (properties == null)
                return;

            if (!((NewExpression)properties.Body).Arguments.Any())
                throw new ArgumentException(Constants.PropertiesAnonymousWithProperties);
        }

        protected void ValidatePropertyIsObject<T>(string propertyName, Expression<Func<T, object>> property = null)
        {
            if (property == null)
                return;

            if (property.Body.Type.Name.Contains("Anonymous"))
                throw new ArgumentException(Constants.PropertyForm.Replace("{propertyName}", propertyName));
        }

        public class EntityInfo
        {
            public object Entity { get; set; }

            public string EntityDbName { get; set; }

            public Type EntityType { get; set; }

            public string[] PropertyNames { get; set; }

            public string[] PropertyDbNames { get; set; }

            public Type[] PropertyTypes { get; set; }
        }

        #region Private Methods

        private object SetValue(object value)
        {
            if (value == null)
                return value;

            Type typeOfValue = value.GetType();

            if (!typeOfValue.IsArray)
                return value;

            var valueArray = (IEnumerable)value;
            var values = new object[((Array)valueArray).Length];
            int i = 0;

            foreach (var valueInArray in valueArray)
            {
                values[i] = valueInArray;
                i++;
            }

            return values;
        }

        private void BasicValidatePropertiesIsAnonymous<T>(Expression<Func<T, dynamic>> properties = null)
        {
            if (properties == null)
                return;

            if (!properties.Body.Type.Name.Contains("Anonymous"))
                throw new ArgumentException(Constants.PropertiesAnonymous);
        }

        private void ValidateProperty(Type typeOfProperty, string propertyName)
        {
            bool isValueType = typeOfProperty.IsValueType;
            bool isString = typeOfProperty == typeof(string);
            bool isArray = typeOfProperty.IsArray;
            Type elementType = typeOfProperty.GetElementType();
            bool isArrayOfValueType = isArray && (elementType != null && elementType.IsValueType);
            bool isArrayOfString = isArray && (elementType != null && elementType == typeof(string));

            if (!isValueType && !isString && !isArrayOfValueType && !isArrayOfString)
                throw new ArgumentException(Constants.PropertyPrimitive.Replace("{propertyName}", propertyName));
        }

        #endregion
    }
}
