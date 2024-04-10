using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dappator.Internal
{
    internal class QueryBuilderQuery : QueryBuilderExecutableBase, Interfaces.IQueryBuilderQuery
    {
        public QueryBuilderQuery(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
        }

        public IEnumerable<T> Query<T>()
        {
            return base.ExceptionFor<IEnumerable<T>>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                IEnumerable<T> result = base._dbConnection.Query<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public IEnumerable<TReturn> Query<TReturn>(Type[] types, Func<object[], TReturn> map)
        {
            return base.ExceptionFor<IEnumerable<TReturn>>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = base._dbConnection.Query<TReturn>(base._query, types, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> map)
        {
            return base.ExceptionFor<IEnumerable<TReturn>>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = base._dbConnection.Query<TFirst, TSecond, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map)
        {
            return base.ExceptionFor<IEnumerable<TReturn>>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = base._dbConnection.Query<TFirst, TSecond, TThird, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> map)
        {
            return base.ExceptionFor<IEnumerable<TReturn>>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = base._dbConnection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map)
        {
            return base.ExceptionFor<IEnumerable<TReturn>>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = base._dbConnection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map)
        {
            return base.ExceptionFor<IEnumerable<TReturn>>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = base._dbConnection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map)
        {
            return base.ExceptionFor<IEnumerable<TReturn>>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = base._dbConnection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public T QuerySingle<T>()
        {
            return base.ExceptionFor<T>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                T result = base._dbConnection.QuerySingle<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        public T QuerySingleOrDefault<T>()
        {
            return base.ExceptionFor<T>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                T result = base._dbConnection.QuerySingleOrDefault<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        public T QueryFirst<T>()
        {
            return base.ExceptionFor<T>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                T result = base._dbConnection.QueryFirst<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        public T QueryFirstOrDefault<T>()
        {
            return base.ExceptionFor<T>(() =>
            {
                base.OpenConnectionAndSetTransaction();

                var parameters = base.GetParameters();
                T result = base._dbConnection.QueryFirstOrDefault<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        public async Task<IEnumerable<T>> QueryAsync<T>()
        {
            return await base.ExceptionForAsync<IEnumerable<T>>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                IEnumerable<T> result = await base._dbConnection.QueryAsync<T>(sql: base._query, param: parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(Type[] types, Func<object[], TReturn> map)
        {
            return await base.ExceptionForAsync<IEnumerable<TReturn>>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = await base._dbConnection.QueryAsync<TReturn>(base._query, types, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> map)
        {
            return await base.ExceptionForAsync<IEnumerable<TReturn>>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = await base._dbConnection.QueryAsync<TFirst, TSecond, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map)
        {
            return await base.ExceptionForAsync<IEnumerable<TReturn>>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = await base._dbConnection.QueryAsync<TFirst, TSecond, TThird, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> map)
        {
            return await base.ExceptionForAsync<IEnumerable<TReturn>>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = await base._dbConnection.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map)
        {
            return await base.ExceptionForAsync<IEnumerable<TReturn>>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = await base._dbConnection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map)
        {
            return await base.ExceptionForAsync<IEnumerable<TReturn>>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = await base._dbConnection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map)
        {
            return await base.ExceptionForAsync<IEnumerable<TReturn>>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                IEnumerable<TReturn> result = await base._dbConnection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(base._query, map, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout, buffered: base._buffered);

                return result;
            });
        }

        public async Task<T> QuerySingleAsync<T>()
        {
            return await base.ExceptionForAsync<T>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                T result = await base._dbConnection.QuerySingleAsync<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>()
        {
            return await base.ExceptionForAsync<T>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                T result = await base._dbConnection.QuerySingleOrDefaultAsync<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        public async Task<T> QueryFirstAsync<T>()
        {
            return await base.ExceptionForAsync<T>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                T result = await base._dbConnection.QueryFirstAsync<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>()
        {
            return await base.ExceptionForAsync<T>(async () =>
            {
                await base.OpenConnectionAndSetTransactionAsync();

                var parameters = base.GetParameters();
                T result = await base._dbConnection.QueryFirstOrDefaultAsync<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }
    }
}
