using System;
using System.Linq.Expressions;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderJoin : IQueryBuilderWhereGroupByOrderBy
    {
        /// <summary>
        /// Introduces a "CAST(Property AS xxx)" statement over a property that has been set on Select() statement.
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the CAST statement</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the statement</param>
        /// <param name="castType">Type for CAST(Property AS xxx)</param>
        /// <param name="alias">The alias used on the Select() statement</param>
        IQueryBuilderJoin Cast<T>(Expression<Func<T, object>> property, string castType, string alias = null);

        /// <summary>
        /// Constructs the INNER JOIN statement<br/>
        /// Usage:<br/>
        /// .InnerJoin&lt;Entity1, Entity2&gt;(e1 => e1.Prop, e2 => e2.Prop, e2 => new { e2.Prop1, e2.Prop2, ... })
        /// </summary>
        /// <typeparam name="T1">Type of the source Entity/Table to join to</typeparam>
        /// <typeparam name="T2">Type of the target Entity/Table to be joinned</typeparam>
        /// <param name="propertyEntity1">Lambda Expression for the source Entity Property (Table Column) for the left side of the ON clause</param>
        /// <param name="propertyEntity2">Lambda Expression for the target Entity Property (Table Column) for the right side of the ON clause</param>
        /// <param name="properties">Lambda Expression for the target Entity Properties (Table Columns) to be included on the SELECT statement</param>
        /// <param name="sourceAlias">The "AS" alias statement of the source Entity/Table when that table has alias</param>
        /// <param name="targetAlias">The "AS" alias statement of the target Entity/Table when same table is joinned more than once</param>
        IQueryBuilderJoin InnerJoin<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>> properties = null, string sourceAlias = null, string targetAlias = null);

        /// <summary>
        /// Constructs the LEFT JOIN statement<br/>
        /// Usage:<br/>
        /// .LeftJoin&lt;Entity1, Entity2&gt;(e1 => e1.Prop, e2 => e2.Prop, e2 => new { e2.Prop1, e2.Prop2, ... })
        /// </summary>
        /// <typeparam name="T1">Type of the source Entity/Table to join to</typeparam>
        /// <typeparam name="T2">Type of the target Entity/Table to be joinned</typeparam>
        /// <param name="propertyEntity1">Lambda Expression for the source Entity Property (Table Column) for the left side of the ON clause</param>
        /// <param name="propertyEntity2">Lambda Expression for the target Entity Property (Table Column) for the right side of the ON clause</param>
        /// <param name="properties">Lambda Expression for the target Entity Properties (Table Columns) to be included on the SELECT statement</param>
        /// <param name="sourceAlias">The "AS" alias statement of the source Entity/Table when that table has alias</param>
        /// <param name="targetAlias">The "AS" alias statement of the target Entity/Table when same table is joinned more than once</param>
        IQueryBuilderJoin LeftJoin<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>> properties = null, string sourceAlias = null, string targetAlias = null);
    }
}
