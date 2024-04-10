using Dapper;
using Microsoft.Data.SqlClient;

namespace Dappator.Test
{
    public class MyExampleTest
    {
        [Test]
        public async Task Send_DateOnly_TimeOnly_via_Dictionary()
        {
            #region Arrange

            using var SqlConnection = new SqlConnection("SomeConnectionString");
            SqlConnection.Open();

            string createDateAndTimeTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'DateAndTime') 
                    CREATE TABLE [DateAndTime] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [DateTime] [DATETIME] NOT NULL,
                        [DateOnly] [DATE] NOT NULL,
                        [TimeOnly] [TIME] NOT NULL,
                        CONSTRAINT [PK_DateAndTime] PRIMARY KEY CLUSTERED
                        ( [Id] ASC )
                    )";

            var command = new SqlCommand(createDateAndTimeTable, SqlConnection);
            command.ExecuteNonQuery();
            command.Dispose();

            var dateTime = new DateTime(2001, 2, 3, 4, 5, 6);
            var dateOnly = new DateOnly(2002, 3, 4);
            var timeOnly = new TimeOnly(7, 8, 9);

            var parameters = new Dictionary<string, object>
            {
                { "p0", dateTime },
                { "p1", dateOnly },
                { "p2", timeOnly },
            };

            #endregion

            #region Act

            string query = "" +
                "INSERT INTO [DateAndTime] ([DateTime], [DateOnly], [TimeOnly]) " +
                "VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            // Here it fails with the following error:
            // System.NotSupportedException : The member p1 of type System.DateOnly cannot be used as a parameter value.
            object id = await SqlConnection.ExecuteScalarAsync(query, parameters);

            #endregion

            #region Assert

            Assert.That((long)id, Is.GreaterThan(0));

            #endregion
        }

        [Test]
        public async Task Send_DateOnly_TimeOnly_via_DynamicParameters()
        {
            #region Arrange

            using var SqlConnection = new SqlConnection("SomeConnectionString");
            SqlConnection.Open();

            string createDateAndTimeTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'DateAndTime') 
                    CREATE TABLE [DateAndTime] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [DateTime] [DATETIME] NOT NULL,
                        [DateOnly] [DATE] NOT NULL,
                        [TimeOnly] [TIME] NOT NULL,
                        CONSTRAINT [PK_DateAndTime] PRIMARY KEY CLUSTERED
                        ( [Id] ASC )
                    )";

            var command = new SqlCommand(createDateAndTimeTable, SqlConnection);
            command.ExecuteNonQuery();
            command.Dispose();

            var dateTime = new DateTime(2001, 2, 3, 4, 5, 6);
            var dateOnly = new DateOnly(2002, 3, 4);
            var timeOnly = new TimeOnly(7, 8, 9);

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p0", dateTime);
            dynamicParameters.Add("p1", dateOnly);
            dynamicParameters.Add("p2", timeOnly);

            #endregion

            #region Act

            string query = "" +
                "INSERT INTO [DateAndTime] ([DateTime], [DateOnly], [TimeOnly]) " +
                "VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            // Here it fails with the following error:
            // System.NotSupportedException : The member p1 of type System.DateOnly cannot be used as a parameter value.
            object id = await SqlConnection.ExecuteScalarAsync(query, dynamicParameters);

            #endregion

            #region Assert

            Assert.That((long)id, Is.GreaterThan(0));

            #endregion
        }

        [Test]
        public async Task Send_DateOnly_TimeOnly_via_DynamicParameters_Set_DbType_Get_Row()
        {
            #region Arrange

            using var SqlConnection = new SqlConnection("SomeConnectionString");
            SqlConnection.Open();

            string createDateAndTimeTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'DateAndTime') 
                    CREATE TABLE [DateAndTime] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [DateTime] [DATETIME] NOT NULL,
                        [DateOnly] [DATE] NOT NULL,
                        [TimeOnly] [TIME] NOT NULL,
                        CONSTRAINT [PK_DateAndTime] PRIMARY KEY CLUSTERED
                        ( [Id] ASC )
                    )";

            var command = new SqlCommand(createDateAndTimeTable, SqlConnection);
            command.ExecuteNonQuery();
            command.Dispose();

            var dateTime = new DateTime(2001, 2, 3, 4, 5, 6);
            var dateOnly = new DateOnly(2002, 3, 4);
            var timeOnly = new TimeOnly(7, 8, 9);

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p0", dateTime);
            dynamicParameters.Add("p1", dateOnly, System.Data.DbType.Date);
            dynamicParameters.Add("p2", timeOnly, System.Data.DbType.Time);

            #endregion

            #region Act

            string query = "" +
                "INSERT INTO [DateAndTime] ([DateTime], [DateOnly], [TimeOnly]) " +
                "VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            // Here it works!
            object id = await SqlConnection.ExecuteScalarAsync(query, dynamicParameters);

            // Now, we are gonna get that row.
            dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p0", (long)id);

            query = "SELECT * FROM [DateAndTime] WHERE [Id] = @p0";

            // Here it fails with the following error:
            // System.Data.DataException : Error parsing column 2 (DateOnly=3/4/2002 12:00:00 AM - DateTime)
            // Inner Exception: System.InvalidCastException : Invalid cast from 'System.DateTime' to 'System.DateOnly'.
            var dateAndTimes = await SqlConnection.QueryAsync<DateAndTime1>(query, dynamicParameters);

            #endregion

            #region Assert

            Assert.That(dateAndTimes.Count(), Is.EqualTo(1));

            #endregion
        }

        class DateAndTime1
        {
            public int Id { get; set; }

            public DateTime DateTime { get; set; }

            public DateOnly DateOnly { get; set; }

            public TimeOnly TimeOnly { get; set; }
        }

        [Test]
        public async Task Send_DateOnly_TimeOnly_via_DynamicParameters_Set_DbType_Get_Row_DateTime_Properties()
        {
            #region Arrange

            using var SqlConnection = new SqlConnection("SomeConnectionString");
            SqlConnection.Open();

            string createDateAndTimeTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'DateAndTime') 
                    CREATE TABLE [DateAndTime] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [DateTime] [DATETIME] NOT NULL,
                        [DateOnly] [DATE] NOT NULL,
                        [TimeOnly] [TIME] NOT NULL,
                        CONSTRAINT [PK_DateAndTime] PRIMARY KEY CLUSTERED
                        ( [Id] ASC )
                    )";

            var command = new SqlCommand(createDateAndTimeTable, SqlConnection);
            command.ExecuteNonQuery();
            command.Dispose();

            var dateTime = new DateTime(2001, 2, 3, 4, 5, 6);
            var dateOnly = new DateOnly(2002, 3, 4);
            var timeOnly = new TimeOnly(7, 8, 9);

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p0", dateTime);
            dynamicParameters.Add("p1", dateOnly, System.Data.DbType.Date);
            dynamicParameters.Add("p2", timeOnly, System.Data.DbType.Time);

            #endregion

            #region Act

            string query = "" +
                "INSERT INTO [DateAndTime] ([DateTime], [DateOnly], [TimeOnly]) " +
                "VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            // Here it works!
            object id = await SqlConnection.ExecuteScalarAsync(query, dynamicParameters);

            // Now, we are gonna get that row.
            dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p0", (long)id);

            query = "SELECT * FROM [DateAndTime] WHERE [Id] = @p0";

            // Here it fails with the following error:
            // System.Data.DataException : Error parsing column 3 (TimeOnly=07:08:09 - Object)
            // Inner Exception: System.InvalidCastException : Object must implement IConvertible.
            var dateAndTimes = await SqlConnection.QueryAsync<DateAndTime2>(query, dynamicParameters);

            #endregion

            #region Assert

            Assert.That(dateAndTimes.Count(), Is.EqualTo(1));

            #endregion
        }

        class DateAndTime2
        {
            public int Id { get; set; }

            public DateTime DateTime { get; set; }

            public DateTime DateOnly { get; set; }

            public DateTime TimeOnly { get; set; }
        }
    }
}
