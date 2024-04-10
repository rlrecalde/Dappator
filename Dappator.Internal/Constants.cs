namespace Dappator.Internal
{
    internal static class Constants
    {
        public const string ClassDescriptionAttribute = "Class '{entityName}' must have a Description attribute for the table name.";
        public const string PropertyDescriptionAttribute = "Property '{propertyName}' must have a Description attribute for the column name.";
        public const string PropertyNotNull = "'property' parameter must not be null.";
        public const string PropertyForNameNotNull = "'{propertyName}' parameter must not be null.";
        public const string PropertyForm = "'{propertyName}' parameter must be in the form: x => x.Prop";
        public const string PropertyPrimitive = "The property '{propertyName}' in the lambda expression must be a primitive type (int, string, DateTime, etc...).";
        public const string PropertyAndValueArray = "If 'property' parameter is array type, 'value' parameter must be an array (and viceversa).";
        public const string PropertyAndValueByteArray = "When 'property' and 'value' parameters are arrays, their types must be byte[].";
        public const string PropertiesNotNull = "'properties' parameter must not be null.";
        public const string PropertiesNotEmptyButNull = "'properties' parameter must not be empty (but can be null, of course).";
        public const string PropertiesAnonymous = "'properties' parameter must be an anonymous object.";
        public const string PropertiesAnonymousWithProperties = "'properties' parameter must be an anonymous object with properties.";
        public const string PropertiesEmptyAggregateNotNull = "'properties' parameter can only be an empty anonymous object when 'aggregate' parameter is not null.";
        public const string PropertiesAndValuesByteArray = "When some of the types from 'properties' parameter and 'values' parameter are arrays, their types must be byte[].";
        public const string TPrimitiveNotCollection = "T must not be a collection. It must be a primitive type.";
        public const string TPrimitiveNotClass = "T must not be a class. It must be a primitive type.";
        public const string OperatorIsNullValueNotSupplied = "When using operator 'IsNull' or IsNotNull' you must not supply a 'value' or 'valueTo'.";
        public const string OperatorNotIsNullValueSupplied = "'value' parameter must be supplied.";
        public const string OperatorBetweenValueToSupplied = "'valueTo' parameter must be supplied.";
        public const string OperatorNotBetweenValueToNotSupplied = "'valueTo' parameter must not be supplied.";
        public const string OperatorBetweenValueValueToSameType = "When using 'Between' operator, 'value' and 'valueTo' must be of the same type.";
        public const string OperatorInValueArray = "When using operator 'In',  'value' parameter must be an array.";
        public const string AliasNotNullOrEmpty = "'alias' parameter must not be null or empty.";
        public const string CastNotNullOrEmpty = "'castType' parameter must not be null or empty.";
        public const string CastMatchProperty = "Casted property not found at Select statement.";
        public const string ValuesSupplied = "'values' parameter must be supplied.";
        public const string ValuesMatchProperties = "'values' parameter quantity must match 'properties' parameter.";
        public const string ValuesType = "Some of the types from 'values' parameter do not match some of the types from 'properties' parameter.";
        public const string ValuesArrayMatchProperties = "Some of the array quantity of the array from 'values' parameter do not match 'properties' parameter quantity.";
        public const string EntitySupplied = "'entity' parameter must be supplied.";
        public const string TypeNotAllowed = "Type '{type}' not allowed.";
    }
}
