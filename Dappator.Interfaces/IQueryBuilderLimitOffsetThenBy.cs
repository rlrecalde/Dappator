using System;
using System.Linq.Expressions;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderLimitOffsetThenBy : IQueryBuilderQuery
    {
        /// <summary>
        /// Constructs a LIMIT/OFFSET clause for Paging purposes<br/>
        /// Usage:<br/>
        /// .LimitOffset(10, 20)<br/>
        /// (in this example we are getting the third page of ten items)<br/>
        /// .LimitOffset(1)<br/>
        /// (in this example we are getting just the first item (like a SELECT TOP 1))
        /// </summary>
        /// <param name="limit">Limit value</param>
        /// <param name="offset">Offset value</param>
        IQueryBuilderQuery LimitOffset(int limit, int offset = 0);

        /// <summary>
        /// Adds an extra ORDER clause after an ORDER BY<br/>
        /// Usage:<br/>
        /// .ThenBy&lt;Entity&gt;(x => x.Prop, Orders.Descending)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the ORDER BY</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the ORDER</param>
        /// <param name="order">Ascending or Descending</param>
        /// <param name="alias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        IQueryBuilderLimitOffsetThenBy ThenBy<T>(Expression<Func<T, object>> property, Common.Orders order, string alias = null);

        /// <summary>
        /// Adds an extra ORDER clause after an ORDER BY<br/>
        /// Usage:<br/>
        /// .ThenBy&lt;Entity&gt;("alias", Orders.Descending)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the ORDER BY</typeparam>
        /// <param name="order">Ascending or Descending</param>
        /// <param name="alias">The Alias used in the COUNT(x) AS 'xxx'.</param>
        IQueryBuilderLimitOffsetThenBy ThenBy<T>(string alias, Common.Orders order);
    }
}
