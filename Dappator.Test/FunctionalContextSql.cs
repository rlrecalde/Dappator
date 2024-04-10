using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Dappator.Test
{
    public class FunctionalContextSql : See_Firebird_TestBase<Providers.SqlProvider>
    {
        [SetUp]
        public void Setup()
        {
            base._connectionString = "Data Source=127.0.0.1;Initial Catalog=Dappator;User ID=sa;Password=Guest123!;TrustServerCertificate=True";

            base.CreateTables();

            DbConnection dbConnection = this._provider.GetOpenConnection(base._connectionString);

            string createDateAndTime = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'U' AND name = 'DateAndTime')
                    CREATE TABLE [DateAndTime] (
                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                        [DateTime] [DATETIME] NOT NULL,
                        [DateOnly] [DATE] NOT NULL,
                        [TimeOnly] [TIME] NOT NULL,
                        CONSTRAINT [PK_DateAndTime] PRIMARY KEY CLUSTERED
                        ( [Id] ASC )
                    )";

            var command = new SqlCommand(createDateAndTime, (SqlConnection)dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();

            string deleteDateAndTime = "DELETE FROM [DateAndTime]";

            command = new SqlCommand(deleteDateAndTime, (SqlConnection)dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();

            dbConnection.Close();
            dbConnection.Dispose();
        }

        #region Insert, Update, Delete, Select

        [Test]
        public void Insert()
        {
            #region Arrange

            var userToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int userId = (int)sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public async Task Insert_Async()
        {
            #region Arrange

            var userToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int userId = (int)await sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public void Insert_Bulk()
        {
            #region Arrange

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            var arrayParams = new object[][] {
                new object[] { user1ToInsert.Nick, user1ToInsert.Password },
                new object[] { user2ToInsert.Nick, user2ToInsert.Password },
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int oneUserId = (int)sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, arrayParams)
                .ExecuteScalar();

            #endregion

            #region Assert

            Assert.That(oneUserId, Is.GreaterThan(0));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Sql.User user = users.FirstOrDefault(x => x.Nick == userToInsert.Nick && x.Password == userToInsert.Password);

                Assert.IsNotNull(user);
                Assert.That(user.Id, Is.GreaterThan(0));
            }

            int lastUserIdInDB = users.Max(x => x.Id);
            Assert.That(oneUserId, Is.EqualTo(lastUserIdInDB));

            #endregion
        }

        [Test]
        public async Task Insert_Bulk_Async()
        {
            #region Arrange

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            var arrayParams = new object[][] {
                new object[] { user1ToInsert.Nick, user1ToInsert.Password },
                new object[] { user2ToInsert.Nick, user2ToInsert.Password },
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int oneUserId = (int)await sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, arrayParams)
                .ExecuteScalarAsync();

            #endregion

            #region Assert

            Assert.That(oneUserId, Is.GreaterThan(0));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Sql.User user = users.FirstOrDefault(x => x.Nick == userToInsert.Nick && x.Password == userToInsert.Password);

                Assert.IsNotNull(user);
                Assert.That(user.Id, Is.GreaterThan(0));
            }

            int lastUserIdInDB = users.Max(x => x.Id);
            Assert.That(oneUserId, Is.EqualTo(lastUserIdInDB));

            #endregion
        }

        [Test]
        public void Update()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int rowNumber = sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Update_Async()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int rowNumber = await sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Delete()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int rowNumber = sqlContext
                .Delete<Model.Sql.User>()
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Delete_Async()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int rowNumber = await sqlContext
                .Delete<Model.Sql.User>()
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Select()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .Query<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public async Task Select_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = await sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .QueryAsync<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public void Select_Where()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user2.Id)
                .Query<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(1));

            var userReturned = users.First();
            Assert.That(userReturned.Id, Is.EqualTo(user2.Id));
            Assert.That(userReturned.Nick, Is.EqualTo(user2.Nick));
            Assert.That(userReturned.Password, Is.EqualTo(user2.Password));

            #endregion
        }

        [Test]
        public async Task Select_Where_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = await sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user2.Id)
                .QueryAsync<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(1));

            var userReturned = users.First();
            Assert.That(userReturned.Id, Is.EqualTo(user2.Id));
            Assert.That(userReturned.Nick, Is.EqualTo(user2.Nick));
            Assert.That(userReturned.Password, Is.EqualTo(user2.Password));

            #endregion
        }

        [Test]
        public void Select_Limit()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Sql.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .Query<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(1));

            var userReturned = users.First();
            Assert.That(userReturned.Id, Is.EqualTo(user1.Id));
            Assert.That(userReturned.Nick, Is.EqualTo(user1.Nick));
            Assert.That(userReturned.Password, Is.EqualTo(user1.Password));

            #endregion
        }

        [Test]
        public async Task Select_Limit_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = await sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Sql.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryAsync<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(1));

            var userReturned = users.First();
            Assert.That(userReturned.Id, Is.EqualTo(user1.Id));
            Assert.That(userReturned.Nick, Is.EqualTo(user1.Nick));
            Assert.That(userReturned.Password, Is.EqualTo(user1.Password));

            #endregion
        }

        [Test]
        public void Select_Limit_Offset()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Sql.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1, 1)
                .Query<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(1));

            var userReturned = users.First();
            Assert.That(userReturned.Id, Is.EqualTo(user2.Id));
            Assert.That(userReturned.Nick, Is.EqualTo(user2.Nick));
            Assert.That(userReturned.Password, Is.EqualTo(user2.Password));

            #endregion
        }

        [Test]
        public async Task Select_Limit_Offset_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = await sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Sql.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1, 1)
                .QueryAsync<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(1));

            var userReturned = users.First();
            Assert.That(userReturned.Id, Is.EqualTo(user2.Id));
            Assert.That(userReturned.Nick, Is.EqualTo(user2.Nick));
            Assert.That(userReturned.Password, Is.EqualTo(user2.Password));

            #endregion
        }

        [Test]
        public void Select_Limit_FirstOrDefault()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            Model.Sql.User user = sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Sql.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.IsNotNull(user);
            Assert.That(user.Id, Is.EqualTo(user1.Id));
            Assert.That(user.Nick, Is.EqualTo(user1.Nick));
            Assert.That(user.Password, Is.EqualTo(user1.Password));

            #endregion
        }

        [Test]
        public async Task Select_Limit_FirstOrDefault_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            Model.Sql.User user = await sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Sql.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.IsNotNull(user);
            Assert.That(user.Id, Is.EqualTo(user1.Id));
            Assert.That(user.Nick, Is.EqualTo(user1.Nick));
            Assert.That(user.Password, Is.EqualTo(user1.Password));

            #endregion
        }

        [Test]
        public void Select_With_Query()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            string query = sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .GetQuery();

            IEnumerable<Model.Sql.User> users = sqlContext
                .SetQuery(query)
                .Query<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public async Task Select_With_Query_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            string query = sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .GetQuery();

            IEnumerable<Model.Sql.User> users = await sqlContext
                .SetQuery(query)
                .QueryAsync<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public void Select_Not_Buffered()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);
            sqlContext.Buffered = false;

            IEnumerable<Model.Sql.User> users = sqlContext
                .Select<Model.Sql.User>(x => new { x.Id, x.Nick, x.Password })
                .Query<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.IsTrue(users.GetType().FullName.Contains("Dapper"));

            #endregion
        }

        #endregion

        #region Select Aggregate

        [Test]
        public void Select_Count()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Count<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                int expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Count();
                Assert.That((int)userValueAmount.Amount, Is.EqualTo(expectedAmount));
            }

            #endregion
        }

        [Test]
        public async Task Select_Count_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Count<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                int expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Count();
                Assert.That((int)userValueAmount.Amount, Is.EqualTo(expectedAmount));
            }

            #endregion
        }

        [Test]
        public void Select_Max()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Max<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                double expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Max(x => x.Value);
                Assert.That(userValueAmount.Amount, Is.EqualTo(expectedAmount));
            }

            #endregion
        }

        [Test]
        public async Task Select_Max_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Max<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                double expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Max(x => x.Value);
                Assert.That(userValueAmount.Amount, Is.EqualTo(expectedAmount));
            }

            #endregion
        }

        [Test]
        public void Select_Min()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Min<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                double expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Min(x => x.Value);
                Assert.That(userValueAmount.Amount, Is.EqualTo(expectedAmount));
            }

            #endregion
        }

        [Test]
        public async Task Select_Min_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Min<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                double expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Min(x => x.Value);
                Assert.That(userValueAmount.Amount, Is.EqualTo(expectedAmount));
            }

            #endregion
        }

        [Test]
        public void Select_Sum()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Sum<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                double expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Sum(x => x.Value);
                Assert.That(userValueAmount.Amount, Is.EqualTo(expectedAmount));
            }

            #endregion
        }

        [Test]
        public async Task Select_Sum_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Sum<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                double expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Sum(x => x.Value);
                Assert.That(userValueAmount.Amount, Is.EqualTo(expectedAmount));
            }

            #endregion
        }

        [Test]
        public void Select_Avg()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Avg<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                double expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Average(x => x.Value);
                Assert.That(Math.Round(userValueAmount.Amount, 5), Is.EqualTo(Math.Round(expectedAmount, 5)));
            }

            #endregion
        }

        [Test]
        public async Task Select_Avg_Async()
        {
            #region Arrange

            var user1 = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1);

            var user2 = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2);

            var user1Value1 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value1);

            var user1Value2 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value2);

            var user1Value3 = new Model.Sql.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user1Value3);

            var user2Value1 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value1);

            var user2Value2 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value2);

            var user2Value3 = new Model.Sql.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue<Model.Sql.UserValue>(user2Value3);

            var userValues = new List<Model.Sql.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Sql.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Avg<Model.Sql.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Sql.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            #endregion

            #region Assert

            int expectedAmounts = userValues.Select(x => x.UserId).Distinct().Count();
            Assert.That(userValueAmounts.Count(), Is.EqualTo(expectedAmounts));

            foreach (var userValueAmount in userValueAmounts)
            {
                double expectedAmount = userValues.Where(x => x.UserId == userValueAmount.UserId).Average(x => x.Value);
                Assert.That(Math.Round(userValueAmount.Amount, 5), Is.EqualTo(Math.Round(expectedAmount, 5)));
            }

            #endregion
        }

        #endregion

        #region Transactions

        [Test]
        public void Insert_In_Transaction()
        {
            #region Arrange

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int user1Id = (int)sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalar();

            user1ToInsert.Id = user1Id;

            int user2Id = (int)sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalar();

            user2ToInsert.Id = user2Id;

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Sql.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

                Assert.IsNotNull(user);
                Assert.That(user.Nick, Is.EqualTo(userToInsert.Nick));
                Assert.That(user.Password, Is.EqualTo(userToInsert.Password));
            }

            #endregion
        }

        [Test]
        public async Task Insert_In_Transaction_Async()
        {
            #region Arrange

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int user1Id = (int)await sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalarAsync();

            user1ToInsert.Id = user1Id;

            int user2Id = (int)await sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalarAsync();

            user2ToInsert.Id = user2Id;

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Sql.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

                Assert.IsNotNull(user);
                Assert.That(user.Nick, Is.EqualTo(userToInsert.Nick));
                Assert.That(user.Password, Is.EqualTo(userToInsert.Password));
            }

            #endregion
        }

        [Test]
        public void Update_In_Transaction()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int rowNumber1 = sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            nickToUpdate += "_extra";
            passwordToUpdate += "_extra";

            int rowNumber2 = sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Update_In_Transaction_Async()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int rowNumber1 = await sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            nickToUpdate += "_extra";
            passwordToUpdate += "_extra";

            int rowNumber2 = await sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Delete_In_Transaction()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int rowNumber1 = sqlContext
                .Delete<Model.Sql.User>()
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            int rowNumber2 = sqlContext
                .Delete<Model.Sql.User>()
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(0));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Delete_In_Transaction_Async()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int rowNumber1 = await sqlContext
                .Delete<Model.Sql.User>()
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            int rowNumber2 = await sqlContext
                .Delete<Model.Sql.User>()
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(0));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Insert_Update_In_Transaction()
        {
            #region Arrange

            var userToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{userToInsert.Nick}_sarasa";
            string passwordToUpdate = $"{userToInsert.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int userId = (int)sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            userToInsert.Id = userId;

            int rowNumber = sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(userToInsert.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Insert_Update_In_Transaction_Async()
        {
            #region Arrange

            var userToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{userToInsert.Nick}_sarasa";
            string passwordToUpdate = $"{userToInsert.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int userId = (int)await sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            userToInsert.Id = userId;

            int rowNumber = await sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(userToInsert.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Insert_Delete_In_Transaction()
        {
            #region Arrange

            var userToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int userId = (int)sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            userToInsert.Id = userId;

            int rowNumber = sqlContext
                .Delete<Model.Sql.User>()
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Insert_Delete_In_Transaction_Async()
        {
            #region Arrange

            var userToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int userId = (int)await sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            userToInsert.Id = userId;

            int rowNumber = await sqlContext
                .Delete<Model.Sql.User>()
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Insert_Update_In_Transaction_2_Same_Connection()
        {
            #region Arrange

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick1ToUpdate = $"{user1ToInsert.Nick}_sarasa";
            string password1ToUpdate = $"{user1ToInsert.Password}_sarasa";

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick2ToUpdate = $"{user2ToInsert.Nick}_sarasa";
            string password2ToUpdate = $"{user2ToInsert.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int user1Id = (int)sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalar();

            user1ToInsert.Id = user1Id;

            int rowNumber1 = sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nick1ToUpdate, password1ToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user1ToInsert.Id)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            // Here we don't set 'ExecuteInTransaction' property to false
            // so, a new DbTransaction object will be created for the following queries.

            IEnumerable<Model.Sql.User> usersFirst = base.GetUsers<Model.Sql.User>();

            int user2Id = (int)sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalar();

            user2ToInsert.Id = user2Id;

            int rowNumber2 = sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nick2ToUpdate, password2ToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user2ToInsert.Id)
                .Execute();

            dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            IEnumerable<Model.Sql.User> usersSecond = base.GetUsers<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(usersFirst.Count(), Is.EqualTo(1));
            Assert.That(usersSecond.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public async Task Insert_Update_In_Transaction_2_Same_Connection_Async()
        {
            #region Arrange

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick1ToUpdate = $"{user1ToInsert.Nick}_sarasa";
            string password1ToUpdate = $"{user1ToInsert.Password}_sarasa";

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick2ToUpdate = $"{user2ToInsert.Nick}_sarasa";
            string password2ToUpdate = $"{user2ToInsert.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;

            int user1Id = (int)await sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalarAsync();

            user1ToInsert.Id = user1Id;

            int rowNumber1 = await sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nick1ToUpdate, password1ToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user1ToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            // Here we don't set 'ExecuteInTransaction' property to false
            // so, a new DbTransaction object will be created for the following queries.

            IEnumerable<Model.Sql.User> usersFirst = base.GetUsers<Model.Sql.User>();

            int user2Id = (int)await sqlContext
                .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalarAsync();

            user2ToInsert.Id = user2Id;

            int rowNumber2 = await sqlContext
                .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nick2ToUpdate, password2ToUpdate)
                .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, user2ToInsert.Id)
                .ExecuteAsync();

            dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            IEnumerable<Model.Sql.User> usersSecond = base.GetUsers<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(usersFirst.Count(), Is.EqualTo(1));
            Assert.That(usersSecond.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public void Transaction_Fails()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";
            string exceptionMessage = "Entity not found";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;
            DbTransaction dbTransaction = null;

            var exception = Assert.Throws<Exception>(() =>
            {
                try
                {
                    int userId = (int)sqlContext
                        .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                        .ExecuteScalar();

                    int rowNumber = sqlContext
                        .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                        .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, -1)
                        .Execute();

                    if (rowNumber == 0)
                        throw new Exception(exceptionMessage);

                    dbTransaction = sqlContext.GetDbTransaction();
                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction = sqlContext.GetDbTransaction();
                    dbTransaction.Rollback();

                    throw;
                }
                finally
                {
                    dbTransaction.Dispose();
                    sqlContext.ExecuteInTransaction = false;
                }
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(exceptionMessage));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Transaction_Fails_Async()
        {
            #region Arrange

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";
            string exceptionMessage = "Entity not found";

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            sqlContext.ExecuteInTransaction = true;
            DbTransaction dbTransaction = null;

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                try
                {
                    int userId = (int)await sqlContext
                        .Insert<Model.Sql.User>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                        .ExecuteScalarAsync();

                    int rowNumber = await sqlContext
                        .Update<Model.Sql.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                        .Where<Model.Sql.User>(x => x.Id, Common.Operators.EqualTo, -1)
                        .ExecuteAsync();

                    if (rowNumber == 0)
                        throw new Exception(exceptionMessage);

                    dbTransaction = sqlContext.GetDbTransaction();
                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction = sqlContext.GetDbTransaction();
                    dbTransaction.Rollback();

                    throw;
                }
                finally
                {
                    dbTransaction.Dispose();
                    sqlContext.ExecuteInTransaction = false;
                }
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(exceptionMessage));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        #endregion

        #region Error Handling

        [Test]
        public void Insert_Sql_Error()
        {
            #region Arrange

            int userId = -1;
            double value = 0;

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            Interfaces.IException exception = null;

            try
            {
                int userValueId = (int)sqlContext
                    .Insert<Model.Sql.UserValue>(x => new { x.UserId, x.Value }, userId, value)
                    .ExecuteScalar();
            }
            catch (Exception ex)
            {
                exception = (Interfaces.IException)ex;
            }

            #endregion

            #region Assert

            Console.WriteLine(exception?.Query);
            Console.WriteLine(exception?.Message);
            Console.WriteLine(exception?.StackTrace);

            #endregion
        }

        [Test]
        public async Task Insert_Sql_Error_Async()
        {
            #region Arrange

            int userId = -1;
            double value = 0;

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            Interfaces.IException exception = null;

            try
            {
                int userValueId = (int)await sqlContext
                    .Insert<Model.Sql.UserValue>(x => new { x.UserId, x.Value }, userId, value)
                    .ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                exception = (Interfaces.IException)ex;
            }

            #endregion

            #region Assert

            Console.WriteLine(exception?.Query);
            Console.WriteLine(exception?.Message);
            Console.WriteLine(exception?.StackTrace);

            #endregion
        }

        #endregion

        #region Stored Procedures

        [Test]
        public void SpInsertUser()
        {
            #region Arrange

            base.CreateSpInsertUser();

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int rowNumber = sqlContext
                .StoredProcedure<Model.Sql.SpInsertUser>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                .Execute();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();

            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.GreaterThan(0));
            Assert.That(insertedUser.Nick, Is.EqualTo(user.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(user.Password));

            #endregion
        }

        [Test]
        public async Task SpInsertUser_Async()
        {
            #region Arrange

            base.CreateSpInsertUser();

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int rowNumber = await sqlContext
                .StoredProcedure<Model.Sql.SpInsertUser>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                .ExecuteAsync();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();

            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.GreaterThan(0));
            Assert.That(insertedUser.Nick, Is.EqualTo(user.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(user.Password));

            #endregion
        }

        [Test]
        public void SpInsertUserAndGetId()
        {
            #region Arrange

            base.CreateSpInsertUserAndGetId();

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int userId = (int)sqlContext
                .StoredProcedure<Model.Sql.SpInsertUserAndGetId>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                .ExecuteAndRead<long>();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();

            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(user.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(user.Password));

            #endregion
        }

        [Test]
        public async Task SpInsertUserAndGetId_Async()
        {
            #region Arrange

            base.CreateSpInsertUserAndGetId();

            var user = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int userId = (int)await sqlContext
                .StoredProcedure<Model.Sql.SpInsertUserAndGetId>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                .ExecuteAndReadAsync<long>();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.Sql.User> users = base.GetUsers<Model.Sql.User>();

            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Sql.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(user.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(user.Password));

            #endregion
        }

        [Test]
        public void SpGetUserById()
        {
            #region Arrange

            base.CreateSpGetUserById();

            var userToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(userToInsert);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            Model.Sql.User user = sqlContext
                .StoredProcedure<Model.Sql.SpGetUserById>(x => new { x.Id }, userToInsert.Id)
                .ExecuteAndRead<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(user.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(user.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public async Task SpGetUserById_Async()
        {
            #region Arrange

            base.CreateSpGetUserById();

            var userToInsert = new Model.Sql.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser<Model.Sql.User>(userToInsert);

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            Model.Sql.User user = await sqlContext
                .StoredProcedure<Model.Sql.SpGetUserById>(x => new { x.Id }, userToInsert.Id)
                .ExecuteAndReadAsync<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(user.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(user.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public void SpGetUsers()
        {
            #region Arrange

            base.CreateSpGetUsers();

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1ToInsert);

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2ToInsert);

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = sqlContext
                .StoredProcedure<Model.Sql.SpGetUsers>()
                .ExecuteAndRead<IEnumerable<Model.Sql.User>>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Sql.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

                Assert.IsNotNull(user);
                Assert.That(user.Nick, Is.EqualTo(userToInsert.Nick));
                Assert.That(user.Password, Is.EqualTo(userToInsert.Password));
            }

            #endregion
        }

        [Test]
        public async Task SpGetUsers_Async()
        {
            #region Arrange

            base.CreateSpGetUsers();

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1ToInsert);

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2ToInsert);

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = await sqlContext
                .StoredProcedure<Model.Sql.SpGetUsers>()
                .ExecuteAndReadAsync<IEnumerable<Model.Sql.User>>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Sql.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

                Assert.IsNotNull(user);
                Assert.That(user.Nick, Is.EqualTo(userToInsert.Nick));
                Assert.That(user.Password, Is.EqualTo(userToInsert.Password));
            }

            #endregion
        }

        #endregion

        #region Functions

        [Test]
        public void FnGetUsers()
        {
            #region Arrange

            base.CreateFnGetUsers();

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1ToInsert);

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2ToInsert);

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = sqlContext
                .TableFunction<Model.Sql.FnGetUsers>()
                .ExecuteAndQuery<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Sql.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

                Assert.IsNotNull(user);
                Assert.That(user.Nick, Is.EqualTo(userToInsert.Nick));
                Assert.That(user.Password, Is.EqualTo(userToInsert.Password));
            }

            #endregion
        }

        [Test]
        public async Task FnGetUsers_Async()
        {
            #region Arrange

            base.CreateFnGetUsers();

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1ToInsert);

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2ToInsert);

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            IEnumerable<Model.Sql.User> users = await sqlContext
                .TableFunction<Model.Sql.FnGetUsers>()
                .ExecuteAndQueryAsync<Model.Sql.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Sql.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

                Assert.IsNotNull(user);
                Assert.That(user.Nick, Is.EqualTo(userToInsert.Nick));
                Assert.That(user.Password, Is.EqualTo(userToInsert.Password));
            }

            #endregion
        }

        [Test]
        public void FnGetUserIdByNick()
        {
            #region Arrange

            base.CreateFnGetUserIdByNick();

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1ToInsert);

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2ToInsert);

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int userId = sqlContext
                .ScalarFunction<Model.Sql.FnGetUserIdByNick>(x => new { x.Nick }, user2ToInsert.Nick)
                .ExecuteAndReadScalar<int>();

            #endregion

            #region Assert

            Assert.That(userId, Is.EqualTo(user2ToInsert.Id));

            #endregion
        }

        [Test]
        public async Task FnGetUserIdByNick_Async()
        {
            #region Arrange

            base.CreateFnGetUserIdByNick();

            var user1ToInsert = new Model.Sql.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser<Model.Sql.User>(user1ToInsert);

            var user2ToInsert = new Model.Sql.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser<Model.Sql.User>(user2ToInsert);

            var usersToInsert = new List<Model.Sql.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqlContext(base._connectionString);

            int userId = await sqlContext
                .ScalarFunction<Model.Sql.FnGetUserIdByNick>(x => new { x.Nick }, user2ToInsert.Nick)
                .ExecuteAndReadScalarAsync<int>();

            #endregion

            #region Assert

            Assert.That(userId, Is.EqualTo(user2ToInsert.Id));

            #endregion
        }

        #endregion

        [Test]
        public void Insert_Dates()
        {
            #region Arrange

            var dateTime = new DateTime(2001, 2, 3, 4, 5, 6);
            var dateOnly = new DateOnly(2002, 3, 4);
            var timeOnly = new TimeOnly(7, 8, 9);

            using var sqlContext = new SqlContext(base._connectionString);

            #endregion

            #region Act

            int id = (int)sqlContext
                .Insert<Model.Sql.DateAndTime>(x => new
                { x.DateTime, x.DateOnly, x.TimeOnly },
                    dateTime, dateOnly, timeOnly)
                .ExecuteScalar();

            #endregion

            #region Assert

            Assert.That(id, Is.GreaterThan(0));

            Model.Sql.DateAndTime dateAndTime = sqlContext
                .Select<Model.Sql.DateAndTime>(x => new { x.Id, x.DateTime, x.DateOnly })
                .Where<Model.Sql.DateAndTime>(x => x.Id, Common.Operators.EqualTo, id)
                .OrderBy<Model.Sql.DateAndTime>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.Sql.DateAndTime>();

            Assert.IsNotNull(dateAndTime);

            Assert.That(dateAndTime.DateTime.Year, Is.EqualTo(dateTime.Year));
            Assert.That(dateAndTime.DateTime.Month, Is.EqualTo(dateTime.Month));
            Assert.That(dateAndTime.DateTime.Day, Is.EqualTo(dateTime.Day));
            Assert.That(dateAndTime.DateTime.Hour, Is.EqualTo(dateTime.Hour));
            Assert.That(dateAndTime.DateTime.Minute, Is.EqualTo(dateTime.Minute));
            Assert.That(dateAndTime.DateTime.Second, Is.EqualTo(dateTime.Second));

            Assert.That(dateAndTime.DateOnly.Year, Is.EqualTo(dateOnly.Year));
            Assert.That(dateAndTime.DateOnly.Month, Is.EqualTo(dateOnly.Month));
            Assert.That(dateAndTime.DateOnly.Day, Is.EqualTo(dateOnly.Day));

            //Assert.That(dateAndTime.TimeOnly.Hour, Is.EqualTo(timeOnly.Hour));
            //Assert.That(dateAndTime.TimeOnly.Minute, Is.EqualTo(timeOnly.Minute));
            //Assert.That(dateAndTime.TimeOnly.Second, Is.EqualTo(timeOnly.Second));

            #endregion
        }
    }
}
