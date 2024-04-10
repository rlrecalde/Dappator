using System;
using System.Linq.Expressions;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderOrderBy
    {
        /// <summary>
        /// Constructs an ORDER BY clause<br/>
        /// Usage:<br/>
        /// .OrderBy&lt;Entity&gt;(x => x.Prop, Orders.Descending)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the ORDER BY</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the ORDER BY</param>
        /// <param name="order">Ascending or Descending</param>
        /// <param name="alias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        IQueryBuilderLimitOffsetThenBy OrderBy<T>(Expression<Func<T, object>> property, Common.Orders order, string alias = null);

        /// <summary>
        /// Constructs an ORDER BY clause<br/>
        /// Usage:<br/>
        /// .OrderBy&lt;Entity&gt;("alias", Orders.Descending)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the ORDER BY</typeparam>
        /// <param name="alias">The Alias used in the COUNT(x) AS 'xxx'.</param>
        /// <param name="order">Ascending or Descending</param>
        IQueryBuilderLimitOffsetThenBy OrderBy<T>(string alias, Common.Orders order);
    }
}
