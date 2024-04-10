using System;
using System.Linq.Expressions;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderAggregate
    {
        /// <summary>
        /// Introduces a COUNT() function into the SELECT statement<br/>
        /// Usage:<br/>
        /// .Count&lt;Entity&gt;(x => x.Prop)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the COUNT(Property)</param>
        /// <param name="distinct">Boolean for the COUNT(DISTINCT Property)</param>
        /// <param name="cast">Type for CAST(COUNT(Property) AS xxx)</param>
        /// <param name="alias">The "AS" alias statement for the COUNT(Property) AS 'alias'</param>
        /// <param name="tableAlias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        void Count<T>(Expression<Func<T, object>> property, bool distinct = false, string cast = null, string alias = null, string tableAlias = null);

        /// <summary>
        /// Introduces a MAX() function into the SELECT statement<br/>
        /// Usage:<br/>
        /// .Max&lt;Entity&gt;(x => x.Prop)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the MAX(Property)</param>
        /// <param name="cast">Type for CAST(MAX(Property) AS xxx)</param>
        /// <param name="alias">The "AS" alias statement for the MAX(Property) AS 'alias'</param>
        /// <param name="tableAlias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        void Max<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null);

        /// <summary>
        /// Introduces a MIN() function into the SELECT statement<br/>
        /// Usage:<br/>
        /// .Min&lt;Entity&gt;(x => x.Prop)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the MIN(Property)</param>
        /// <param name="cast">Type for CAST(MIN(Property) AS xxx)</param>
        /// <param name="alias">The "AS" alias statement for the MIN(Property) AS 'alias'</param>
        /// <param name="tableAlias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        void Min<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null);

        /// <summary>
        /// Introduces a SUM() function into the SELECT statement<br/>
        /// Usage:<br/>
        /// .Sum&lt;Entity&gt;(x => x.Prop)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the SUM(Property)</param>
        /// <param name="cast">Type for CAST(SUM(Property) AS xxx)</param>
        /// <param name="alias">The "AS" alias statement for the SUM(Property) AS 'alias'</param>
        /// <param name="tableAlias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        void Sum<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null);

        /// <summary>
        /// Introduces an AVG() function into the SELECT statement<br/>
        /// Usage:<br/>
        /// .Avg&lt;Entity&gt;(x => x.Prop)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the AVG(Property)</param>
        /// <param name="cast">Type for CAST(AVG(Property) AS xxx)</param>
        /// <param name="alias">The "AS" alias statement for the AVG(Property) AS 'alias'</param>
        /// <param name="tableAlias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        void Avg<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null);
    }
}
