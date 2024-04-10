using System.Data.Common;
using System.Data;
using Dapper;
using System.Collections;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Dappator.Internal
{
    internal abstract class QueryBuilderExecutableBase : QueryBuilderBaseIni, Interfaces.IQueryBuilderGetQuery
    {
        private IDictionary<Type, System.Data.DbType> _typeMappings;
        private IDictionary<System.Data.DbType, System.Data.DbType> _dbTypeConversions;
        private IDictionary<System.Data.DbType, Type> _typeConversions;

        public event EventHandler<DbTransaction> TransactionEvent;

        public QueryBuilderExecutableBase(QueryBuilderBase queryBuilderBase) : base(queryBuilderBase)
        {
            this._typeMappings = new Dictionary<Type, System.Data.DbType>
            {
                { typeof(byte), System.Data.DbType.Byte },
                { typeof(sbyte), System.Data.DbType.SByte },
                { typeof(short), System.Data.DbType.Int16 },
                { typeof(ushort), System.Data.DbType.UInt16 },
                { typeof(int), System.Data.DbType.Int32 },
                { typeof(uint), System.Data.DbType.UInt32 },
                { typeof(long), System.Data.DbType.Int64 },
                { typeof(ulong), System.Data.DbType.UInt64 },
                { typeof(float), System.Data.DbType.Single },
                { typeof(double), System.Data.DbType.Double },
                { typeof(decimal), System.Data.DbType.Decimal },
                { typeof(bool), System.Data.DbType.Boolean },
                { typeof(string), System.Data.DbType.String },
                { typeof(char), System.Data.DbType.StringFixedLength },
                { typeof(Guid), System.Data.DbType.Guid },
                { typeof(DateTime), System.Data.DbType.DateTime },
                { typeof(DateTimeOffset), System.Data.DbType.DateTimeOffset },
                { typeof(TimeSpan), System.Data.DbType.Time },
                { typeof(byte[]), System.Data.DbType.Binary },
                { typeof(byte?), System.Data.DbType.Byte },
                { typeof(sbyte?), System.Data.DbType.SByte },
                { typeof(short?), System.Data.DbType.Int16 },
                { typeof(ushort?), System.Data.DbType.UInt16 },
                { typeof(int?), System.Data.DbType.Int32 },
                { typeof(uint?), System.Data.DbType.UInt32 },
                { typeof(long?), System.Data.DbType.Int64 },
                { typeof(ulong?), System.Data.DbType.UInt64 },
                { typeof(float?), System.Data.DbType.Single },
                { typeof(double?), System.Data.DbType.Double },
                { typeof(decimal?), System.Data.DbType.Decimal },
                { typeof(bool?), System.Data.DbType.Boolean },
                { typeof(char?), System.Data.DbType.StringFixedLength },
                { typeof(Guid?), System.Data.DbType.Guid },
                { typeof(DateTime?), System.Data.DbType.DateTime },
                { typeof(DateTimeOffset?), System.Data.DbType.DateTimeOffset },
                { typeof(TimeSpan?), System.Data.DbType.Time },
#if NET6_0_OR_GREATER
                { typeof(DateOnly), System.Data.DbType.Date },
                { typeof(TimeOnly), System.Data.DbType.Time },
                { typeof(DateOnly?), System.Data.DbType.Date },
                { typeof(TimeOnly?), System.Data.DbType.Time },
#endif
            };

            this._dbTypeConversions = new Dictionary<System.Data.DbType, System.Data.DbType>
            {
                { System.Data.DbType.SByte, System.Data.DbType.Int16 },
                { System.Data.DbType.UInt16, System.Data.DbType.Int32 },
                { System.Data.DbType.UInt32, System.Data.DbType.Int64 },
                { System.Data.DbType.UInt64, System.Data.DbType.Int64 },
            };

            this._typeConversions = new Dictionary<System.Data.DbType, Type>
            {
                { System.Data.DbType.Int16, typeof(short) },
                { System.Data.DbType.Int32, typeof(int) },
                { System.Data.DbType.Int64, typeof(long) },
            };
        }

        public Interfaces.IQueryAndValues GetQuery()
        {
            var queryAndValues = new QueryAndValues(base._query, base._values);

            return queryAndValues;
        }

        protected int BasicExecute()
        {
            return base.ExceptionFor<int>(() =>
            {
                this.OpenConnectionAndSetTransaction();

                var parameters = this.GetParameters();
                int rowNumber = base._dbConnection.Execute(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return rowNumber;
            });
        }

        protected async Task<int> BasicExecuteAsync()
        {
            return await base.ExceptionForAsync<int>(async () =>
            {
                await this.OpenConnectionAndSetTransactionAsync();

                var parameters = this.GetParameters();
                int rowNumber = await base._dbConnection.ExecuteAsync(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return rowNumber;
            });
        }

        protected long BasicExecuteScalar()
        {
            return base.ExceptionFor<long>(() =>
            {
                this.OpenConnectionAndSetTransaction();

                var parameters = this.GetParameters();

                if (base._dbType == DbType.Oracle)
                    parameters.Add("generated_id", (long)-1, System.Data.DbType.Int64, ParameterDirection.ReturnValue);

                object id = base._dbConnection.ExecuteScalar(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                if (base._dbType == DbType.Oracle)
                    return parameters.Get<long>("generated_id");

                return (long)id;
            });
        }

        protected async Task<long> BasicExecuteScalarAsync()
        {
            return await base.ExceptionForAsync<long>(async () =>
            {
                await this.OpenConnectionAndSetTransactionAsync();

                var parameters = this.GetParameters();

                if (base._dbType == DbType.Oracle)
                    parameters.Add("generated_id", (long)-1, System.Data.DbType.Int64, ParameterDirection.ReturnValue);

                object id = await base._dbConnection.ExecuteScalarAsync(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                if (base._dbType == DbType.Oracle)
                    return parameters.Get<long>("generated_id");

                return (long)id;
            });
        }

        protected T BasicExecuteAndRead<T>()
        {
            return base.ExceptionFor<T>(() =>
            {
                this.OpenConnectionAndSetTransaction();
                var parameters = this.GetParameters();

                if (typeof(IEnumerable).IsAssignableFrom(typeof(T)) && typeof(T) != typeof(string))
                {
                    Type type = typeof(T).GetGenericArguments()[0];
                    Type listType = typeof(List<>).MakeGenericType(type);
                    var typeCollection = Activator.CreateInstance(listType);

                    var enumerableResult = base._dbConnection.Query(type, base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                    foreach (var element in enumerableResult)
                    {
                        typeCollection.GetType().GetMethod("Add").Invoke(typeCollection, new object[] { element });
                    }

                    return (T)typeCollection;
                }

                var result = base._dbConnection.QueryFirstOrDefault<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        protected async Task<T> BasicExecuteAndReadAsync<T>()
        {
            return await base.ExceptionForAsync<T>(async () =>
            {
                await this.OpenConnectionAndSetTransactionAsync();
                var parameters = this.GetParameters();

                if (typeof(IEnumerable).IsAssignableFrom(typeof(T)) && typeof(T) != typeof(string))
                {
                    Type type = typeof(T).GetGenericArguments()[0];
                    Type listType = typeof(List<>).MakeGenericType(type);
                    var typeCollection = Activator.CreateInstance(listType);

                    var enumerableResult = await base._dbConnection.QueryAsync(type, base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                    foreach (var element in enumerableResult)
                    {
                        typeCollection.GetType().GetMethod("Add").Invoke(typeCollection, new object[] { element });
                    }

                    return (T)typeCollection;
                }

                var result = await base._dbConnection.QueryFirstOrDefaultAsync<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        protected IEnumerable<T> BasicExecuteAndQuery<T>()
        {
            return base.ExceptionFor<IEnumerable<T>>(() =>
            {
                this.OpenConnectionAndSetTransaction();

                var parameters = this.GetParameters();
                IEnumerable<T> result = base._dbConnection.Query<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        protected async Task<IEnumerable<T>> BasicExecuteAndQueryAsync<T>()
        {
            return await base.ExceptionForAsync<IEnumerable<T>>(async () =>
            {
                await this.OpenConnectionAndSetTransactionAsync();

                var parameters = this.GetParameters();
                IEnumerable<T> result = await base._dbConnection.QueryAsync<T>(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                return result;
            });
        }

        protected T BasicExecuteAndReadScalar<T>()
        {
            return base.ExceptionFor<T>(() =>
            {
                this.VerifyT<T>();
                this.OpenConnectionAndSetTransaction();

                var parameters = this.GetParameters();
                object result = base._dbConnection.ExecuteScalar(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                if (base._dbType == DbType.Oracle)
                    result = Convert.ChangeType(result, typeof(T));

                return (T)result;
            });
        }

        protected async Task<T> BasicExecuteAndReadScalarAsync<T>()
        {
            return await base.ExceptionForAsync<T>(async () =>
            {
                this.VerifyT<T>();
                await this.OpenConnectionAndSetTransactionAsync();

                var parameters = this.GetParameters();
                object result = await base._dbConnection.ExecuteScalarAsync(base._query, parameters, transaction: base._dbTransaction, commandTimeout: base._commandTimeout);

                if (base._dbType == DbType.Oracle)
                    result = Convert.ChangeType(result, typeof(T));

                return (T)result;
            });
        }

        protected void OpenConnectionAndSetTransaction()
        {
            if (base._dbConnection.State == ConnectionState.Closed)
                base._dbConnection.Open();

            if (base._executeInTransaction == false)
                return;

            if (base._dbTransaction == null || TransactionConnectionIsNull(base._dbTransaction))
                base._dbTransaction = base._transactionIsolationLevel == null 
                    ? base._dbConnection.BeginTransaction() 
                    : base._dbConnection.BeginTransaction((IsolationLevel)base._transactionIsolationLevel);

            if (TransactionEvent != null)
                TransactionEvent(this, base._dbTransaction);
        }

        protected async Task OpenConnectionAndSetTransactionAsync()
        {
            if (base._dbConnection.State == ConnectionState.Closed)
                await base._dbConnection.OpenAsync();

            if (base._executeInTransaction == false)
                return;

            if (base._dbTransaction == null || TransactionConnectionIsNull(base._dbTransaction))
            {
#if NETCOREAPP3_0
                base._dbTransaction = base._transactionIsolationLevel == null 
                    ? await base._dbConnection.BeginTransactionAsync()
                    : await base._dbConnection.BeginTransactionAsync((IsolationLevel)base._transactionIsolationLevel);
#else
                base._dbTransaction = await Task.Run(() =>
                {
                    return base._transactionIsolationLevel == null
                        ? base._dbConnection.BeginTransaction()
                        : base._dbConnection.BeginTransaction((IsolationLevel)base._transactionIsolationLevel);
                });
#endif
            }

            if (TransactionEvent != null)
                TransactionEvent(this, base._dbTransaction);
        }

        protected DynamicParameters GetParameters()
        {
            if (base._values == null || !base._values.Any())
                return null;

            var dynamicParameters = new DynamicParameters();
            bool isBulk = base._values.All(x => x != null && x.GetType().IsArray && x.GetType().GetElementType() == typeof(object));

            for (int i = 0; i < base._values.Length; i++)
            {
                if (!isBulk)
                {
                    this.AddToDynamicParameters(base._values[i], i, i, dynamicParameters);
                    continue;
                }

                object[] valuesInValues = (object[])base._values[i];

                for (int j = 0; j < valuesInValues.Length; j++)
                {
                    int index = (i * valuesInValues.Length) + j;
                    this.AddToDynamicParameters(valuesInValues[j], index, j, dynamicParameters);
                }
            }

            return dynamicParameters;
        }

        #region Private Methods

        private void AddToDynamicParameters(object value, int pIndex, int typeIndex, DynamicParameters dynamicParameters)
        {
            object modifiedValue = this.ObjectArrayToByteArray(value);
            System.Data.DbType? dbType = base._types != null ? this.GetDbTypeFromType(base._types[typeIndex]) : this.GetDbType(modifiedValue);
            System.Data.DbType? convertedDbType = this.ConvertUnsupportedDbType(dbType, ref modifiedValue);

            if (base._dbType == DbType.Oracle && convertedDbType == System.Data.DbType.Time)
            {
                convertedDbType = null;
                if (value != null)
                    convertedDbType = System.Data.DbType.Object;
            }

            dynamicParameters.Add($"p{pIndex}", modifiedValue, convertedDbType, direction: ParameterDirection.Input);
        }

        private object ObjectArrayToByteArray(object value)
        {
            if (value == null)
                return value;

            if (value.GetType() != typeof(object[]))
                return value;

            object[] objects = (object[])value;
            byte[] bytes = new byte[objects.Length];

            for (int i = 0; i < objects.Length; i++)
                bytes[i] = (byte)objects[i];

            return bytes;
        }

        private System.Data.DbType? GetDbType(object value)
        {
            if (value == null)
                return null;

            Type typeOfValue = value.GetType();
            System.Data.DbType dbType;
            
            if (!this._typeMappings.TryGetValue(typeOfValue, out dbType))
                throw new ArgumentException(Constants.TypeNotAllowed.Replace("{type}", typeOfValue.ToString()));

            return dbType;
        }

        private System.Data.DbType? GetDbTypeFromType(Type type)
        {
            System.Data.DbType dbType;

            if (!this._typeMappings.TryGetValue(type, out dbType))
                throw new ArgumentException(Constants.TypeNotAllowed.Replace("{type}", type.ToString()));

            return dbType;
        }

        private System.Data.DbType? ConvertUnsupportedDbType(System.Data.DbType? dbType, ref object value)
        {
            if (dbType == null)
                return null;

            System.Data.DbType dbTypeConversion;

            if (!this._dbTypeConversions.TryGetValue((System.Data.DbType)dbType, out dbTypeConversion))
                return dbType;

            if (value != null)
                value = Convert.ChangeType(value, this._typeConversions[dbTypeConversion]);

            return dbTypeConversion;
        }

        private bool TransactionConnectionIsNull(DbTransaction dbTransaction)
        {
            DbConnection dbConnection = null;

            try
            {
                dbConnection = dbTransaction?.Connection;
            }
            catch (Exception) { }

            return dbConnection == null;
        }

        private void VerifyT<T>()
        {
            if (typeof(T) == typeof(string))
                return;

            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
                throw new ArgumentException(Constants.TPrimitiveNotCollection);

            var instanceOfT = Activator.CreateInstance(typeof(T));
            Type type = instanceOfT.GetType();

            if (type != typeof(string) && type.BaseType != typeof(ValueType))
                throw new ArgumentException(Constants.TPrimitiveNotClass);
        }

        #endregion
    }
}
