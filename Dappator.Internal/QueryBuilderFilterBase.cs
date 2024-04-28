using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dappator.Internal
{
    internal abstract class QueryBuilderFilterBase : QueryBuilderExecuteAndQuery
    {
        protected QueryBuilderFilterBase(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
        }

        protected void BasicWhere<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, string alias = null)
        {
            FilterData filterData = this.GetFilterData<T>(property, op, value, valueTo, alias);

            base._query += $" WHERE {filterData.Table}.{filterData.PropertyDbName} {filterData.Operator}";

            this.SetValueParameter(filterData.PropertyType, op, value, valueTo);
        }

        protected void BasicAnd<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false, string alias = null)
        {
            FilterData filterData = this.GetFilterData<T>(property, op, value, valueTo, alias);

            string openParenthesis = openParenthesisBeforeCondition ? "(" : "";
            string closeParenthesis = closeParenthesisAfterCondition ? ")" : "";

            base._query += $" AND {openParenthesis}{filterData.Table}.{filterData.PropertyDbName} {filterData.Operator}";

            this.SetValueParameter(filterData.PropertyType, op, value, valueTo);

            base._query += closeParenthesis;
        }

        protected void BasicOr<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false, string alias = null)
        {
            FilterData filterData = this.GetFilterData<T>(property, op, value, valueTo, alias);

            string openParenthesis = openParenthesisBeforeCondition ? "(" : "";
            string closeParenthesis = closeParenthesisAfterCondition ? ")" : "";

            base._query += $" OR {openParenthesis}{filterData.Table}.{filterData.PropertyDbName} {filterData.Operator}";
            
            this.SetValueParameter(filterData.PropertyType, op, value, valueTo);
            
            base._query += closeParenthesis;
        }

        protected void BasicCloseParenthesis()
        {
            base._query += ")";
        }

        #region Private Methods

        private FilterData GetFilterData<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, string alias = null)
        {
            this.ValidateParameters<T>(property, op, value, valueTo);

            EntityInfo entityInfo = base.GetEntityInfoFromObjectExpression<T>(property.Body);

            string oper = base._operators[op];
            string table = string.IsNullOrEmpty(alias) ? entityInfo.EntityDbName : alias;

            var filterData = new FilterData
            {
                PropertyDbName = entityInfo.PropertyDbNames[0],
                PropertyType = entityInfo.PropertyTypes[0],
                Operator = oper,
                Table = table,
            };

            return filterData;
        }

        private void ValidateParameters<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null)
        {
            if (property == null)
                throw new ArgumentException(Constants.PropertyNotNull);

            base.ValidatePropertyIsObject<T>("property", property);

            if ((op == Common.Operators.IsNull || op == Common.Operators.IsNotNull) && (value != null || valueTo != null))
                throw new ArgumentException(Constants.OperatorIsNullValueNotSupplied);

            if (op != Common.Operators.IsNull && op != Common.Operators.IsNotNull && value == null)
                throw new ArgumentException(Constants.OperatorNotIsNullValueSupplied);

            if (op == Common.Operators.Between && valueTo == null)
                throw new ArgumentException(Constants.OperatorBetweenValueToSupplied);

            if (op != Common.Operators.Between && valueTo != null)
                throw new ArgumentException(Constants.OperatorNotBetweenValueToNotSupplied);

            if (value != null && valueTo != null)
            {
                Type typeOfValue = value.GetType();
                Type typeOfValueTo = valueTo.GetType();

                if (typeOfValue != typeOfValueTo)
                    throw new ArgumentException(Constants.OperatorBetweenValueValueToSameType);
            }

            if (value == null)
                return;

            Type propertyType = property.Body.Type;
            Type valueType = value.GetType();

            if (op != Common.Operators.In &&
                ((propertyType.IsArray && !valueType.IsArray) || (!propertyType.IsArray && valueType.IsArray)))
                throw new ArgumentException(Constants.PropertyAndValueArray);

            if (op == Common.Operators.In && !valueType.IsArray)
                throw new ArgumentException(Constants.OperatorInValueArray);

            if (op != Common.Operators.In && propertyType.IsArray && valueType.IsArray)
            {
                Type propertyElementType = propertyType.GetElementType();
                Type valueElementType = valueType.GetElementType();

                if (propertyElementType != typeof(byte) || valueElementType != typeof(byte))
                    throw new ArgumentException(Constants.PropertyAndValueByteArray);
            }
        }

        private void SetValueParameter(Type propertyType, Common.Operators op, object value = null, object valueTo = null)
        {
            if (value == null)
                return;

            string stringValue = base.GetStringValue(value, propertyType.IsArray);
            base._query += $" {stringValue}";

            object[] values = new object[] { value };

            if (valueTo != null)
            {
                string stringValueTo = base.GetStringValue(valueTo, propertyType.IsArray);
                base._query += $" AND {stringValueTo}";

#if NET471_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
                values = values.Append(valueTo).ToArray();
#else
                IList<object> valuesCopy = values.ToList();
                valuesCopy.Add(valueTo);
                values = valuesCopy.ToArray();
#endif
            }

            if (op == Common.Operators.In)
            {
                object[] aValue = (object[])value;
                values = new object[aValue.Length];

                for (int i = 0; i < aValue.Length; i++)
                    values[i] = aValue[i];
            }

            Type[] types = new Type[values.Length];

            for (int i = 0; i < values.Length; i++)
                types[i] = propertyType;

            base.SetTypes(types);
            base.SetValues(values);
        }

        #endregion

        class FilterData
        {
            public string PropertyDbName { get; set; }

            public Type PropertyType { get; set; }

            public string Operator { get; set; }

            public string Table { get; set; }
        }
    }
}
