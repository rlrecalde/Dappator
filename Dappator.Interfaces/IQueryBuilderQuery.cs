using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dappator.Interfaces
{
    public interface IQueryBuilderQuery : IQueryBuilderGetQuery
    {
        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Query&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <returns>A collection of <typeparamref name="T"/></returns>
        IEnumerable<T> Query<T>();

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Query&lt;<typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="types">Array of types in the recordset.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        IEnumerable<TReturn> Query<TReturn>(Type[] types, Func<object[], TReturn> map);

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Query&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> map);

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Query&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map);

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Query&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TFourth"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> map);

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Query&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TFourth"/>, <typeparamref name="TFifth"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map);

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Query&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TFourth"/>, <typeparamref name="TFifth"/>, <typeparamref name="TSixth"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map);

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.Query&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TFourth"/>, <typeparamref name="TFifth"/>, <typeparamref name="TSixth"/>, <typeparamref name="TSeventh"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map);

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.QuerySingle&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <returns>The single <typeparamref name="T"/> object</returns>
        T QuerySingle<T>();

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.QuerySingleOrDefault&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <returns>The single or default <typeparamref name="T"/> object</returns>
        T QuerySingleOrDefault<T>();

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.QueryFirst&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <returns>The first <typeparamref name="T"/> object</returns>
        T QueryFirst<T>();

        /// <summary>
        /// Executes the built query against Dapper -> DbConnection.QueryFirstOrDefault&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <returns>The first or default <typeparamref name="T"/> object</returns>
        T QueryFirstOrDefault<T>();

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="T"/>&gt;() method
        /// Execute a query asynchronously using Task.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <returns>A collection of <typeparamref name="T"/></returns>
        Task<IEnumerable<T>> QueryAsync<T>();

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        Task<IEnumerable<TReturn>> QueryAsync<TReturn>(Type[] types, Func<object[], TReturn> map);

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> map);

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map);

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TFourth"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> map);

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TFourth"/>, <typeparamref name="TFifth"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map);

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TFourth"/>, <typeparamref name="TFifth"/>, <typeparamref name="TSixth"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map);

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryAsync&lt;<typeparamref name="TFirst"/>, <typeparamref name="TSecond"/>, <typeparamref name="TThird"/>, <typeparamref name="TFourth"/>, <typeparamref name="TFifth"/>, <typeparamref name="TSixth"/>, <typeparamref name="TSeventh"/>, <typeparamref name="TReturn"/>&gt;() method
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <returns>A collection of <typeparamref name="TReturn"/></returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map);

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QuerySingleAsync&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <returns>The single <typeparamref name="T"/> object</returns>
        Task<T> QuerySingleAsync<T>();

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QuerySingleOrDefault&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <returns>The single or default <typeparamref name="T"/> object</returns>
        Task<T> QuerySingleOrDefaultAsync<T>();

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryFirstAsync&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <returns>The first <typeparamref name="T"/> object</returns>
        Task<T> QueryFirstAsync<T>();

        /// <summary>
        /// Executes the built query asynchronously against Dapper -> DbConnection.QueryFirstOrDefaultAsync&lt;<typeparamref name="T"/>&gt;() method
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <returns>The first or default <typeparamref name="T"/> object</returns>
        Task<T> QueryFirstOrDefaultAsync<T>();
    }
}
