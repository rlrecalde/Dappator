using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;

namespace Dappator.Test
{
    public class FunctionalContextSqlite : See_Firebird_TestBase<Providers.SqliteProvider>
    {
        private SqliteConnection _sqliteConnection;

        [SetUp]
        public void Setup()
        {
            string connectionString = "DataSource=:memory:";
            this._sqliteConnection = new SqliteConnection(connectionString);
            this._sqliteConnection.Open();

            string createUser = this._provider.GetCreateUserTableQuery();
            DbCommand dbCommand = this._provider.GetCommand(createUser, this._sqliteConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            string createUserValue = this._provider.GetCreateUserValueTableQuery();
            dbCommand = this._provider.GetCommand(createUserValue, this._sqliteConnection);
            dbCommand.ExecuteNonQuery();
            dbCommand.Dispose();

            string createDateAndTime = "" +
                "CREATE TABLE DateAndTime (" +
                "   Id INTEGER PRIMARY KEY, " +
                "   [DateTime] TEXT NOT NULL, " +
                "   [DateOnly] TEXT NOT NULL, " +
                "   [TimeOnly] TEXT NOT NULL" +
                ");";

            var sqliteCommand = this._sqliteConnection.CreateCommand();
            sqliteCommand.CommandText = createDateAndTime;
            sqliteCommand.ExecuteNonQuery();
            sqliteCommand.Dispose();
        }

        [TearDown]
        public void TearDown()
        {
            this._sqliteConnection.Close();
            this._sqliteConnection.Dispose();
        }

        #region Insert, Update, Delete, Select

        [Test]
        public void Insert()
        {
            #region Arrange

            var userToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            int userId = (int)sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Firebirdite.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public async Task Insert_Async()
        {
            #region Arrange

            var userToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            int userId = (int)await sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Firebirdite.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public void Insert_Bulk()
        {
            #region Arrange

            var user1ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.Firebirdite.User> { user1ToInsert, user2ToInsert };

            var arrayParams = new object[][] {
                new object[] { user1ToInsert.Nick, user1ToInsert.Password },
                new object[] { user2ToInsert.Nick, user2ToInsert.Password },
            };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            int oneUserId = (int)sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, arrayParams)
                .ExecuteScalar();

            #endregion

            #region Assert

            Assert.That(oneUserId, Is.GreaterThan(0));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);

            foreach (var userToInsert in usersToInsert)
            {
                Model.Firebirdite.User user = users.FirstOrDefault(x => x.Nick == userToInsert.Nick && x.Password == userToInsert.Password);

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

            var user1ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.Firebirdite.User> { user1ToInsert, user2ToInsert };

            var arrayParams = new object[][] {
                new object[] { user1ToInsert.Nick, user1ToInsert.Password },
                new object[] { user2ToInsert.Nick, user2ToInsert.Password },
            };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            int oneUserId = (int)await sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, arrayParams)
                .ExecuteScalarAsync();

            #endregion

            #region Assert

            Assert.That(oneUserId, Is.GreaterThan(0));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Firebirdite.User user = users.FirstOrDefault(x => x.Nick == userToInsert.Nick && x.Password == userToInsert.Password);

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

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUserSqlite(user, this._sqliteConnection);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            int rowNumber = sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Firebirdite.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Update_Async()
        {
            #region Arrange

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUserSqlite(user, this._sqliteConnection);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            int rowNumber = await sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Firebirdite.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Delete()
        {
            #region Arrange

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUserSqlite(user, this._sqliteConnection);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            int rowNumber = sqlContext
                .Delete<Model.Firebirdite.User>()
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Delete_Async()
        {
            #region Arrange

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUserSqlite(user, this._sqliteConnection);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            int rowNumber = await sqlContext
                .Delete<Model.Firebirdite.User>()
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Select()
        {
            #region Arrange

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.Firebirdite.User> users = sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .Query<Model.Firebirdite.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public async Task Select_Async()
        {
            #region Arrange

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.Firebirdite.User> users = await sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .QueryAsync<Model.Firebirdite.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public void Select_Where()
        {
            #region Arrange

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.Firebirdite.User> users = sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user2.Id)
                .Query<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.Firebirdite.User> users = await sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user2.Id)
                .QueryAsync<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.Firebirdite.User> users = sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Firebirdite.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .Query<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.Firebirdite.User> users = await sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Firebirdite.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryAsync<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.Firebirdite.User> users = sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Firebirdite.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1, 1)
                .Query<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.Firebirdite.User> users = await sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Firebirdite.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1, 1)
                .QueryAsync<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            Model.Firebirdite.User user = sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Firebirdite.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            Model.Firebirdite.User user = await sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.Firebirdite.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            string query = sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .GetQuery();

            IEnumerable<Model.Firebirdite.User> users = sqlContext
                .SetQuery(query)
                .Query<Model.Firebirdite.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public async Task Select_With_Query_Async()
        {
            #region Arrange

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            string query = sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .GetQuery();

            IEnumerable<Model.Firebirdite.User> users = await sqlContext
                .SetQuery(query)
                .QueryAsync<Model.Firebirdite.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public void Select_Not_Buffered()
        {
            #region Arrange

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);
            sqlContext.Buffered = false;

            IEnumerable<Model.Firebirdite.User> users = sqlContext
                .Select<Model.Firebirdite.User>(x => new { x.Id, x.Nick, x.Password })
                .Query<Model.Firebirdite.User>();

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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Count<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Count<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Max<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Max<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Min<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Min<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Sum<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Sum<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Avg<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1 = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUserSqlite(user1, this._sqliteConnection);

            var user2 = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUserSqlite(user2, this._sqliteConnection);

            var user1Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValueSqlite(user1Value1, this._sqliteConnection);

            var user1Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValueSqlite(user1Value2, this._sqliteConnection);

            var user1Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValueSqlite(user1Value3, this._sqliteConnection);

            var user2Value1 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValueSqlite(user2Value1, this._sqliteConnection);

            var user2Value2 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValueSqlite(user2Value2, this._sqliteConnection);

            var user2Value3 = new Model.Firebirdite.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValueSqlite(user2Value3, this._sqliteConnection);

            var userValues = new List<Model.Firebirdite.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await sqlContext
                .Select<Model.Firebirdite.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Avg<Model.Firebirdite.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.Firebirdite.UserValue>(x => x.UserId)
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

            var user1ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            var user2ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.Firebirdite.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int user1Id = (int)sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalar();

            user1ToInsert.Id = user1Id;

            int user2Id = (int)sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalar();

            user2ToInsert.Id = user2Id;

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Firebirdite.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

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

            var user1ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.Firebirdite.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int user1Id = (int)await sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalarAsync();

            user1ToInsert.Id = user1Id;

            int user2Id = (int)await sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalarAsync();

            user2ToInsert.Id = user2Id;

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.Firebirdite.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

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

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUserSqlite(user, this._sqliteConnection);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int rowNumber1 = sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            nickToUpdate += "_extra";
            passwordToUpdate += "_extra";

            int rowNumber2 = sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Firebirdite.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Update_In_Transaction_Async()
        {
            #region Arrange

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUserSqlite(user, this._sqliteConnection);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int rowNumber1 = await sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            nickToUpdate += "_extra";
            passwordToUpdate += "_extra";

            int rowNumber2 = await sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Firebirdite.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Delete_In_Transaction()
        {
            #region Arrange

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUserSqlite(user, this._sqliteConnection);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int rowNumber1 = sqlContext
                .Delete<Model.Firebirdite.User>()
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            int rowNumber2 = sqlContext
                .Delete<Model.Firebirdite.User>()
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(0));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Delete_In_Transaction_Async()
        {
            #region Arrange

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUserSqlite(user, this._sqliteConnection);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int rowNumber1 = await sqlContext
                .Delete<Model.Firebirdite.User>()
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            int rowNumber2 = await sqlContext
                .Delete<Model.Firebirdite.User>()
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(0));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Insert_Update_In_Transaction()
        {
            #region Arrange

            var userToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{userToInsert.Nick}_sarasa";
            string passwordToUpdate = $"{userToInsert.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int userId = (int)sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            userToInsert.Id = userId;

            int rowNumber = sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, userId)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Firebirdite.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(userToInsert.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Insert_Update_In_Transaction_Async()
        {
            #region Arrange

            var userToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{userToInsert.Nick}_sarasa";
            string passwordToUpdate = $"{userToInsert.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int userId = (int)await sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            userToInsert.Id = userId;

            int rowNumber = await sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.Firebirdite.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(userToInsert.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Insert_Delete_In_Transaction()
        {
            #region Arrange

            var userToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int userId = (int)sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            userToInsert.Id = userId;

            int rowNumber = sqlContext
                .Delete<Model.Firebirdite.User>()
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, userId)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Insert_Delete_In_Transaction_Async()
        {
            #region Arrange

            var userToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int userId = (int)await sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            userToInsert.Id = userId;

            int rowNumber = await sqlContext
                .Delete<Model.Firebirdite.User>()
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Insert_Update_In_Transaction_2_Same_Connection()
        {
            #region Arrange

            var user1ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{user1ToInsert.Nick}_sarasa";
            string passwordToUpdate = $"{user1ToInsert.Password}_sarasa";

            var user2ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick2ToUpdate = $"{user2ToInsert.Nick}_sarasa";
            string password2ToUpdate = $"{user2ToInsert.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int user1Id = (int)sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalar();

            user1ToInsert.Id = user1Id;

            int rowNumber1 = sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user1ToInsert.Id)
                .Execute();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            // Here we don't set 'ExecuteInTransaction' property to false
            // so, a new DbTransaction object will be created for the following queries.

            IEnumerable<Model.Firebirdite.User> usersFirst = base.GetUsersSqlite(this._sqliteConnection);

            int user2Id = (int)sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalar();

            user2ToInsert.Id = user2Id;

            int rowNumber2 = sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nick2ToUpdate, password2ToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user2ToInsert.Id)
                .Execute();

            dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            IEnumerable<Model.Firebirdite.User> usersSecond = base.GetUsersSqlite(this._sqliteConnection);

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

            var user1ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick1ToUpdate = $"{user1ToInsert.Nick}_sarasa";
            string password1ToUpdate = $"{user1ToInsert.Password}_sarasa";

            var user2ToInsert = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick2ToUpdate = $"{user2ToInsert.Nick}_sarasa";
            string password2ToUpdate = $"{user2ToInsert.Password}_sarasa";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;

            int user1Id = (int)await sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalarAsync();

            user1ToInsert.Id = user1Id;

            int rowNumber1 = await sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nick1ToUpdate, password1ToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user1ToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            // Here we don't set 'ExecuteInTransaction' property to false
            // so, a new DbTransaction object will be created for the following queries.

            IEnumerable<Model.Firebirdite.User> usersFirst = base.GetUsersSqlite(this._sqliteConnection);

            int user2Id = (int)await sqlContext
                .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalarAsync();

            user2ToInsert.Id = user2Id;

            int rowNumber2 = await sqlContext
                .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nick2ToUpdate, password2ToUpdate)
                .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, user2ToInsert.Id)
                .ExecuteAsync();

            dbTransaction = sqlContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            sqlContext.ExecuteInTransaction = false;

            IEnumerable<Model.Firebirdite.User> usersSecond = base.GetUsersSqlite(this._sqliteConnection);

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

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";
            string exceptionMessage = "Entity not found";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;
            DbTransaction dbTransaction = null;

            var exception = Assert.Throws<Exception>(() =>
            {
                try
                {
                    int userId = (int)sqlContext
                        .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                        .ExecuteScalar();

                    int rowNumber = sqlContext
                        .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                        .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, -1)
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

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Transaction_Fails_Async()
        {
            #region Arrange

            var user = new Model.Firebirdite.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";
            string exceptionMessage = "Entity not found";

            #endregion

            #region Act

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            sqlContext.ExecuteInTransaction = true;
            DbTransaction dbTransaction = null;

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                try
                {
                    int userId = (int)await sqlContext
                        .Insert<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                        .ExecuteScalarAsync();

                    int rowNumber = await sqlContext
                        .Update<Model.Firebirdite.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                        .Where<Model.Firebirdite.User>(x => x.Id, Common.Operators.EqualTo, -1)
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

            IEnumerable<Model.Firebirdite.User> users = base.GetUsersSqlite(this._sqliteConnection);
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

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            Interfaces.IException exception = null;

            try
            {
                int userValueId = (int)sqlContext
                    .Insert<Model.Firebirdite.UserValue>(x => new { x.UserId, x.Value }, userId, value)
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

            using var sqlContext = new SqliteContext(this._sqliteConnection, preventClosing: true);

            Interfaces.IException exception = null;

            try
            {
                int userValueId = (int)await sqlContext
                    .Insert<Model.Firebirdite.UserValue>(x => new { x.UserId, x.Value }, userId, value)
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

        [Test]
        public void Insert_Dates()
        {
            #region Arrange

            var dateTime = new DateTime(2001, 2, 3, 4, 5, 6);
            var dateOnly = new DateOnly(2002, 3, 4);
            var timeOnly = new TimeOnly(7, 8, 9);

            using var sqlContext = new SqliteContext(this._sqliteConnection);

            #endregion

            #region Act

            int id = (int)sqlContext
                .Insert<Model.Firebirdite.DateAndTime>(x => new
                { x.DateTime, x.DateOnly, x.TimeOnly },
                    dateTime, dateOnly, timeOnly)
                .ExecuteScalar();

            #endregion

            #region Assert

            Assert.That(id, Is.GreaterThan(0));

            Model.Firebirdite.DateAndTime dateAndTimeString = sqlContext
                .Select<Model.Firebirdite.DateAndTime>(x => new { x.Id, x.DateTime, x.DateOnly, x.TimeOnly })
                .Where<Model.Firebirdite.DateAndTime>(x => x.Id, Common.Operators.EqualTo, id)
                .OrderBy<Model.Firebirdite.DateAndTime>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.Firebirdite.DateAndTime>();

            Assert.IsNotNull(dateAndTimeString);

            var dateTimeReceived = Convert.ToDateTime(dateAndTimeString.DateTime);
            var dateOnlyReceived = DateOnly.Parse(dateAndTimeString.DateOnly);
            var timeOnlyReceived = TimeOnly.Parse(dateAndTimeString.TimeOnly);

            Assert.That(dateTimeReceived.Year, Is.EqualTo(dateTime.Year));
            Assert.That(dateTimeReceived.Month, Is.EqualTo(dateTime.Month));
            Assert.That(dateTimeReceived.Day, Is.EqualTo(dateTime.Day));
            Assert.That(dateTimeReceived.Hour, Is.EqualTo(dateTime.Hour));
            Assert.That(dateTimeReceived.Minute, Is.EqualTo(dateTime.Minute));
            Assert.That(dateTimeReceived.Second, Is.EqualTo(dateTime.Second));

            Assert.That(dateOnlyReceived.Year, Is.EqualTo(dateOnly.Year));
            Assert.That(dateOnlyReceived.Month, Is.EqualTo(dateOnly.Month));
            Assert.That(dateOnlyReceived.Day, Is.EqualTo(dateOnly.Day));

            Assert.That(timeOnlyReceived.Hour, Is.EqualTo(timeOnly.Hour));
            Assert.That(timeOnlyReceived.Minute, Is.EqualTo(timeOnly.Minute));
            Assert.That(timeOnlyReceived.Second, Is.EqualTo(timeOnly.Second));

            #endregion
        }
    }
}
