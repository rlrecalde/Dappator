using System;
using System.Linq.Expressions;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderWhereGroupByOrderBy : IQueryBuilderQuery, IQueryBuilderGroupByOrderBy
    {
        /// <summary>
        /// Constructs the WHERE clause<br/>
        /// Usage:<br/>
        /// .Where&lt;Entity&gt;(x => x.Prop, Operators.EqualTo, 5)<br/>
        /// (In case 'Operators.In' is used, 'value' parameter has to be an Array)<br/>
        /// (In case 'Operators.Like' is used, 'value' parameter has include '%' symbols)<br/>
        /// (In case 'Operators.IsNull' or 'Operators.IsNotNull' is used, 'value' parameter is dismissed)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the WHERE condition</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the condition</param>
        /// <param name="op">Condition Operator</param>
        /// <param name="value">Condition Value</param>
        /// <param name="valueTo">Condition Value when using 'Between' operator</param>
        /// <param name="alias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        IQueryBuilderAndOrGroupByOrderBy Where<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, string alias = null);
    }
}
