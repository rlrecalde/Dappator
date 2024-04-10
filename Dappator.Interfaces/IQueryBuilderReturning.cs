using System.Linq.Expressions;
using System;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderReturning : IQueryBuilderExecuteScalar
    {
        /// <summary>
        /// Sets the column name for the RETURNING clause<br/>
        /// Usage:<br/>
        /// .Returning&lt;Entity&gt;(x => x.IdentityProp)<br/>
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="property">Lambda Expression for the identity/autoincrement Entity Property (Table Column)</param>
        IQueryBuilderExecuteScalar Returning<T>(Expression<Func<T, object>> property);
    }
}
