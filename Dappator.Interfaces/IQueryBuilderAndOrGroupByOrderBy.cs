﻿using System;
using System.Linq.Expressions;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderAndOrGroupByOrderBy : IQueryBuilderQuery, IQueryBuilderGroupByOrderBy
    {
        /// <summary>
        /// Constructs an AND clause to be used with a WHERE clause<br/>
        /// Usage:<br/>
        /// .And&lt;Entity&gt;(x => x.Prop, Operators.EqualTo, 5)<br/>
        /// (In case 'Operators.In' is used, 'value' parameter has to be an Array of objects)<br/>
        /// (In case 'Operators.Like' is used, 'value' parameter has include '%' symbols)<br/>
        /// (In case 'Operators.IsNull' or 'Operators.IsNotNull' is used, 'value' parameter is dismissed)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the AND condition</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the condition</param>
        /// <param name="op">Condition Operator</param>
        /// <param name="openParenthesisBeforeCondition">Specifies whether an open parenthesis has to be included before the condition</param>
        /// <param name="closeParenthesisAfterCondition">Specifies whether a closed parenthesis has to be included after the condition</param>
        /// <param name="value">Condition Value</param>
        /// <param name="valueTo">Condition Value when using 'Between' operator</param>
        /// <param name="alias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        IQueryBuilderAndOrGroupByOrderBy And<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false, string alias = null);

        /// <summary>
        /// Constructs an OR clause to be used with a WHERE clause<br/>
        /// Usage:<br/>
        /// .Or&lt;Entity&gt;(x => x.Prop, Operators.EqualTo, 5)<br/>
        /// (In case 'Operators.In' is used, 'value' parameter has to be an Array of objects)<br/>
        /// (In case 'Operators.Like' is used, 'value' parameter has include '%' symbols)<br/>
        /// (In case 'Operators.IsNull' or 'Operators.IsNotNull' is used, 'value' parameter is dismissed)
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the OR condition</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the condition</param>
        /// <param name="op">Condition Operator</param>
        /// <param name="openParenthesisBeforeCondition">Specifies whether an open parenthesis has to be included before the condition</param>
        /// <param name="closeParenthesisAfterCondition">Specifies whether a closed parenthesis has to be included after the condition</param>
        /// <param name="value">Condition Value</param>
        /// <param name="valueTo">Condition Value when using 'Between' operator</param>
        /// <param name="alias">The "AS" alias statement of the Entity/Table when that table has alias</param>
        IQueryBuilderAndOrGroupByOrderBy Or<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false, string alias = null);

        /// <summary>
        /// Adds a close parenthesis
        /// </summary>
        IQueryBuilderAndOrGroupByOrderBy CloseParenthesis();
    }
}
