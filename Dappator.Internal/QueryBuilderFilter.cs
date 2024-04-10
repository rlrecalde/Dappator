using System;
using System.Linq.Expressions;

namespace Dappator.Internal
{
    internal class QueryBuilderFilter : QueryBuilderFilterBase, Interfaces.IQueryBuilderWhere, Interfaces.IQueryBuilderAndOr
    {
        public QueryBuilderFilter(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
        }

        public Interfaces.IQueryBuilderAndOr Where<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null)
        {
            base.BasicWhere<T>(property, op, value, valueTo);

            return this;
        }

        public Interfaces.IQueryBuilderAndOr And<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false)
        {
            base.BasicAnd<T>(property, op, value, valueTo, openParenthesisBeforeCondition, closeParenthesisAfterCondition);

            return this;
        }

        public Interfaces.IQueryBuilderAndOr Or<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false)
        {
            base.BasicOr<T>(property, op, value, valueTo, openParenthesisBeforeCondition, closeParenthesisAfterCondition);

            return this;
        }

        public Interfaces.IQueryBuilderAndOr CloseParenthesis()
        {
            base.BasicCloseParenthesis();

            return this;
        }
    }
}
