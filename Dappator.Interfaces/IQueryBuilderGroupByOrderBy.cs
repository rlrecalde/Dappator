using System;
using System.Linq.Expressions;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderGroupByOrderBy : IQueryBuilderOrderBy
    {
        /// <summary>
        /// Constructs a GROUP BY clause<br/>
        /// Usage:<br/>
        /// .GroupBy&lt;Entity&gt;(x => x.Prop)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the GROUP BY</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the GROUP BY</param>
        /// <param name="alias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        IQueryBuilderAndByOrderBy GroupBy<T>(Expression<Func<T, object>> property, string alias = null);
    }
}
