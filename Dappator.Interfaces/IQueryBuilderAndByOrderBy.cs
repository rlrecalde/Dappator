using System;
using System.Linq.Expressions;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderAndByOrderBy : IQueryBuilderQuery, IQueryBuilderOrderBy
    {
        /// <summary>
        /// Adds an extra GROUP clause after a GROUP BY<br/>
        /// Usage:<br/>
        /// .AndBy&lt;Entity&gt;(x => x.Prop)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the GROUP BY</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the GROUP</param>
        /// <param name="alias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        IQueryBuilderAndByOrderBy AndBy<T>(Expression<Func<T, object>> property, string alias = null);
    }
}
