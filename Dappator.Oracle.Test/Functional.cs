using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dappator.Oracle.Test
{
    public class Functional : TestBase
    {
        [SetUp]
        public void Setup()
        {
            base._connectionString = "Data Source=localhost/FREE;User ID=system;Password=guest";

            base.CreateTables();
        }

        #region Insert, Update, Delete, Select

        [Test]
        public void Insert()
        {
            #region Arrange

            var userToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int userId = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public async Task Insert_Async()
        {
            #region Arrange

            var userToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int userId = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public void Insert_With_Object()
        {
            #region Arrange

            var userToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int userId = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, userToInsert)
                .ExecuteScalar();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public async Task Insert_With_Object_Async()
        {
            #region Arrange

            var userToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int userId = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, userToInsert)
                .ExecuteScalarAsync();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(userId, Is.GreaterThan(0));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.EqualTo(userId));
            Assert.That(insertedUser.Nick, Is.EqualTo(userToInsert.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(userToInsert.Password));

            #endregion
        }

        [Test]
        public void Insert_Bulk()
        {
            #region Arrange

            var user1ToInsert = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            var arrayParams = new object[][] {
                new object[] { user1ToInsert.Nick, user1ToInsert.Password },
                new object[] { user2ToInsert.Nick, user2ToInsert.Password },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int oneUserId = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, arrayParams)
                .ExecuteScalar();

            dataContext.Dispose();

            #endregion

            #region Assert

            //Assert.That(oneUserId, Is.GreaterThan(0));
            // PL/SQL: ORA-63809: returning clause is not allowed with INSERT and Table Value Constructor

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.User user = users.FirstOrDefault(x => x.Nick == userToInsert.Nick && x.Password == userToInsert.Password);

                Assert.IsNotNull(user);
                Assert.That(user.Id, Is.GreaterThan(0));
            }

            //int lastUserIdInDB = users.Max(x => x.Id);
            //Assert.That(oneUserId, Is.EqualTo(lastUserIdInDB));

            #endregion
        }

        [Test]
        public async Task Insert_Bulk_Async()
        {
            #region Arrange

            var user1ToInsert = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            var arrayParams = new object[][] {
                new object[] { user1ToInsert.Nick, user1ToInsert.Password },
                new object[] { user2ToInsert.Nick, user2ToInsert.Password },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int oneUserId = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, arrayParams)
                .ExecuteScalarAsync();

            dataContext.Dispose();

            #endregion

            #region Assert

            //Assert.That(oneUserId, Is.GreaterThan(0));
            // PL/SQL: ORA-63809: returning clause is not allowed with INSERT and Table Value Constructor

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.User user = users.FirstOrDefault(x => x.Nick == userToInsert.Nick && x.Password == userToInsert.Password);

                Assert.IsNotNull(user);
                Assert.That(user.Id, Is.GreaterThan(0));
            }

            //int lastUserIdInDB = users.Max(x => x.Id);
            //Assert.That(oneUserId, Is.EqualTo(lastUserIdInDB));

            #endregion
        }

        [Test]
        public void Insert_Bulk_With_Object()
        {
            #region Arrange

            var user1ToInsert = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            var arrayParams = new object[][] {
                new object[] { user1ToInsert.Nick, user1ToInsert.Password },
                new object[] { user2ToInsert.Nick, user2ToInsert.Password },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int oneUserId = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, usersToInsert)
                .ExecuteScalar();

            dataContext.Dispose();

            #endregion

            #region Assert

            //Assert.That(oneUserId, Is.GreaterThan(0));
            // PL/SQL: ORA-63809: returning clause is not allowed with INSERT and Table Value Constructor

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.User user = users.FirstOrDefault(x => x.Nick == userToInsert.Nick && x.Password == userToInsert.Password);

                Assert.IsNotNull(user);
                Assert.That(user.Id, Is.GreaterThan(0));
            }

            //int lastUserIdInDB = users.Max(x => x.Id);
            //Assert.That(oneUserId, Is.EqualTo(lastUserIdInDB));

            #endregion
        }

        [Test]
        public async Task Insert_Bulk_With_Object_Async()
        {
            #region Arrange

            var user1ToInsert = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            var arrayParams = new object[][] {
                new object[] { user1ToInsert.Nick, user1ToInsert.Password },
                new object[] { user2ToInsert.Nick, user2ToInsert.Password },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int oneUserId = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, usersToInsert)
                .ExecuteScalarAsync();

            dataContext.Dispose();

            #endregion

            #region Assert

            //Assert.That(oneUserId, Is.GreaterThan(0));
            // PL/SQL: ORA-63809: returning clause is not allowed with INSERT and Table Value Constructor

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.User user = users.FirstOrDefault(x => x.Nick == userToInsert.Nick && x.Password == userToInsert.Password);

                Assert.IsNotNull(user);
                Assert.That(user.Id, Is.GreaterThan(0));
            }

            //int lastUserIdInDB = users.Max(x => x.Id);
            //Assert.That(oneUserId, Is.EqualTo(lastUserIdInDB));

            #endregion
        }

        [Test]
        public void Insert_Returning_Another_Column()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            var userValue = new Model.UserValue
            {
                UserId = user.Id,
                Value = 1.7,
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int userId = (int)dataContext
                .Insert<Model.UserValue>(x => new { x.UserId, x.Value }, userValue.UserId, userValue.Value)
                .Returning<Model.UserValue>(x => x.UserId)
                .ExecuteScalar();

            #endregion

            #region Assert

            Assert.That(userId, Is.EqualTo(user.Id));

            #endregion
        }

        [Test]
        public async Task Insert_Returning_Another_Column_Async()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            var userValue = new Model.UserValue
            {
                UserId = user.Id,
                Value = 1.7,
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int userId = (int)await dataContext
                .Insert<Model.UserValue>(x => new { x.UserId, x.Value }, userValue.UserId, userValue.Value)
                .Returning<Model.UserValue>(x => x.UserId)
                .ExecuteScalarAsync();

            #endregion

            #region Assert

            Assert.That(userId, Is.EqualTo(user.Id));

            #endregion
        }

        [Test]
        public void Update()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int rowNumber = dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Update_Async()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int rowNumber = await dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Update_With_Object()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            var userToUpdate = new Model.User
            {
                Nick = $"{user.Nick}_sarasa",
                Password = $"{user.Password}_sarasa",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int rowNumber = dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, userToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(userToUpdate.Nick));
            Assert.That(updatedUser.Password, Is.EqualTo(userToUpdate.Password));

            #endregion
        }

        [Test]
        public async Task Update_With_Object_Async()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            var userToUpdate = new Model.User
            {
                Nick = $"{user.Nick}_sarasa",
                Password = $"{user.Password}_sarasa",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int rowNumber = await dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, userToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(userToUpdate.Nick));
            Assert.That(updatedUser.Password, Is.EqualTo(userToUpdate.Password));

            #endregion
        }

        [Test]
        public void Insert_Update_Many_Types()
        {
            #region Arrange

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            var dataType = new Model.DataType
            {
                Byte = byte.MaxValue,
                Sbyte = sbyte.MinValue,
                Short = short.MinValue,
                Ushort = ushort.MaxValue,
                Int = int.MinValue,
                Uint = uint.MaxValue,
                Long = long.MinValue,
                Ulong = (ulong)long.MaxValue,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = true,
                String = "Any string",
                Char = 'C',
                Guid = guid1.ToString(),
                DateTime = new DateTime(2024, 3, 30, 21, 2, 23),
                DateTimeOffset = new DateTimeOffset(new DateTime(2024, 3, 30, 21, 2, 23).ToUniversalTime()),
                TimeSpan = new TimeSpan(1, 2, 3),
                Bytes = new byte[] { 4, 5, 6 },
            };

            var dataTypeToUpdate = new Model.DataType
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F + 1,
                Double = (double)789.126F - 1,
                Decimal = 789.124M + 1,
                Currency = 789.13F + 1,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid2.ToString(),
                DateTime = dataType.DateTime.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = dataType.DateTimeOffset.Add(new TimeSpan(3, 3, 0)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int dataTypeId = (int)dataContext
                .Insert<Model.DataType>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataType.Byte, dataType.Sbyte, dataType.Short, dataType.Ushort, dataType.Int, dataType.Uint, dataType.Long, dataType.Ulong, dataType.Float, dataType.Double, dataType.Decimal, dataType.Currency, dataType.Bool, dataType.String, dataType.Char, guid1.ToString(), dataType.DateTime, dataType.DateTimeOffset, dataType.TimeSpan, dataType.Bytes)
                .ExecuteScalar();

            Model.DataType insertedDataType = dataContext
                .Select<Model.DataType>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataType>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataType>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.DataType>();

            dataContext
                .Update<Model.DataType>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataTypeToUpdate.Byte, dataTypeToUpdate.Sbyte, dataTypeToUpdate.Short, dataTypeToUpdate.Ushort, dataTypeToUpdate.Int, dataTypeToUpdate.Uint, dataTypeToUpdate.Long, dataTypeToUpdate.Ulong, dataTypeToUpdate.Float, dataTypeToUpdate.Double, dataTypeToUpdate.Decimal, dataTypeToUpdate.Currency, dataTypeToUpdate.Bool, dataTypeToUpdate.String, dataTypeToUpdate.Char, guid2.ToString(), dataTypeToUpdate.DateTime, dataTypeToUpdate.DateTimeOffset, dataTypeToUpdate.TimeSpan, dataTypeToUpdate.Bytes)
                .Where<Model.DataType>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .Execute();

            Model.DataType updatedDataType = dataContext
                .Select<Model.DataType>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataType>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataType>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.DataType>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.IsNotNull(insertedDataType);
            Assert.That(insertedDataType.Id, Is.EqualTo(dataTypeId));
            Assert.That(insertedDataType.Byte, Is.EqualTo(dataType.Byte));
            Assert.That(insertedDataType.Sbyte, Is.EqualTo(dataType.Sbyte));
            Assert.That(insertedDataType.Short, Is.EqualTo(dataType.Short));
            Assert.That(insertedDataType.Ushort, Is.EqualTo(dataType.Ushort));
            Assert.That(insertedDataType.Int, Is.EqualTo(dataType.Int));
            Assert.That(insertedDataType.Uint, Is.EqualTo(dataType.Uint));
            Assert.That(insertedDataType.Long, Is.EqualTo(dataType.Long));
            Assert.That(insertedDataType.Ulong, Is.EqualTo(dataType.Ulong));
            Assert.That(insertedDataType.Float, Is.EqualTo(dataType.Float));
            Assert.That(insertedDataType.Double, Is.EqualTo(dataType.Double));
            Assert.That(insertedDataType.Decimal, Is.EqualTo(dataType.Decimal));
            Assert.That(insertedDataType.Currency, Is.EqualTo(dataType.Currency));
            Assert.That(insertedDataType.Bool, Is.EqualTo(dataType.Bool));
            Assert.That(insertedDataType.String, Is.EqualTo(dataType.String));
            Assert.That(insertedDataType.Char, Is.EqualTo(dataType.Char));
            Assert.That(insertedDataType.Guid.ToUpper(), Is.EqualTo(dataType.Guid.ToUpper()));
            Assert.That(insertedDataType.DateTime.Year, Is.EqualTo(dataType.DateTime.Year));
            Assert.That(insertedDataType.DateTime.Month, Is.EqualTo(dataType.DateTime.Month));
            Assert.That(insertedDataType.DateTime.Day, Is.EqualTo(dataType.DateTime.Day));
            Assert.That(insertedDataType.DateTime.Hour, Is.EqualTo(dataType.DateTime.Hour));
            Assert.That(insertedDataType.DateTime.Minute, Is.EqualTo(dataType.DateTime.Minute));
            Assert.That(insertedDataType.DateTime.Second, Is.EqualTo(dataType.DateTime.Second));
            Assert.That(insertedDataType.DateTimeOffset.Year, Is.EqualTo(dataType.DateTimeOffset.Year));
            Assert.That(insertedDataType.DateTimeOffset.Month, Is.EqualTo(dataType.DateTimeOffset.Month));
            Assert.That(insertedDataType.DateTimeOffset.Day, Is.EqualTo(dataType.DateTimeOffset.Day));
            Assert.That(insertedDataType.DateTimeOffset.Hour, Is.EqualTo(dataType.DateTimeOffset.Hour));
            Assert.That(insertedDataType.DateTimeOffset.Minute, Is.EqualTo(dataType.DateTimeOffset.Minute));
            Assert.That(insertedDataType.DateTimeOffset.Second, Is.EqualTo(dataType.DateTimeOffset.Second));
            Assert.That(insertedDataType.TimeSpan, Is.EqualTo(dataType.TimeSpan));

            for (int i = 0; i < dataType.Bytes.Length; i++)
                Assert.That(insertedDataType.Bytes[i], Is.EqualTo(dataType.Bytes[i]));

            Assert.IsNotNull(updatedDataType);
            Assert.That(updatedDataType.Byte, Is.EqualTo(dataTypeToUpdate.Byte));
            Assert.That(updatedDataType.Sbyte, Is.EqualTo(dataTypeToUpdate.Sbyte));
            Assert.That(updatedDataType.Short, Is.EqualTo(dataTypeToUpdate.Short));
            Assert.That(updatedDataType.Ushort, Is.EqualTo(dataTypeToUpdate.Ushort));
            Assert.That(updatedDataType.Int, Is.EqualTo(dataTypeToUpdate.Int));
            Assert.That(updatedDataType.Uint, Is.EqualTo(dataTypeToUpdate.Uint));
            Assert.That(updatedDataType.Long, Is.EqualTo(dataTypeToUpdate.Long));
            Assert.That(updatedDataType.Ulong, Is.EqualTo(dataTypeToUpdate.Ulong));
            Assert.That(updatedDataType.Float, Is.EqualTo(dataTypeToUpdate.Float));
            Assert.That(updatedDataType.Double, Is.EqualTo(dataTypeToUpdate.Double));
            Assert.That(updatedDataType.Decimal, Is.EqualTo(dataTypeToUpdate.Decimal));
            Assert.That(updatedDataType.Currency, Is.EqualTo(dataTypeToUpdate.Currency));
            Assert.That(updatedDataType.Bool, Is.EqualTo(dataTypeToUpdate.Bool));
            Assert.That(updatedDataType.String, Is.EqualTo(dataTypeToUpdate.String));
            Assert.That(updatedDataType.Char, Is.EqualTo(dataTypeToUpdate.Char));
            Assert.That(updatedDataType.Guid.ToUpper(), Is.EqualTo(dataTypeToUpdate.Guid.ToUpper()));
            Assert.That(updatedDataType.DateTime.Year, Is.EqualTo(dataTypeToUpdate.DateTime.Year));
            Assert.That(updatedDataType.DateTime.Month, Is.EqualTo(dataTypeToUpdate.DateTime.Month));
            Assert.That(updatedDataType.DateTime.Day, Is.EqualTo(dataTypeToUpdate.DateTime.Day));
            Assert.That(updatedDataType.DateTime.Hour, Is.EqualTo(dataTypeToUpdate.DateTime.Hour));
            Assert.That(updatedDataType.DateTime.Minute, Is.EqualTo(dataTypeToUpdate.DateTime.Minute));
            Assert.That(updatedDataType.DateTime.Second, Is.EqualTo(dataTypeToUpdate.DateTime.Second));
            Assert.That(updatedDataType.DateTimeOffset.Year, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Year));
            Assert.That(updatedDataType.DateTimeOffset.Month, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Month));
            Assert.That(updatedDataType.DateTimeOffset.Day, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Day));
            Assert.That(updatedDataType.DateTimeOffset.Hour, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Hour));
            Assert.That(updatedDataType.DateTimeOffset.Minute, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Minute));
            Assert.That(updatedDataType.DateTimeOffset.Second, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Second));
            Assert.That(updatedDataType.TimeSpan, Is.EqualTo(dataTypeToUpdate.TimeSpan));

            for (int i = 0; i < dataTypeToUpdate.Bytes.Length; i++)
                Assert.That(updatedDataType.Bytes[i], Is.EqualTo(dataTypeToUpdate.Bytes[i]));

            #endregion
        }

        [Test]
        public async Task Insert_Update_Many_Types_Async()
        {
            #region Arrange

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            var dataType = new Model.DataType
            {
                Byte = byte.MaxValue,
                Sbyte = sbyte.MinValue,
                Short = short.MinValue,
                Ushort = ushort.MaxValue,
                Int = int.MinValue,
                Uint = uint.MaxValue,
                Long = long.MinValue,
                Ulong = (ulong)long.MaxValue,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = true,
                String = "Any string",
                Char = 'C',
                Guid = guid1.ToString(),
                DateTime = new DateTime(2024, 3, 30, 21, 2, 23),
                DateTimeOffset = new DateTimeOffset(new DateTime(2024, 3, 30, 21, 2, 23).ToUniversalTime()),
                TimeSpan = new TimeSpan(1, 2, 3),
                Bytes = new byte[] { 4, 5, 6 },
            };

            var dataTypeToUpdate = new Model.DataType
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F + 1,
                Double = (double)789.126F - 1,
                Decimal = 789.124M + 1,
                Currency = 789.13F + 1,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid2.ToString(),
                DateTime = dataType.DateTime.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = dataType.DateTimeOffset.Add(new TimeSpan(3, 3, 0)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int dataTypeId = (int)await dataContext
                .Insert<Model.DataType>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataType.Byte, dataType.Sbyte, dataType.Short, dataType.Ushort, dataType.Int, dataType.Uint, dataType.Long, dataType.Ulong, dataType.Float, dataType.Double, dataType.Decimal, dataType.Currency, dataType.Bool, dataType.String, dataType.Char, guid1.ToString(), dataType.DateTime, dataType.DateTimeOffset, dataType.TimeSpan, dataType.Bytes)
                .ExecuteScalarAsync();

            Model.DataType insertedDataType = await dataContext
                .Select<Model.DataType>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataType>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataType>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.DataType>();

            await dataContext
                .Update<Model.DataType>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataTypeToUpdate.Byte, dataTypeToUpdate.Sbyte, dataTypeToUpdate.Short, dataTypeToUpdate.Ushort, dataTypeToUpdate.Int, dataTypeToUpdate.Uint, dataTypeToUpdate.Long, dataTypeToUpdate.Ulong, dataTypeToUpdate.Float, dataTypeToUpdate.Double, dataTypeToUpdate.Decimal, dataTypeToUpdate.Currency, dataTypeToUpdate.Bool, dataTypeToUpdate.String, dataTypeToUpdate.Char, guid2.ToString(), dataTypeToUpdate.DateTime, dataTypeToUpdate.DateTimeOffset, dataTypeToUpdate.TimeSpan, dataTypeToUpdate.Bytes)
                .Where<Model.DataType>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .ExecuteAsync();

            Model.DataType updatedDataType = await dataContext
                .Select<Model.DataType>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataType>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataType>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.DataType>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.IsNotNull(insertedDataType);
            Assert.That(insertedDataType.Id, Is.EqualTo(dataTypeId));
            Assert.That(insertedDataType.Byte, Is.EqualTo(dataType.Byte));
            Assert.That(insertedDataType.Sbyte, Is.EqualTo(dataType.Sbyte));
            Assert.That(insertedDataType.Short, Is.EqualTo(dataType.Short));
            Assert.That(insertedDataType.Ushort, Is.EqualTo(dataType.Ushort));
            Assert.That(insertedDataType.Int, Is.EqualTo(dataType.Int));
            Assert.That(insertedDataType.Uint, Is.EqualTo(dataType.Uint));
            Assert.That(insertedDataType.Long, Is.EqualTo(dataType.Long));
            Assert.That(insertedDataType.Ulong, Is.EqualTo(dataType.Ulong));
            Assert.That(insertedDataType.Float, Is.EqualTo(dataType.Float));
            Assert.That(insertedDataType.Double, Is.EqualTo(dataType.Double));
            Assert.That(insertedDataType.Decimal, Is.EqualTo(dataType.Decimal));
            Assert.That(insertedDataType.Currency, Is.EqualTo(dataType.Currency));
            Assert.That(insertedDataType.Bool, Is.EqualTo(dataType.Bool));
            Assert.That(insertedDataType.String, Is.EqualTo(dataType.String));
            Assert.That(insertedDataType.Char, Is.EqualTo(dataType.Char));
            Assert.That(insertedDataType.Guid.ToUpper(), Is.EqualTo(dataType.Guid.ToUpper()));
            Assert.That(insertedDataType.DateTime.Year, Is.EqualTo(dataType.DateTime.Year));
            Assert.That(insertedDataType.DateTime.Month, Is.EqualTo(dataType.DateTime.Month));
            Assert.That(insertedDataType.DateTime.Day, Is.EqualTo(dataType.DateTime.Day));
            Assert.That(insertedDataType.DateTime.Hour, Is.EqualTo(dataType.DateTime.Hour));
            Assert.That(insertedDataType.DateTime.Minute, Is.EqualTo(dataType.DateTime.Minute));
            Assert.That(insertedDataType.DateTime.Second, Is.EqualTo(dataType.DateTime.Second));
            Assert.That(insertedDataType.DateTimeOffset.Year, Is.EqualTo(dataType.DateTimeOffset.Year));
            Assert.That(insertedDataType.DateTimeOffset.Month, Is.EqualTo(dataType.DateTimeOffset.Month));
            Assert.That(insertedDataType.DateTimeOffset.Day, Is.EqualTo(dataType.DateTimeOffset.Day));
            Assert.That(insertedDataType.DateTimeOffset.Hour, Is.EqualTo(dataType.DateTimeOffset.Hour));
            Assert.That(insertedDataType.DateTimeOffset.Minute, Is.EqualTo(dataType.DateTimeOffset.Minute));
            Assert.That(insertedDataType.DateTimeOffset.Second, Is.EqualTo(dataType.DateTimeOffset.Second));
            Assert.That(insertedDataType.TimeSpan, Is.EqualTo(dataType.TimeSpan));

            for (int i = 0; i < dataType.Bytes.Length; i++)
                Assert.That(insertedDataType.Bytes[i], Is.EqualTo(dataType.Bytes[i]));

            Assert.IsNotNull(updatedDataType);
            Assert.That(updatedDataType.Byte, Is.EqualTo(dataTypeToUpdate.Byte));
            Assert.That(updatedDataType.Sbyte, Is.EqualTo(dataTypeToUpdate.Sbyte));
            Assert.That(updatedDataType.Short, Is.EqualTo(dataTypeToUpdate.Short));
            Assert.That(updatedDataType.Ushort, Is.EqualTo(dataTypeToUpdate.Ushort));
            Assert.That(updatedDataType.Int, Is.EqualTo(dataTypeToUpdate.Int));
            Assert.That(updatedDataType.Uint, Is.EqualTo(dataTypeToUpdate.Uint));
            Assert.That(updatedDataType.Long, Is.EqualTo(dataTypeToUpdate.Long));
            Assert.That(updatedDataType.Ulong, Is.EqualTo(dataTypeToUpdate.Ulong));
            Assert.That(updatedDataType.Float, Is.EqualTo(dataTypeToUpdate.Float));
            Assert.That(updatedDataType.Double, Is.EqualTo(dataTypeToUpdate.Double));
            Assert.That(updatedDataType.Decimal, Is.EqualTo(dataTypeToUpdate.Decimal));
            Assert.That(updatedDataType.Currency, Is.EqualTo(dataTypeToUpdate.Currency));
            Assert.That(updatedDataType.Bool, Is.EqualTo(dataTypeToUpdate.Bool));
            Assert.That(updatedDataType.String, Is.EqualTo(dataTypeToUpdate.String));
            Assert.That(updatedDataType.Char, Is.EqualTo(dataTypeToUpdate.Char));
            Assert.That(updatedDataType.Guid.ToUpper(), Is.EqualTo(dataTypeToUpdate.Guid.ToUpper()));
            Assert.That(updatedDataType.DateTime.Year, Is.EqualTo(dataTypeToUpdate.DateTime.Year));
            Assert.That(updatedDataType.DateTime.Month, Is.EqualTo(dataTypeToUpdate.DateTime.Month));
            Assert.That(updatedDataType.DateTime.Day, Is.EqualTo(dataTypeToUpdate.DateTime.Day));
            Assert.That(updatedDataType.DateTime.Hour, Is.EqualTo(dataTypeToUpdate.DateTime.Hour));
            Assert.That(updatedDataType.DateTime.Minute, Is.EqualTo(dataTypeToUpdate.DateTime.Minute));
            Assert.That(updatedDataType.DateTime.Second, Is.EqualTo(dataTypeToUpdate.DateTime.Second));
            Assert.That(updatedDataType.DateTimeOffset.Year, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Year));
            Assert.That(updatedDataType.DateTimeOffset.Month, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Month));
            Assert.That(updatedDataType.DateTimeOffset.Day, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Day));
            Assert.That(updatedDataType.DateTimeOffset.Hour, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Hour));
            Assert.That(updatedDataType.DateTimeOffset.Minute, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Minute));
            Assert.That(updatedDataType.DateTimeOffset.Second, Is.EqualTo(dataTypeToUpdate.DateTimeOffset.Second));
            Assert.That(updatedDataType.TimeSpan, Is.EqualTo(dataTypeToUpdate.TimeSpan));

            for (int i = 0; i < dataTypeToUpdate.Bytes.Length; i++)
                Assert.That(updatedDataType.Bytes[i], Is.EqualTo(dataTypeToUpdate.Bytes[i]));

            #endregion
        }

        [Test]
        public void Insert_Update_Nullable_Many_Types()
        {
            #region Arrange

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            var dataType = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue,
                Sbyte = sbyte.MinValue,
                Short = short.MinValue,
                Ushort = ushort.MaxValue,
                Int = int.MinValue,
                Uint = uint.MaxValue,
                Long = long.MinValue,
                Ulong = (ulong)long.MaxValue,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = true,
                String = "Any string",
                Char = 'C',
                Guid = guid1.ToString(),
                DateTime = new DateTime(2024, 3, 30, 21, 2, 23),
                DateTimeOffset = new DateTimeOffset(new DateTime(2024, 3, 30, 21, 2, 23).ToUniversalTime()),
                TimeSpan = new TimeSpan(1, 2, 3),
                Bytes = new byte[] { 4, 5, 6 },
            };

            var dataTypeToUpdate = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F + 1,
                Double = (double)789.126F - 1,
                Decimal = 789.124M + 1,
                Currency = 789.13F + 1,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid2.ToString(),
                DateTime = dataType.DateTime?.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = dataType.DateTimeOffset?.Add(new TimeSpan(3, 3, 0)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int dataTypeId = (int)dataContext
                .Insert<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataType.Byte, dataType.Sbyte, dataType.Short, dataType.Ushort, dataType.Int, dataType.Uint, dataType.Long, dataType.Ulong, dataType.Float, dataType.Double, dataType.Decimal, dataType.Currency, dataType.Bool, dataType.String, dataType.Char, guid1.ToString(), dataType.DateTime, dataType.DateTimeOffset, dataType.TimeSpan, dataType.Bytes)
                .ExecuteScalar();

            Model.DataTypeNullable insertedDataType = dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataTypeNullable>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.DataTypeNullable>();

            dataContext
                .Update<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataTypeToUpdate.Byte, dataTypeToUpdate.Sbyte, dataTypeToUpdate.Short, dataTypeToUpdate.Ushort, dataTypeToUpdate.Int, dataTypeToUpdate.Uint, dataTypeToUpdate.Long, dataTypeToUpdate.Ulong, dataTypeToUpdate.Float, dataTypeToUpdate.Double, dataTypeToUpdate.Decimal, dataTypeToUpdate.Currency, dataTypeToUpdate.Bool, dataTypeToUpdate.String, dataTypeToUpdate.Char, guid2.ToString(), dataTypeToUpdate.DateTime, dataTypeToUpdate.DateTimeOffset, dataTypeToUpdate.TimeSpan, dataTypeToUpdate.Bytes)
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .Execute();

            Model.DataTypeNullable updatedDataType = dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataTypeNullable>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.IsNotNull(insertedDataType);
            Assert.That(insertedDataType.Id, Is.EqualTo(dataTypeId));
            Assert.That(insertedDataType.Byte, Is.EqualTo(dataType.Byte));
            Assert.That(insertedDataType.Sbyte, Is.EqualTo(dataType.Sbyte));
            Assert.That(insertedDataType.Short, Is.EqualTo(dataType.Short));
            Assert.That(insertedDataType.Ushort, Is.EqualTo(dataType.Ushort));
            Assert.That(insertedDataType.Int, Is.EqualTo(dataType.Int));
            Assert.That(insertedDataType.Uint, Is.EqualTo(dataType.Uint));
            Assert.That(insertedDataType.Long, Is.EqualTo(dataType.Long));
            Assert.That(insertedDataType.Ulong, Is.EqualTo(dataType.Ulong));
            Assert.That(insertedDataType.Float, Is.EqualTo(dataType.Float));
            Assert.That(insertedDataType.Double, Is.EqualTo(dataType.Double));
            Assert.That(insertedDataType.Decimal, Is.EqualTo(dataType.Decimal));
            Assert.That(insertedDataType.Currency, Is.EqualTo(dataType.Currency));
            Assert.That(insertedDataType.Bool, Is.EqualTo(dataType.Bool));
            Assert.That(insertedDataType.String, Is.EqualTo(dataType.String));
            Assert.That(insertedDataType.Char, Is.EqualTo(dataType.Char));
            Assert.That(insertedDataType.Guid.ToUpper(), Is.EqualTo(dataType.Guid.ToUpper()));
            Assert.IsNotNull(insertedDataType.DateTime);
            Assert.That(insertedDataType.DateTime?.Year, Is.EqualTo(dataType.DateTime?.Year));
            Assert.That(insertedDataType.DateTime?.Month, Is.EqualTo(dataType.DateTime?.Month));
            Assert.That(insertedDataType.DateTime?.Day, Is.EqualTo(dataType.DateTime?.Day));
            Assert.That(insertedDataType.DateTime?.Hour, Is.EqualTo(dataType.DateTime?.Hour));
            Assert.That(insertedDataType.DateTime?.Minute, Is.EqualTo(dataType.DateTime?.Minute));
            Assert.That(insertedDataType.DateTime?.Second, Is.EqualTo(dataType.DateTime?.Second));
            Assert.IsNotNull(insertedDataType.DateTimeOffset);
            Assert.That(insertedDataType.DateTimeOffset?.Year, Is.EqualTo(dataType.DateTimeOffset?.Year));
            Assert.That(insertedDataType.DateTimeOffset?.Month, Is.EqualTo(dataType.DateTimeOffset?.Month));
            Assert.That(insertedDataType.DateTimeOffset?.Day, Is.EqualTo(dataType.DateTimeOffset?.Day));
            Assert.That(insertedDataType.DateTimeOffset?.Hour, Is.EqualTo(dataType.DateTimeOffset?.Hour));
            Assert.That(insertedDataType.DateTimeOffset?.Minute, Is.EqualTo(dataType.DateTimeOffset?.Minute));
            Assert.That(insertedDataType.DateTimeOffset?.Second, Is.EqualTo(dataType.DateTimeOffset?.Second));
            Assert.That(insertedDataType.TimeSpan, Is.EqualTo(dataType.TimeSpan));

            for (int i = 0; i < dataType.Bytes.Length; i++)
                Assert.That(insertedDataType.Bytes[i], Is.EqualTo(dataType.Bytes[i]));

            Assert.IsNotNull(updatedDataType);
            Assert.That(updatedDataType.Byte, Is.EqualTo(dataTypeToUpdate.Byte));
            Assert.That(updatedDataType.Sbyte, Is.EqualTo(dataTypeToUpdate.Sbyte));
            Assert.That(updatedDataType.Short, Is.EqualTo(dataTypeToUpdate.Short));
            Assert.That(updatedDataType.Ushort, Is.EqualTo(dataTypeToUpdate.Ushort));
            Assert.That(updatedDataType.Int, Is.EqualTo(dataTypeToUpdate.Int));
            Assert.That(updatedDataType.Uint, Is.EqualTo(dataTypeToUpdate.Uint));
            Assert.That(updatedDataType.Long, Is.EqualTo(dataTypeToUpdate.Long));
            Assert.That(updatedDataType.Ulong, Is.EqualTo(dataTypeToUpdate.Ulong));
            Assert.That(updatedDataType.Float, Is.EqualTo(dataTypeToUpdate.Float));
            Assert.That(updatedDataType.Double, Is.EqualTo(dataTypeToUpdate.Double));
            Assert.That(updatedDataType.Decimal, Is.EqualTo(dataTypeToUpdate.Decimal));
            Assert.That(updatedDataType.Currency, Is.EqualTo(dataTypeToUpdate.Currency));
            Assert.That(updatedDataType.Bool, Is.EqualTo(dataTypeToUpdate.Bool));
            Assert.That(updatedDataType.String, Is.EqualTo(dataTypeToUpdate.String));
            Assert.That(updatedDataType.Char, Is.EqualTo(dataTypeToUpdate.Char));
            Assert.That(updatedDataType.Guid.ToUpper(), Is.EqualTo(dataTypeToUpdate.Guid.ToUpper()));
            Assert.IsNotNull(updatedDataType.DateTime);
            Assert.That(updatedDataType.DateTime?.Year, Is.EqualTo(dataTypeToUpdate.DateTime?.Year));
            Assert.That(updatedDataType.DateTime?.Month, Is.EqualTo(dataTypeToUpdate.DateTime?.Month));
            Assert.That(updatedDataType.DateTime?.Day, Is.EqualTo(dataTypeToUpdate.DateTime?.Day));
            Assert.That(updatedDataType.DateTime?.Hour, Is.EqualTo(dataTypeToUpdate.DateTime?.Hour));
            Assert.That(updatedDataType.DateTime?.Minute, Is.EqualTo(dataTypeToUpdate.DateTime?.Minute));
            Assert.That(updatedDataType.DateTime?.Second, Is.EqualTo(dataTypeToUpdate.DateTime?.Second));
            Assert.IsNotNull(updatedDataType.DateTimeOffset);
            Assert.That(updatedDataType.DateTimeOffset?.Year, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Year));
            Assert.That(updatedDataType.DateTimeOffset?.Month, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Month));
            Assert.That(updatedDataType.DateTimeOffset?.Day, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Day));
            Assert.That(updatedDataType.DateTimeOffset?.Hour, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Hour));
            Assert.That(updatedDataType.DateTimeOffset?.Minute, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Minute));
            Assert.That(updatedDataType.DateTimeOffset?.Second, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Second));
            Assert.That(updatedDataType.TimeSpan, Is.EqualTo(dataTypeToUpdate.TimeSpan));

            for (int i = 0; i < dataTypeToUpdate.Bytes.Length; i++)
                Assert.That(updatedDataType.Bytes[i], Is.EqualTo(dataTypeToUpdate.Bytes[i]));

            #endregion
        }

        [Test]
        public async Task Insert_Update_Nullable_Many_Types_Async()
        {
            #region Arrange

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            var dataType = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue,
                Sbyte = sbyte.MinValue,
                Short = short.MinValue,
                Ushort = ushort.MaxValue,
                Int = int.MinValue,
                Uint = uint.MaxValue,
                Long = long.MinValue,
                Ulong = (ulong)long.MaxValue,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = true,
                String = "Any string",
                Char = 'C',
                Guid = guid1.ToString(),
                DateTime = new DateTime(2024, 3, 30, 21, 2, 23),
                DateTimeOffset = new DateTimeOffset(new DateTime(2024, 3, 30, 21, 2, 23).ToUniversalTime()),
                TimeSpan = new TimeSpan(1, 2, 3),
                Bytes = new byte[] { 4, 5, 6 },
            };

            var dataTypeToUpdate = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F + 1,
                Double = (double)789.126F - 1,
                Decimal = 789.124M + 1,
                Currency = 789.13F + 1,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid2.ToString(),
                DateTime = dataType.DateTime?.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = dataType.DateTimeOffset?.Add(new TimeSpan(3, 3, 0)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int dataTypeId = (int)await dataContext
                .Insert<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataType.Byte, dataType.Sbyte, dataType.Short, dataType.Ushort, dataType.Int, dataType.Uint, dataType.Long, dataType.Ulong, dataType.Float, dataType.Double, dataType.Decimal, dataType.Currency, dataType.Bool, dataType.String, dataType.Char, guid1.ToString(), dataType.DateTime, dataType.DateTimeOffset, dataType.TimeSpan, dataType.Bytes)
                .ExecuteScalarAsync();

            Model.DataTypeNullable insertedDataType = await dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataTypeNullable>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.DataTypeNullable>();

            await dataContext
                .Update<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataTypeToUpdate.Byte, dataTypeToUpdate.Sbyte, dataTypeToUpdate.Short, dataTypeToUpdate.Ushort, dataTypeToUpdate.Int, dataTypeToUpdate.Uint, dataTypeToUpdate.Long, dataTypeToUpdate.Ulong, dataTypeToUpdate.Float, dataTypeToUpdate.Double, dataTypeToUpdate.Decimal, dataTypeToUpdate.Currency, dataTypeToUpdate.Bool, dataTypeToUpdate.String, dataTypeToUpdate.Char, guid2.ToString(), dataTypeToUpdate.DateTime, dataTypeToUpdate.DateTimeOffset, dataTypeToUpdate.TimeSpan, dataTypeToUpdate.Bytes)
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .ExecuteAsync();

            Model.DataTypeNullable updatedDataType = await dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataTypeNullable>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.IsNotNull(insertedDataType);
            Assert.That(insertedDataType.Id, Is.EqualTo(dataTypeId));
            Assert.That(insertedDataType.Byte, Is.EqualTo(dataType.Byte));
            Assert.That(insertedDataType.Sbyte, Is.EqualTo(dataType.Sbyte));
            Assert.That(insertedDataType.Short, Is.EqualTo(dataType.Short));
            Assert.That(insertedDataType.Ushort, Is.EqualTo(dataType.Ushort));
            Assert.That(insertedDataType.Int, Is.EqualTo(dataType.Int));
            Assert.That(insertedDataType.Uint, Is.EqualTo(dataType.Uint));
            Assert.That(insertedDataType.Long, Is.EqualTo(dataType.Long));
            Assert.That(insertedDataType.Ulong, Is.EqualTo(dataType.Ulong));
            Assert.That(insertedDataType.Float, Is.EqualTo(dataType.Float));
            Assert.That(insertedDataType.Double, Is.EqualTo(dataType.Double));
            Assert.That(insertedDataType.Decimal, Is.EqualTo(dataType.Decimal));
            Assert.That(insertedDataType.Currency, Is.EqualTo(dataType.Currency));
            Assert.That(insertedDataType.Bool, Is.EqualTo(dataType.Bool));
            Assert.That(insertedDataType.String, Is.EqualTo(dataType.String));
            Assert.That(insertedDataType.Char, Is.EqualTo(dataType.Char));
            Assert.That(insertedDataType.Guid.ToUpper(), Is.EqualTo(dataType.Guid.ToUpper()));
            Assert.IsNotNull(insertedDataType.DateTime);
            Assert.That(insertedDataType.DateTime?.Year, Is.EqualTo(dataType.DateTime?.Year));
            Assert.That(insertedDataType.DateTime?.Month, Is.EqualTo(dataType.DateTime?.Month));
            Assert.That(insertedDataType.DateTime?.Day, Is.EqualTo(dataType.DateTime?.Day));
            Assert.That(insertedDataType.DateTime?.Hour, Is.EqualTo(dataType.DateTime?.Hour));
            Assert.That(insertedDataType.DateTime?.Minute, Is.EqualTo(dataType.DateTime?.Minute));
            Assert.That(insertedDataType.DateTime?.Second, Is.EqualTo(dataType.DateTime?.Second));
            Assert.IsNotNull(insertedDataType.DateTimeOffset);
            Assert.That(insertedDataType.DateTimeOffset?.Year, Is.EqualTo(dataType.DateTimeOffset?.Year));
            Assert.That(insertedDataType.DateTimeOffset?.Month, Is.EqualTo(dataType.DateTimeOffset?.Month));
            Assert.That(insertedDataType.DateTimeOffset?.Day, Is.EqualTo(dataType.DateTimeOffset?.Day));
            Assert.That(insertedDataType.DateTimeOffset?.Hour, Is.EqualTo(dataType.DateTimeOffset?.Hour));
            Assert.That(insertedDataType.DateTimeOffset?.Minute, Is.EqualTo(dataType.DateTimeOffset?.Minute));
            Assert.That(insertedDataType.DateTimeOffset?.Second, Is.EqualTo(dataType.DateTimeOffset?.Second));
            Assert.That(insertedDataType.TimeSpan, Is.EqualTo(dataType.TimeSpan));

            for (int i = 0; i < dataType.Bytes.Length; i++)
                Assert.That(insertedDataType.Bytes[i], Is.EqualTo(dataType.Bytes[i]));

            Assert.IsNotNull(updatedDataType);
            Assert.That(updatedDataType.Byte, Is.EqualTo(dataTypeToUpdate.Byte));
            Assert.That(updatedDataType.Sbyte, Is.EqualTo(dataTypeToUpdate.Sbyte));
            Assert.That(updatedDataType.Short, Is.EqualTo(dataTypeToUpdate.Short));
            Assert.That(updatedDataType.Ushort, Is.EqualTo(dataTypeToUpdate.Ushort));
            Assert.That(updatedDataType.Int, Is.EqualTo(dataTypeToUpdate.Int));
            Assert.That(updatedDataType.Uint, Is.EqualTo(dataTypeToUpdate.Uint));
            Assert.That(updatedDataType.Long, Is.EqualTo(dataTypeToUpdate.Long));
            Assert.That(updatedDataType.Ulong, Is.EqualTo(dataTypeToUpdate.Ulong));
            Assert.That(updatedDataType.Float, Is.EqualTo(dataTypeToUpdate.Float));
            Assert.That(updatedDataType.Double, Is.EqualTo(dataTypeToUpdate.Double));
            Assert.That(updatedDataType.Decimal, Is.EqualTo(dataTypeToUpdate.Decimal));
            Assert.That(updatedDataType.Currency, Is.EqualTo(dataTypeToUpdate.Currency));
            Assert.That(updatedDataType.Bool, Is.EqualTo(dataTypeToUpdate.Bool));
            Assert.That(updatedDataType.String, Is.EqualTo(dataTypeToUpdate.String));
            Assert.That(updatedDataType.Char, Is.EqualTo(dataTypeToUpdate.Char));
            Assert.That(updatedDataType.Guid.ToUpper(), Is.EqualTo(dataTypeToUpdate.Guid.ToUpper()));
            Assert.IsNotNull(updatedDataType.DateTime);
            Assert.That(updatedDataType.DateTime?.Year, Is.EqualTo(dataTypeToUpdate.DateTime?.Year));
            Assert.That(updatedDataType.DateTime?.Month, Is.EqualTo(dataTypeToUpdate.DateTime?.Month));
            Assert.That(updatedDataType.DateTime?.Day, Is.EqualTo(dataTypeToUpdate.DateTime?.Day));
            Assert.That(updatedDataType.DateTime?.Hour, Is.EqualTo(dataTypeToUpdate.DateTime?.Hour));
            Assert.That(updatedDataType.DateTime?.Minute, Is.EqualTo(dataTypeToUpdate.DateTime?.Minute));
            Assert.That(updatedDataType.DateTime?.Second, Is.EqualTo(dataTypeToUpdate.DateTime?.Second));
            Assert.IsNotNull(updatedDataType.DateTimeOffset);
            Assert.That(updatedDataType.DateTimeOffset?.Year, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Year));
            Assert.That(updatedDataType.DateTimeOffset?.Month, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Month));
            Assert.That(updatedDataType.DateTimeOffset?.Day, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Day));
            Assert.That(updatedDataType.DateTimeOffset?.Hour, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Hour));
            Assert.That(updatedDataType.DateTimeOffset?.Minute, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Minute));
            Assert.That(updatedDataType.DateTimeOffset?.Second, Is.EqualTo(dataTypeToUpdate.DateTimeOffset?.Second));
            Assert.That(updatedDataType.TimeSpan, Is.EqualTo(dataTypeToUpdate.TimeSpan));

            for (int i = 0; i < dataTypeToUpdate.Bytes.Length; i++)
                Assert.That(updatedDataType.Bytes[i], Is.EqualTo(dataTypeToUpdate.Bytes[i]));

            #endregion
        }

        [Test]
        public void Insert_Update_Null_Many_Types()
        {
            #region Arrange

            var dataType = new Model.DataTypeNullable
            {
                Byte = null,
                Sbyte = null,
                Short = null,
                Ushort = null,
                Int = null,
                Uint = null,
                Long = null,
                Ulong = null,
                Float = null,
                Double = null,
                Decimal = null,
                Currency = null,
                Bool = null,
                String = null,
                Char = null,
                Guid = null,
                DateTime = null,
                DateTimeOffset = null,
                TimeSpan = null,
                Bytes = null,
            };

            Guid guid = Guid.NewGuid();

            var dataTypeToUpdate1 = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid.ToString(),
                DateTime = DateTime.Now.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = DateTimeOffset.UtcNow.Add(new TimeSpan(3, 3, 3, 3)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            var dataTypeToUpdate2 = new Model.DataTypeNullable
            {
                Byte = null,
                Sbyte = null,
                Short = null,
                Ushort = null,
                Int = null,
                Uint = null,
                Long = null,
                Ulong = null,
                Float = null,
                Double = null,
                Decimal = null,
                Currency = null,
                Bool = null,
                String = null,
                Char = null,
                Guid = null,
                DateTime = null,
                DateTimeOffset = null,
                TimeSpan = null,
                Bytes = null,
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int dataTypeId = (int)dataContext
                .Insert<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataType.Byte, dataType.Sbyte, dataType.Short, dataType.Ushort, dataType.Int, dataType.Uint, dataType.Long, dataType.Ulong, dataType.Float, dataType.Double, dataType.Decimal, dataType.Currency, dataType.Bool, dataType.String, dataType.Char, dataType.Guid, dataType.DateTime, dataType.DateTimeOffset, dataType.TimeSpan, dataType.Bytes)
                .ExecuteScalar();

            Model.DataTypeNullable insertedDataType = dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataTypeNullable>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.DataTypeNullable>();

            dataContext
                .Update<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataTypeToUpdate1.Byte, dataTypeToUpdate1.Sbyte, dataTypeToUpdate1.Short, dataTypeToUpdate1.Ushort, dataTypeToUpdate1.Int, dataTypeToUpdate1.Uint, dataTypeToUpdate1.Long, dataTypeToUpdate1.Ulong, dataTypeToUpdate1.Float, dataTypeToUpdate1.Double, dataTypeToUpdate1.Decimal, dataTypeToUpdate1.Currency, dataTypeToUpdate1.Bool, dataTypeToUpdate1.String, dataTypeToUpdate1.Char, guid.ToString(), dataTypeToUpdate1.DateTime, dataTypeToUpdate1.DateTimeOffset, dataTypeToUpdate1.TimeSpan, dataTypeToUpdate1.Bytes)
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .Execute();

            dataContext
                .Update<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataTypeToUpdate2.Byte, dataTypeToUpdate2.Sbyte, dataTypeToUpdate2.Short, dataTypeToUpdate2.Ushort, dataTypeToUpdate2.Int, dataTypeToUpdate2.Uint, dataTypeToUpdate2.Long, dataTypeToUpdate2.Ulong, dataTypeToUpdate2.Float, dataTypeToUpdate2.Double, dataTypeToUpdate2.Decimal, dataTypeToUpdate2.Currency, dataTypeToUpdate2.Bool, dataTypeToUpdate2.String, dataTypeToUpdate2.Char, dataTypeToUpdate2.Guid, dataTypeToUpdate2.DateTime, dataTypeToUpdate2.DateTimeOffset, dataTypeToUpdate2.TimeSpan, dataTypeToUpdate2.Bytes)
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .Execute();

            Model.DataTypeNullable updatedDataType = dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataTypeNullable>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.IsNotNull(insertedDataType);
            Assert.That(insertedDataType.Id, Is.EqualTo(dataTypeId));
            Assert.IsNull(insertedDataType.Byte);
            Assert.IsNull(insertedDataType.Sbyte);
            Assert.IsNull(insertedDataType.Short);
            Assert.IsNull(insertedDataType.Ushort);
            Assert.IsNull(insertedDataType.Int);
            Assert.IsNull(insertedDataType.Uint);
            Assert.IsNull(insertedDataType.Long);
            Assert.IsNull(insertedDataType.Ulong);
            Assert.IsNull(insertedDataType.Float);
            Assert.IsNull(insertedDataType.Double);
            Assert.IsNull(insertedDataType.Decimal);
            Assert.IsNull(insertedDataType.Currency);
            Assert.IsNull(insertedDataType.Bool);
            Assert.IsNull(insertedDataType.String);
            Assert.IsNull(insertedDataType.Char);
            Assert.IsNull(insertedDataType.Guid);
            Assert.IsNull(insertedDataType.DateTime);
            Assert.IsNull(insertedDataType.DateTimeOffset);
            Assert.IsNull(insertedDataType.TimeSpan);
            Assert.IsNull(insertedDataType.Bytes);

            Assert.IsNotNull(updatedDataType);
            Assert.IsNull(updatedDataType.Byte);
            Assert.IsNull(updatedDataType.Sbyte);
            Assert.IsNull(updatedDataType.Short);
            Assert.IsNull(updatedDataType.Ushort);
            Assert.IsNull(updatedDataType.Int);
            Assert.IsNull(updatedDataType.Uint);
            Assert.IsNull(updatedDataType.Long);
            Assert.IsNull(updatedDataType.Ulong);
            Assert.IsNull(updatedDataType.Float);
            Assert.IsNull(updatedDataType.Double);
            Assert.IsNull(updatedDataType.Decimal);
            Assert.IsNull(updatedDataType.Currency);
            Assert.IsNull(updatedDataType.Bool);
            Assert.IsNull(updatedDataType.String);
            Assert.IsNull(updatedDataType.Char);
            Assert.IsNull(updatedDataType.Guid);
            Assert.IsNull(updatedDataType.DateTime);
            Assert.IsNull(updatedDataType.DateTimeOffset);
            Assert.IsNull(updatedDataType.TimeSpan);
            Assert.IsNull(updatedDataType.Bytes);

            #endregion
        }

        [Test]
        public async Task Insert_Update_Null_Many_Types_Async()
        {
            #region Arrange

            var dataType = new Model.DataTypeNullable
            {
                Byte = null,
                Sbyte = null,
                Short = null,
                Ushort = null,
                Int = null,
                Uint = null,
                Long = null,
                Ulong = null,
                Float = null,
                Double = null,
                Decimal = null,
                Currency = null,
                Bool = null,
                String = null,
                Char = null,
                Guid = null,
                DateTime = null,
                DateTimeOffset = null,
                TimeSpan = null,
                Bytes = null,
            };

            Guid guid = Guid.NewGuid();

            var dataTypeToUpdate1 = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid.ToString(),
                DateTime = DateTime.Now.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = DateTimeOffset.UtcNow.Add(new TimeSpan(3, 3, 3, 3)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            var dataTypeToUpdate2 = new Model.DataTypeNullable
            {
                Byte = null,
                Sbyte = null,
                Short = null,
                Ushort = null,
                Int = null,
                Uint = null,
                Long = null,
                Ulong = null,
                Float = null,
                Double = null,
                Decimal = null,
                Currency = null,
                Bool = null,
                String = null,
                Char = null,
                Guid = null,
                DateTime = null,
                DateTimeOffset = null,
                TimeSpan = null,
                Bytes = null,
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int dataTypeId = (int)await dataContext
                .Insert<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataType.Byte, dataType.Sbyte, dataType.Short, dataType.Ushort, dataType.Int, dataType.Uint, dataType.Long, dataType.Ulong, dataType.Float, dataType.Double, dataType.Decimal, dataType.Currency, dataType.Bool, dataType.String, dataType.Char, dataType.Guid, dataType.DateTime, dataType.DateTimeOffset, dataType.TimeSpan, dataType.Bytes)
                .ExecuteScalarAsync();

            Model.DataTypeNullable insertedDataType = await dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataTypeNullable>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.DataTypeNullable>();

            await dataContext
                .Update<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataTypeToUpdate1.Byte, dataTypeToUpdate1.Sbyte, dataTypeToUpdate1.Short, dataTypeToUpdate1.Ushort, dataTypeToUpdate1.Int, dataTypeToUpdate1.Uint, dataTypeToUpdate1.Long, dataTypeToUpdate1.Ulong, dataTypeToUpdate1.Float, dataTypeToUpdate1.Double, dataTypeToUpdate1.Decimal, dataTypeToUpdate1.Currency, dataTypeToUpdate1.Bool, dataTypeToUpdate1.String, dataTypeToUpdate1.Char, guid.ToString(), dataTypeToUpdate1.DateTime, dataTypeToUpdate1.DateTimeOffset, dataTypeToUpdate1.TimeSpan, dataTypeToUpdate1.Bytes)
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .ExecuteAsync();

            await dataContext
                .Update<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    dataTypeToUpdate2.Byte, dataTypeToUpdate2.Sbyte, dataTypeToUpdate2.Short, dataTypeToUpdate2.Ushort, dataTypeToUpdate2.Int, dataTypeToUpdate2.Uint, dataTypeToUpdate2.Long, dataTypeToUpdate2.Ulong, dataTypeToUpdate2.Float, dataTypeToUpdate2.Double, dataTypeToUpdate2.Decimal, dataTypeToUpdate2.Currency, dataTypeToUpdate2.Bool, dataTypeToUpdate2.String, dataTypeToUpdate2.Char, dataTypeToUpdate2.Guid, dataTypeToUpdate2.DateTime, dataTypeToUpdate2.DateTimeOffset, dataTypeToUpdate2.TimeSpan, dataTypeToUpdate2.Bytes)
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .ExecuteAsync();

            Model.DataTypeNullable updatedDataType = await dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Where<Model.DataTypeNullable>(x => x.Id, Common.Operators.EqualTo, dataTypeId)
                .OrderBy<Model.DataTypeNullable>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.IsNotNull(insertedDataType);
            Assert.That(insertedDataType.Id, Is.EqualTo(dataTypeId));
            Assert.IsNull(insertedDataType.Byte);
            Assert.IsNull(insertedDataType.Sbyte);
            Assert.IsNull(insertedDataType.Short);
            Assert.IsNull(insertedDataType.Ushort);
            Assert.IsNull(insertedDataType.Int);
            Assert.IsNull(insertedDataType.Uint);
            Assert.IsNull(insertedDataType.Long);
            Assert.IsNull(insertedDataType.Ulong);
            Assert.IsNull(insertedDataType.Float);
            Assert.IsNull(insertedDataType.Double);
            Assert.IsNull(insertedDataType.Decimal);
            Assert.IsNull(insertedDataType.Currency);
            Assert.IsNull(insertedDataType.Bool);
            Assert.IsNull(insertedDataType.String);
            Assert.IsNull(insertedDataType.Char);
            Assert.IsNull(insertedDataType.Guid);
            Assert.IsNull(insertedDataType.DateTime);
            Assert.IsNull(insertedDataType.DateTimeOffset);
            Assert.IsNull(insertedDataType.TimeSpan);
            Assert.IsNull(insertedDataType.Bytes);

            Assert.IsNotNull(updatedDataType);
            Assert.IsNull(updatedDataType.Byte);
            Assert.IsNull(updatedDataType.Sbyte);
            Assert.IsNull(updatedDataType.Short);
            Assert.IsNull(updatedDataType.Ushort);
            Assert.IsNull(updatedDataType.Int);
            Assert.IsNull(updatedDataType.Uint);
            Assert.IsNull(updatedDataType.Long);
            Assert.IsNull(updatedDataType.Ulong);
            Assert.IsNull(updatedDataType.Float);
            Assert.IsNull(updatedDataType.Double);
            Assert.IsNull(updatedDataType.Decimal);
            Assert.IsNull(updatedDataType.Currency);
            Assert.IsNull(updatedDataType.Bool);
            Assert.IsNull(updatedDataType.String);
            Assert.IsNull(updatedDataType.Char);
            Assert.IsNull(updatedDataType.Guid);
            Assert.IsNull(updatedDataType.DateTime);
            Assert.IsNull(updatedDataType.DateTimeOffset);
            Assert.IsNull(updatedDataType.TimeSpan);
            Assert.IsNull(updatedDataType.Bytes);

            #endregion
        }

        [Test]
        public void Insert_Bulk_Many_Types()
        {
            #region Arrange

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            var dataType1 = new Model.DataType
            {
                Byte = byte.MaxValue,
                Sbyte = sbyte.MinValue,
                Short = short.MinValue,
                Ushort = ushort.MaxValue,
                Int = int.MinValue,
                Uint = uint.MaxValue,
                Long = long.MinValue,
                Ulong = (ulong)long.MaxValue,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = true,
                String = "Any string",
                Char = 'C',
                Guid = guid1.ToString(),
                DateTime = new DateTime(2024, 3, 30, 21, 2, 23),
                DateTimeOffset = new DateTimeOffset(new DateTime(2024, 3, 30, 21, 2, 23).ToUniversalTime()),
                TimeSpan = new TimeSpan(1, 2, 3),
                Bytes = new byte[] { 4, 5, 6 },
            };

            var dataType2 = new Model.DataType
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F + 1,
                Double = (double)789.126F - 1,
                Decimal = 789.124M + 1,
                Currency = 789.13F + 1,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid2.ToString(),
                DateTime = dataType1.DateTime.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = dataType1.DateTimeOffset.Add(new TimeSpan(3, 3, 0)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            var dataTypes = new List<Model.DataType> { dataType1, dataType2 };

            var arrayParams = new object[][] {
                new object[] { dataType1.Byte, dataType1.Sbyte, dataType1.Short, dataType1.Ushort, dataType1.Int, dataType1.Uint, dataType1.Long, dataType1.Ulong, dataType1.Float, dataType1.Double, dataType1.Decimal, dataType1.Currency, dataType1.Bool, dataType1.String, dataType1.Char, guid1.ToString(), dataType1.DateTime, dataType1.DateTimeOffset, dataType1.TimeSpan, dataType1.Bytes },
                new object[] { dataType2.Byte, dataType2.Sbyte, dataType2.Short, dataType2.Ushort, dataType2.Int, dataType2.Uint, dataType2.Long, dataType2.Ulong, dataType2.Float, dataType2.Double, dataType2.Decimal, dataType2.Currency, dataType2.Bool, dataType2.String, dataType2.Char, guid2.ToString(), dataType2.DateTime, dataType2.DateTimeOffset, dataType2.TimeSpan, dataType2.Bytes },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext
                .Insert<Model.DataType>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    arrayParams)
                .ExecuteScalar();

            IEnumerable<Model.DataType> insertedDataTypes = dataContext
                .Select<Model.DataType>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Query<Model.DataType>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(insertedDataTypes.Count(), Is.EqualTo(dataTypes.Count()));

            foreach (var dataType in dataTypes)
            {
                Model.DataType insertedDataType = insertedDataTypes.FirstOrDefault(x => x.Byte == dataType.Byte);

                Assert.IsNotNull(insertedDataType);
                Assert.That(insertedDataType.Byte, Is.EqualTo(dataType.Byte));
                Assert.That(insertedDataType.Sbyte, Is.EqualTo(dataType.Sbyte));
                Assert.That(insertedDataType.Short, Is.EqualTo(dataType.Short));
                Assert.That(insertedDataType.Ushort, Is.EqualTo(dataType.Ushort));
                Assert.That(insertedDataType.Int, Is.EqualTo(dataType.Int));
                Assert.That(insertedDataType.Uint, Is.EqualTo(dataType.Uint));
                Assert.That(insertedDataType.Long, Is.EqualTo(dataType.Long));
                Assert.That(insertedDataType.Ulong, Is.EqualTo(dataType.Ulong));
                Assert.That(insertedDataType.Float, Is.EqualTo(dataType.Float));
                Assert.That(insertedDataType.Double, Is.EqualTo(dataType.Double));
                Assert.That(insertedDataType.Decimal, Is.EqualTo(dataType.Decimal));
                Assert.That(insertedDataType.Currency, Is.EqualTo(dataType.Currency));
                Assert.That(insertedDataType.Bool, Is.EqualTo(dataType.Bool));
                Assert.That(insertedDataType.String, Is.EqualTo(dataType.String));
                Assert.That(insertedDataType.Char, Is.EqualTo(dataType.Char));
                Assert.That(insertedDataType.Guid.ToUpper(), Is.EqualTo(dataType.Guid.ToUpper()));
                Assert.That(insertedDataType.DateTime.Year, Is.EqualTo(dataType.DateTime.Year));
                Assert.That(insertedDataType.DateTime.Month, Is.EqualTo(dataType.DateTime.Month));
                Assert.That(insertedDataType.DateTime.Day, Is.EqualTo(dataType.DateTime.Day));
                Assert.That(insertedDataType.DateTime.Hour, Is.EqualTo(dataType.DateTime.Hour));
                Assert.That(insertedDataType.DateTime.Minute, Is.EqualTo(dataType.DateTime.Minute));
                Assert.That(insertedDataType.DateTime.Second, Is.EqualTo(dataType.DateTime.Second));
                Assert.That(insertedDataType.DateTimeOffset.Year, Is.EqualTo(dataType.DateTimeOffset.Year));
                Assert.That(insertedDataType.DateTimeOffset.Month, Is.EqualTo(dataType.DateTimeOffset.Month));
                Assert.That(insertedDataType.DateTimeOffset.Day, Is.EqualTo(dataType.DateTimeOffset.Day));
                Assert.That(insertedDataType.DateTimeOffset.Hour, Is.EqualTo(dataType.DateTimeOffset.Hour));
                Assert.That(insertedDataType.DateTimeOffset.Minute, Is.EqualTo(dataType.DateTimeOffset.Minute));
                Assert.That(insertedDataType.DateTimeOffset.Second, Is.EqualTo(dataType.DateTimeOffset.Second));
                Assert.That(insertedDataType.TimeSpan, Is.EqualTo(dataType.TimeSpan));

                for (int i = 0; i < dataType.Bytes.Length; i++)
                    Assert.That(insertedDataType.Bytes[i], Is.EqualTo(dataType.Bytes[i]));
            }

            #endregion
        }

        [Test]
        public async Task Insert_Bulk_Many_Types_Async()
        {
            #region Arrange

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            var dataType1 = new Model.DataType
            {
                Byte = byte.MaxValue,
                Sbyte = sbyte.MinValue,
                Short = short.MinValue,
                Ushort = ushort.MaxValue,
                Int = int.MinValue,
                Uint = uint.MaxValue,
                Long = long.MinValue,
                Ulong = (ulong)long.MaxValue,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = true,
                String = "Any string",
                Char = 'C',
                Guid = guid1.ToString(),
                DateTime = new DateTime(2024, 3, 30, 21, 2, 23),
                DateTimeOffset = new DateTimeOffset(new DateTime(2024, 3, 30, 21, 2, 23).ToUniversalTime()),
                TimeSpan = new TimeSpan(1, 2, 3),
                Bytes = new byte[] { 4, 5, 6 },
            };

            var dataType2 = new Model.DataType
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F + 1,
                Double = (double)789.126F - 1,
                Decimal = 789.124M + 1,
                Currency = 789.13F + 1,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid2.ToString(),
                DateTime = dataType1.DateTime.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = dataType1.DateTimeOffset.Add(new TimeSpan(3, 3, 0)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            var dataTypes = new List<Model.DataType> { dataType1, dataType2 };

            var arrayParams = new object[][] {
                new object[] { dataType1.Byte, dataType1.Sbyte, dataType1.Short, dataType1.Ushort, dataType1.Int, dataType1.Uint, dataType1.Long, dataType1.Ulong, dataType1.Float, dataType1.Double, dataType1.Decimal, dataType1.Currency, dataType1.Bool, dataType1.String, dataType1.Char, guid1.ToString(), dataType1.DateTime, dataType1.DateTimeOffset, dataType1.TimeSpan, dataType1.Bytes },
                new object[] { dataType2.Byte, dataType2.Sbyte, dataType2.Short, dataType2.Ushort, dataType2.Int, dataType2.Uint, dataType2.Long, dataType2.Ulong, dataType2.Float, dataType2.Double, dataType2.Decimal, dataType2.Currency, dataType2.Bool, dataType2.String, dataType2.Char, guid2.ToString(), dataType2.DateTime, dataType2.DateTimeOffset, dataType2.TimeSpan, dataType2.Bytes },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            await dataContext
                .Insert<Model.DataType>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    arrayParams)
                .ExecuteScalarAsync();

            IEnumerable<Model.DataType> insertedDataTypes = await dataContext
                .Select<Model.DataType>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .QueryAsync<Model.DataType>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(insertedDataTypes.Count(), Is.EqualTo(dataTypes.Count()));

            foreach (var dataType in dataTypes)
            {
                Model.DataType insertedDataType = insertedDataTypes.FirstOrDefault(x => x.Byte == dataType.Byte);

                Assert.IsNotNull(insertedDataType);
                Assert.That(insertedDataType.Byte, Is.EqualTo(dataType.Byte));
                Assert.That(insertedDataType.Sbyte, Is.EqualTo(dataType.Sbyte));
                Assert.That(insertedDataType.Short, Is.EqualTo(dataType.Short));
                Assert.That(insertedDataType.Ushort, Is.EqualTo(dataType.Ushort));
                Assert.That(insertedDataType.Int, Is.EqualTo(dataType.Int));
                Assert.That(insertedDataType.Uint, Is.EqualTo(dataType.Uint));
                Assert.That(insertedDataType.Long, Is.EqualTo(dataType.Long));
                Assert.That(insertedDataType.Ulong, Is.EqualTo(dataType.Ulong));
                Assert.That(insertedDataType.Float, Is.EqualTo(dataType.Float));
                Assert.That(insertedDataType.Double, Is.EqualTo(dataType.Double));
                Assert.That(insertedDataType.Decimal, Is.EqualTo(dataType.Decimal));
                Assert.That(insertedDataType.Currency, Is.EqualTo(dataType.Currency));
                Assert.That(insertedDataType.Bool, Is.EqualTo(dataType.Bool));
                Assert.That(insertedDataType.String, Is.EqualTo(dataType.String));
                Assert.That(insertedDataType.Char, Is.EqualTo(dataType.Char));
                Assert.That(insertedDataType.Guid.ToUpper(), Is.EqualTo(dataType.Guid.ToUpper()));
                Assert.That(insertedDataType.DateTime.Year, Is.EqualTo(dataType.DateTime.Year));
                Assert.That(insertedDataType.DateTime.Month, Is.EqualTo(dataType.DateTime.Month));
                Assert.That(insertedDataType.DateTime.Day, Is.EqualTo(dataType.DateTime.Day));
                Assert.That(insertedDataType.DateTime.Hour, Is.EqualTo(dataType.DateTime.Hour));
                Assert.That(insertedDataType.DateTime.Minute, Is.EqualTo(dataType.DateTime.Minute));
                Assert.That(insertedDataType.DateTime.Second, Is.EqualTo(dataType.DateTime.Second));
                Assert.That(insertedDataType.DateTimeOffset.Year, Is.EqualTo(dataType.DateTimeOffset.Year));
                Assert.That(insertedDataType.DateTimeOffset.Month, Is.EqualTo(dataType.DateTimeOffset.Month));
                Assert.That(insertedDataType.DateTimeOffset.Day, Is.EqualTo(dataType.DateTimeOffset.Day));
                Assert.That(insertedDataType.DateTimeOffset.Hour, Is.EqualTo(dataType.DateTimeOffset.Hour));
                Assert.That(insertedDataType.DateTimeOffset.Minute, Is.EqualTo(dataType.DateTimeOffset.Minute));
                Assert.That(insertedDataType.DateTimeOffset.Second, Is.EqualTo(dataType.DateTimeOffset.Second));
                Assert.That(insertedDataType.TimeSpan, Is.EqualTo(dataType.TimeSpan));

                for (int i = 0; i < dataType.Bytes.Length; i++)
                    Assert.That(insertedDataType.Bytes[i], Is.EqualTo(dataType.Bytes[i]));
            }

            #endregion
        }

        [Test]
        public void Insert_Bulk_Nullable_Many_Types()
        {
            #region Arrange

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            var dataType1 = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue,
                Sbyte = sbyte.MinValue,
                Short = short.MinValue,
                Ushort = ushort.MaxValue,
                Int = int.MinValue,
                Uint = uint.MaxValue,
                Long = long.MinValue,
                Ulong = (ulong)long.MaxValue,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = true,
                String = "Any string",
                Char = 'C',
                Guid = guid1.ToString(),
                DateTime = new DateTime(2024, 3, 30, 21, 2, 23),
                DateTimeOffset = new DateTimeOffset(new DateTime(2024, 3, 30, 21, 2, 23).ToUniversalTime()),
                TimeSpan = new TimeSpan(1, 2, 3),
                Bytes = new byte[] { 4, 5, 6 },
            };

            var dataType2 = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F + 1,
                Double = (double)789.126F - 1,
                Decimal = 789.124M + 1,
                Currency = 789.13F + 1,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid2.ToString(),
                DateTime = dataType1.DateTime?.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = dataType1.DateTimeOffset?.Add(new TimeSpan(3, 3, 0)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            var dataTypes = new List<Model.DataTypeNullable> { dataType1, dataType2 };

            var arrayParams = new object[][] {
                new object[] { dataType1.Byte, dataType1.Sbyte, dataType1.Short, dataType1.Ushort, dataType1.Int, dataType1.Uint, dataType1.Long, dataType1.Ulong, dataType1.Float, dataType1.Double, dataType1.Decimal, dataType1.Currency, dataType1.Bool, dataType1.String, dataType1.Char, guid1.ToString(), dataType1.DateTime, dataType1.DateTimeOffset, dataType1.TimeSpan, dataType1.Bytes },
                new object[] { dataType2.Byte, dataType2.Sbyte, dataType2.Short, dataType2.Ushort, dataType2.Int, dataType2.Uint, dataType2.Long, dataType2.Ulong, dataType2.Float, dataType2.Double, dataType2.Decimal, dataType2.Currency, dataType2.Bool, dataType2.String, dataType2.Char, guid2.ToString(), dataType2.DateTime, dataType2.DateTimeOffset, dataType2.TimeSpan, dataType2.Bytes },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext
                .Insert<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    arrayParams)
                .ExecuteScalar();

            IEnumerable<Model.DataTypeNullable> insertedDataTypes = dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Query<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(insertedDataTypes.Count(), Is.EqualTo(dataTypes.Count()));

            foreach (var dataType in dataTypes)
            {
                Model.DataTypeNullable insertedDataType = insertedDataTypes.FirstOrDefault(x => x.Byte == dataType.Byte);

                Assert.IsNotNull(insertedDataType);
                Assert.That(insertedDataType.Byte, Is.EqualTo(dataType.Byte));
                Assert.That(insertedDataType.Sbyte, Is.EqualTo(dataType.Sbyte));
                Assert.That(insertedDataType.Short, Is.EqualTo(dataType.Short));
                Assert.That(insertedDataType.Ushort, Is.EqualTo(dataType.Ushort));
                Assert.That(insertedDataType.Int, Is.EqualTo(dataType.Int));
                Assert.That(insertedDataType.Uint, Is.EqualTo(dataType.Uint));
                Assert.That(insertedDataType.Long, Is.EqualTo(dataType.Long));
                Assert.That(insertedDataType.Ulong, Is.EqualTo(dataType.Ulong));
                Assert.That(insertedDataType.Float, Is.EqualTo(dataType.Float));
                Assert.That(insertedDataType.Double, Is.EqualTo(dataType.Double));
                Assert.That(insertedDataType.Decimal, Is.EqualTo(dataType.Decimal));
                Assert.That(insertedDataType.Currency, Is.EqualTo(dataType.Currency));
                Assert.That(insertedDataType.Bool, Is.EqualTo(dataType.Bool));
                Assert.That(insertedDataType.String, Is.EqualTo(dataType.String));
                Assert.That(insertedDataType.Char, Is.EqualTo(dataType.Char));
                Assert.That(insertedDataType.Guid.ToUpper(), Is.EqualTo(dataType.Guid.ToUpper()));
                Assert.IsNotNull(insertedDataType.DateTime);
                Assert.That(insertedDataType.DateTime?.Year, Is.EqualTo(dataType.DateTime?.Year));
                Assert.That(insertedDataType.DateTime?.Month, Is.EqualTo(dataType.DateTime?.Month));
                Assert.That(insertedDataType.DateTime?.Day, Is.EqualTo(dataType.DateTime?.Day));
                Assert.That(insertedDataType.DateTime?.Hour, Is.EqualTo(dataType.DateTime?.Hour));
                Assert.That(insertedDataType.DateTime?.Minute, Is.EqualTo(dataType.DateTime?.Minute));
                Assert.That(insertedDataType.DateTime?.Second, Is.EqualTo(dataType.DateTime?.Second));
                Assert.IsNotNull(insertedDataType.DateTimeOffset);
                Assert.That(insertedDataType.DateTimeOffset?.Year, Is.EqualTo(dataType.DateTimeOffset?.Year));
                Assert.That(insertedDataType.DateTimeOffset?.Month, Is.EqualTo(dataType.DateTimeOffset?.Month));
                Assert.That(insertedDataType.DateTimeOffset?.Day, Is.EqualTo(dataType.DateTimeOffset?.Day));
                Assert.That(insertedDataType.DateTimeOffset?.Hour, Is.EqualTo(dataType.DateTimeOffset?.Hour));
                Assert.That(insertedDataType.DateTimeOffset?.Minute, Is.EqualTo(dataType.DateTimeOffset?.Minute));
                Assert.That(insertedDataType.DateTimeOffset?.Second, Is.EqualTo(dataType.DateTimeOffset?.Second));
                Assert.That(insertedDataType.TimeSpan, Is.EqualTo(dataType.TimeSpan));

                for (int i = 0; i < dataType.Bytes.Length; i++)
                    Assert.That(insertedDataType.Bytes[i], Is.EqualTo(dataType.Bytes[i]));
            }

            #endregion
        }

        [Test]
        public async Task Insert_Bulk_Nullable_Many_Types_Async()
        {
            #region Arrange

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            var dataType1 = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue,
                Sbyte = sbyte.MinValue,
                Short = short.MinValue,
                Ushort = ushort.MaxValue,
                Int = int.MinValue,
                Uint = uint.MaxValue,
                Long = long.MinValue,
                Ulong = (ulong)long.MaxValue,
                Float = 789.125F,
                Double = (double)789.126F,
                Decimal = 789.124M,
                Currency = 789.13F,
                Bool = true,
                String = "Any string",
                Char = 'C',
                Guid = guid1.ToString(),
                DateTime = new DateTime(2024, 3, 30, 21, 2, 23),
                DateTimeOffset = new DateTimeOffset(new DateTime(2024, 3, 30, 21, 2, 23).ToUniversalTime()),
                TimeSpan = new TimeSpan(1, 2, 3),
                Bytes = new byte[] { 4, 5, 6 },
            };

            var dataType2 = new Model.DataTypeNullable
            {
                Byte = byte.MaxValue - 1,
                Sbyte = sbyte.MinValue + 1,
                Short = short.MinValue + 1,
                Ushort = ushort.MaxValue - 1,
                Int = int.MinValue + 1,
                Uint = uint.MaxValue - 1,
                Long = long.MinValue + 1,
                Ulong = (ulong)long.MaxValue - 1,
                Float = 789.125F + 1,
                Double = (double)789.126F - 1,
                Decimal = 789.124M + 1,
                Currency = 789.13F + 1,
                Bool = false,
                String = "Another string",
                Char = 'H',
                Guid = guid2.ToString(),
                DateTime = dataType1.DateTime?.Add(new TimeSpan(2, 2, 2, 2)),
                DateTimeOffset = dataType1.DateTimeOffset?.Add(new TimeSpan(3, 3, 0)),
                TimeSpan = new TimeSpan(4, 5, 6),
                Bytes = new byte[] { 7, 8, 9 },
            };

            var dataTypes = new List<Model.DataTypeNullable> { dataType1, dataType2 };

            var arrayParams = new object[][] {
                new object[] { dataType1.Byte, dataType1.Sbyte, dataType1.Short, dataType1.Ushort, dataType1.Int, dataType1.Uint, dataType1.Long, dataType1.Ulong, dataType1.Float, dataType1.Double, dataType1.Decimal, dataType1.Currency, dataType1.Bool, dataType1.String, dataType1.Char, guid1.ToString(), dataType1.DateTime, dataType1.DateTimeOffset, dataType1.TimeSpan, dataType1.Bytes },
                new object[] { dataType2.Byte, dataType2.Sbyte, dataType2.Short, dataType2.Ushort, dataType2.Int, dataType2.Uint, dataType2.Long, dataType2.Ulong, dataType2.Float, dataType2.Double, dataType2.Decimal, dataType2.Currency, dataType2.Bool, dataType2.String, dataType2.Char, guid2.ToString(), dataType2.DateTime, dataType2.DateTimeOffset, dataType2.TimeSpan, dataType2.Bytes },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            await dataContext
                .Insert<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    arrayParams)
                .ExecuteScalarAsync();

            IEnumerable<Model.DataTypeNullable> insertedDataTypes = await dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .QueryAsync<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(insertedDataTypes.Count(), Is.EqualTo(dataTypes.Count()));

            foreach (var dataType in dataTypes)
            {
                Model.DataTypeNullable insertedDataType = insertedDataTypes.FirstOrDefault(x => x.Byte == dataType.Byte);

                Assert.IsNotNull(insertedDataType);
                Assert.That(insertedDataType.Byte, Is.EqualTo(dataType.Byte));
                Assert.That(insertedDataType.Sbyte, Is.EqualTo(dataType.Sbyte));
                Assert.That(insertedDataType.Short, Is.EqualTo(dataType.Short));
                Assert.That(insertedDataType.Ushort, Is.EqualTo(dataType.Ushort));
                Assert.That(insertedDataType.Int, Is.EqualTo(dataType.Int));
                Assert.That(insertedDataType.Uint, Is.EqualTo(dataType.Uint));
                Assert.That(insertedDataType.Long, Is.EqualTo(dataType.Long));
                Assert.That(insertedDataType.Ulong, Is.EqualTo(dataType.Ulong));
                Assert.That(insertedDataType.Float, Is.EqualTo(dataType.Float));
                Assert.That(insertedDataType.Double, Is.EqualTo(dataType.Double));
                Assert.That(insertedDataType.Decimal, Is.EqualTo(dataType.Decimal));
                Assert.That(insertedDataType.Currency, Is.EqualTo(dataType.Currency));
                Assert.That(insertedDataType.Bool, Is.EqualTo(dataType.Bool));
                Assert.That(insertedDataType.String, Is.EqualTo(dataType.String));
                Assert.That(insertedDataType.Char, Is.EqualTo(dataType.Char));
                Assert.That(insertedDataType.Guid.ToUpper(), Is.EqualTo(dataType.Guid.ToUpper()));
                Assert.IsNotNull(insertedDataType.DateTime);
                Assert.That(insertedDataType.DateTime?.Year, Is.EqualTo(dataType.DateTime?.Year));
                Assert.That(insertedDataType.DateTime?.Month, Is.EqualTo(dataType.DateTime?.Month));
                Assert.That(insertedDataType.DateTime?.Day, Is.EqualTo(dataType.DateTime?.Day));
                Assert.That(insertedDataType.DateTime?.Hour, Is.EqualTo(dataType.DateTime?.Hour));
                Assert.That(insertedDataType.DateTime?.Minute, Is.EqualTo(dataType.DateTime?.Minute));
                Assert.That(insertedDataType.DateTime?.Second, Is.EqualTo(dataType.DateTime?.Second));
                Assert.IsNotNull(insertedDataType.DateTimeOffset);
                Assert.That(insertedDataType.DateTimeOffset?.Year, Is.EqualTo(dataType.DateTimeOffset?.Year));
                Assert.That(insertedDataType.DateTimeOffset?.Month, Is.EqualTo(dataType.DateTimeOffset?.Month));
                Assert.That(insertedDataType.DateTimeOffset?.Day, Is.EqualTo(dataType.DateTimeOffset?.Day));
                Assert.That(insertedDataType.DateTimeOffset?.Hour, Is.EqualTo(dataType.DateTimeOffset?.Hour));
                Assert.That(insertedDataType.DateTimeOffset?.Minute, Is.EqualTo(dataType.DateTimeOffset?.Minute));
                Assert.That(insertedDataType.DateTimeOffset?.Second, Is.EqualTo(dataType.DateTimeOffset?.Second));
                Assert.That(insertedDataType.TimeSpan, Is.EqualTo(dataType.TimeSpan));

                for (int i = 0; i < dataType.Bytes.Length; i++)
                    Assert.That(insertedDataType.Bytes[i], Is.EqualTo(dataType.Bytes[i]));
            }

            #endregion
        }

        [Test]
        public void Insert_Bulk_Null_Many_Types()
        {
            #region Arrange

            var dataType1 = new Model.DataTypeNullable
            {
                Byte = null,
                Sbyte = null,
                Short = null,
                Ushort = null,
                Int = null,
                Uint = null,
                Long = null,
                Ulong = null,
                Float = null,
                Double = null,
                Decimal = null,
                Currency = null,
                Bool = null,
                String = null,
                Char = null,
                Guid = null,
                DateTime = null,
                DateTimeOffset = null,
                TimeSpan = null,
                Bytes = null,
            };

            var dataType2 = new Model.DataTypeNullable
            {
                Byte = null,
                Sbyte = null,
                Short = null,
                Ushort = null,
                Int = null,
                Uint = null,
                Long = null,
                Ulong = null,
                Float = null,
                Double = null,
                Decimal = null,
                Currency = null,
                Bool = null,
                String = null,
                Char = null,
                Guid = null,
                DateTime = null,
                DateTimeOffset = null,
                TimeSpan = null,
                Bytes = null,
            };

            var dataTypes = new List<Model.DataTypeNullable> { dataType1, dataType2 };

            var arrayParams = new object[][] {
                new object[] { dataType1.Byte, dataType1.Sbyte, dataType1.Short, dataType1.Ushort, dataType1.Int, dataType1.Uint, dataType1.Long, dataType1.Ulong, dataType1.Float, dataType1.Double, dataType1.Decimal, dataType1.Currency, dataType1.Bool, dataType1.String, dataType1.Char, dataType1.Guid, dataType1.DateTime, dataType1.DateTimeOffset, dataType1.TimeSpan, dataType1.Bytes },
                new object[] { dataType2.Byte, dataType2.Sbyte, dataType2.Short, dataType2.Ushort, dataType2.Int, dataType2.Uint, dataType2.Long, dataType2.Ulong, dataType2.Float, dataType2.Double, dataType2.Decimal, dataType2.Currency, dataType2.Bool, dataType2.String, dataType2.Char, dataType2.Guid, dataType2.DateTime, dataType2.DateTimeOffset, dataType2.TimeSpan, dataType2.Bytes },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext
                .Insert<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    arrayParams)
                .ExecuteScalar();

            IEnumerable<Model.DataTypeNullable> insertedDataTypes = dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .Query<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(insertedDataTypes.Count(), Is.EqualTo(dataTypes.Count()));

            foreach (var insertedDataType in insertedDataTypes)
            {
                Assert.IsNotNull(insertedDataType);
                Assert.That(insertedDataType.Id, Is.GreaterThan(0));
                Assert.IsNull(insertedDataType.Byte);
                Assert.IsNull(insertedDataType.Sbyte);
                Assert.IsNull(insertedDataType.Short);
                Assert.IsNull(insertedDataType.Ushort);
                Assert.IsNull(insertedDataType.Int);
                Assert.IsNull(insertedDataType.Uint);
                Assert.IsNull(insertedDataType.Long);
                Assert.IsNull(insertedDataType.Ulong);
                Assert.IsNull(insertedDataType.Float);
                Assert.IsNull(insertedDataType.Double);
                Assert.IsNull(insertedDataType.Decimal);
                Assert.IsNull(insertedDataType.Currency);
                Assert.IsNull(insertedDataType.Bool);
                Assert.IsNull(insertedDataType.String);
                Assert.IsNull(insertedDataType.Char);
                Assert.IsNull(insertedDataType.Guid);
                Assert.IsNull(insertedDataType.DateTime);
                Assert.IsNull(insertedDataType.DateTimeOffset);
                Assert.IsNull(insertedDataType.TimeSpan);
                Assert.IsNull(insertedDataType.Bytes);
            }

            #endregion
        }

        [Test]
        public async Task Insert_Bulk_Null_Many_Types_Async()
        {
            #region Arrange

            var dataType1 = new Model.DataTypeNullable
            {
                Byte = null,
                Sbyte = null,
                Short = null,
                Ushort = null,
                Int = null,
                Uint = null,
                Long = null,
                Ulong = null,
                Float = null,
                Double = null,
                Decimal = null,
                Currency = null,
                Bool = null,
                String = null,
                Char = null,
                Guid = null,
                DateTime = null,
                DateTimeOffset = null,
                TimeSpan = null,
                Bytes = null,
            };

            var dataType2 = new Model.DataTypeNullable
            {
                Byte = null,
                Sbyte = null,
                Short = null,
                Ushort = null,
                Int = null,
                Uint = null,
                Long = null,
                Ulong = null,
                Float = null,
                Double = null,
                Decimal = null,
                Currency = null,
                Bool = null,
                String = null,
                Char = null,
                Guid = null,
                DateTime = null,
                DateTimeOffset = null,
                TimeSpan = null,
                Bytes = null,
            };

            var dataTypes = new List<Model.DataTypeNullable> { dataType1, dataType2 };

            var arrayParams = new object[][] {
                new object[] { dataType1.Byte, dataType1.Sbyte, dataType1.Short, dataType1.Ushort, dataType1.Int, dataType1.Uint, dataType1.Long, dataType1.Ulong, dataType1.Float, dataType1.Double, dataType1.Decimal, dataType1.Currency, dataType1.Bool, dataType1.String, dataType1.Char, dataType1.Guid, dataType1.DateTime, dataType1.DateTimeOffset, dataType1.TimeSpan, dataType1.Bytes },
                new object[] { dataType2.Byte, dataType2.Sbyte, dataType2.Short, dataType2.Ushort, dataType2.Int, dataType2.Uint, dataType2.Long, dataType2.Ulong, dataType2.Float, dataType2.Double, dataType2.Decimal, dataType2.Currency, dataType2.Bool, dataType2.String, dataType2.Char, dataType2.Guid, dataType2.DateTime, dataType2.DateTimeOffset, dataType2.TimeSpan, dataType2.Bytes },
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            await dataContext
                .Insert<Model.DataTypeNullable>(x => new { x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes },
                    arrayParams)
                .ExecuteScalarAsync();

            IEnumerable<Model.DataTypeNullable> insertedDataTypes = await dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Byte, x.Sbyte, x.Short, x.Ushort, x.Int, x.Uint, x.Long, x.Ulong, x.Float, x.Double, x.Decimal, x.Currency, x.Bool, x.String, x.Char, x.Guid, x.DateTime, x.DateTimeOffset, x.TimeSpan, x.Bytes })
                .QueryAsync<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(insertedDataTypes.Count(), Is.EqualTo(dataTypes.Count()));

            foreach (var insertedDataType in insertedDataTypes)
            {
                Assert.IsNotNull(insertedDataType);
                Assert.That(insertedDataType.Id, Is.GreaterThan(0));
                Assert.IsNull(insertedDataType.Byte);
                Assert.IsNull(insertedDataType.Sbyte);
                Assert.IsNull(insertedDataType.Short);
                Assert.IsNull(insertedDataType.Ushort);
                Assert.IsNull(insertedDataType.Int);
                Assert.IsNull(insertedDataType.Uint);
                Assert.IsNull(insertedDataType.Long);
                Assert.IsNull(insertedDataType.Ulong);
                Assert.IsNull(insertedDataType.Float);
                Assert.IsNull(insertedDataType.Double);
                Assert.IsNull(insertedDataType.Decimal);
                Assert.IsNull(insertedDataType.Currency);
                Assert.IsNull(insertedDataType.Bool);
                Assert.IsNull(insertedDataType.String);
                Assert.IsNull(insertedDataType.Char);
                Assert.IsNull(insertedDataType.Guid);
                Assert.IsNull(insertedDataType.DateTime);
                Assert.IsNull(insertedDataType.DateTimeOffset);
                Assert.IsNull(insertedDataType.TimeSpan);
                Assert.IsNull(insertedDataType.Bytes);
            }

            #endregion
        }

        [Test]
        public void Delete()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int rowNumber = dataContext
                .Delete<Model.User>()
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Delete_Async()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int rowNumber = await dataContext
                .Delete<Model.User>()
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Select()
        {
            #region Arrange

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .Query<Model.User>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public async Task Select_Async()
        {
            #region Arrange

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = await dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .QueryAsync<Model.User>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public void Select_Where()
        {
            #region Arrange

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user2.Id)
                .Query<Model.User>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = await dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user2.Id)
                .QueryAsync<Model.User>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .Query<Model.User>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = await dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryAsync<Model.User>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1, 1)
                .Query<Model.User>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = await dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1, 1)
                .QueryAsync<Model.User>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            Model.User user = dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefault<Model.User>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            Model.User user = await dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .OrderBy<Model.User>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(1)
                .QueryFirstOrDefaultAsync<Model.User>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.IsNotNull(user);
            Assert.That(user.Id, Is.EqualTo(user1.Id));
            Assert.That(user.Nick, Is.EqualTo(user1.Nick));
            Assert.That(user.Password, Is.EqualTo(user1.Password));

            #endregion
        }

        [Test]
        public void Select_With_Cast()
        {
            #region Arrange

            var dataTypeToInsert = new Model.DataTypeNullable
            {
                Float = 123.4F,
                Double = 234.9D,
            };

            base.InsertDataType(dataTypeToInsert);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            Model.DataTypeNullable dataType = dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Float, x.Double })
                .Cast<Model.DataTypeNullable>(x => x.Float, "NUMBER(10)")
                .Cast<Model.DataTypeNullable>(x => x.Double, "NUMBER(10)")
                .QueryFirstOrDefault<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            float expectedFloat = (float)(int)dataTypeToInsert.Float;
            double expectedDouble = (double)Math.Round((double)dataTypeToInsert.Double);

            Assert.IsNotNull(dataType);
            Assert.That(dataType.Float, Is.EqualTo(expectedFloat));
            Assert.That(dataType.Double, Is.EqualTo(expectedDouble));

            #endregion
        }

        [Test]
        public async Task Select_With_Cast_Async()
        {
            #region Arrange

            var dataTypeToInsert = new Model.DataTypeNullable
            {
                Float = 123.4F,
                Double = 234.9D,
            };

            base.InsertDataType(dataTypeToInsert);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            Model.DataTypeNullable dataType = await dataContext
                .Select<Model.DataTypeNullable>(x => new { x.Id, x.Float, x.Double })
                .Cast<Model.DataTypeNullable>(x => x.Float, "NUMBER(10)")
                .Cast<Model.DataTypeNullable>(x => x.Double, "NUMBER(10)")
                .QueryFirstOrDefaultAsync<Model.DataTypeNullable>();

            dataContext.Dispose();

            #endregion

            #region Assert

            float expectedFloat = (float)(int)dataTypeToInsert.Float;
            double expectedDouble = (double)Math.Round((double)dataTypeToInsert.Double);

            Assert.IsNotNull(dataType);
            Assert.That(dataType.Float, Is.EqualTo(expectedFloat));
            Assert.That(dataType.Double, Is.EqualTo(expectedDouble));

            #endregion
        }

        [Test]
        public void Select_With_Query()
        {
            #region Arrange

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            var queryAndValues = dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .Where<Model.User>(x => x.Id, Common.Operators.GreaterThan, 0)
                .And<Model.User>(x => x.Nick, Common.Operators.Like, "%N%")
                .GetQuery();

            IEnumerable<Model.User> users = dataContext
                .SetQuery(queryAndValues.Query, queryAndValues.Values)
                .Query<Model.User>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public async Task Select_With_Query_Async()
        {
            #region Arrange

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            var queryAndValues = dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .Where<Model.User>(x => x.Id, Common.Operators.GreaterThan, 0)
                .And<Model.User>(x => x.Nick, Common.Operators.Like, "%N%")
                .GetQuery();

            IEnumerable<Model.User> users = await dataContext
                .SetQuery(queryAndValues.Query, queryAndValues.Values)
                .QueryAsync<Model.User>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));

            #endregion
        }

        [Test]
        public void Select_Not_Buffered()
        {
            #region Arrange

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);
            dataContext.Buffered = false;

            IEnumerable<Model.User> users = dataContext
                .Select<Model.User>(x => new { x.Id, x.Nick, x.Password })
                .Query<Model.User>();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.IsTrue(users.GetType().FullName.Contains("Dapper"));

            dataContext.Dispose();

            #endregion
        }

        #endregion

        #region Select Aggregate

        [Test]
        public void Select_Count()
        {
            #region Arrange

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Count<Model.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Count<Model.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Max<Model.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Max<Model.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Min<Model.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Min<Model.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Sum<Model.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Sum<Model.UserValue>(x => x.Value, alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Avg<Model.UserValue>(x => x.Value, cast: "decimal(10, 5)", alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .Query<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1 = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1);

            var user2 = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2);

            var user1Value1 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 1.7,
            };

            base.InsertUserValue(user1Value1);

            var user1Value2 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.3,
            };

            base.InsertUserValue(user1Value2);

            var user1Value3 = new Model.UserValue
            {
                UserId = user1.Id,
                Value = 2.9,
            };

            base.InsertUserValue(user1Value3);

            var user2Value1 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 3.7,
            };

            base.InsertUserValue(user2Value1);

            var user2Value2 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 4.3,
            };

            base.InsertUserValue(user2Value2);

            var user2Value3 = new Model.UserValue
            {
                UserId = user2.Id,
                Value = 5.9,
            };

            base.InsertUserValue(user2Value3);

            var userValues = new List<Model.UserValue> { user1Value1, user1Value2, user1Value3, user2Value1, user2Value2, user2Value3 };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.UserValueAmount> userValueAmounts = await dataContext
                .Select<Model.UserValue>(x => new { x.UserId },
                    aggregate: a =>
                    {
                        a.Avg<Model.UserValue>(x => x.Value, cast: "decimal(10, 5)", alias: "Amount");
                    })
                .GroupBy<Model.UserValue>(x => x.UserId)
                .QueryAsync<Model.UserValueAmount>();

            dataContext.Dispose();

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

            var user1ToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int user1Id = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalar();

            user1ToInsert.Id = user1Id;

            int user2Id = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalar();

            user2ToInsert.Id = user2Id;

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

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

            var user1ToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int user1Id = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalarAsync();

            user1ToInsert.Id = user1Id;

            int user2Id = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalarAsync();

            user2ToInsert.Id = user2Id;

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

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

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int rowNumber1 = dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            nickToUpdate += "_extra";
            passwordToUpdate += "_extra";

            int rowNumber2 = dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Update_In_Transaction_Async()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int rowNumber1 = await dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            nickToUpdate += "_extra";
            passwordToUpdate += "_extra";

            int rowNumber2 = await dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Delete_In_Transaction()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int rowNumber1 = dataContext
                .Delete<Model.User>()
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            int rowNumber2 = dataContext
                .Delete<Model.User>()
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .Execute();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(0));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Delete_In_Transaction_Async()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            base.InsertUser(user);

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int rowNumber1 = await dataContext
                .Delete<Model.User>()
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            int rowNumber2 = await dataContext
                .Delete<Model.User>()
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user.Id)
                .ExecuteAsync();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber1, Is.EqualTo(1));
            Assert.That(rowNumber2, Is.EqualTo(0));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Insert_Update_In_Transaction()
        {
            #region Arrange

            var userToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{userToInsert.Nick}_sarasa";
            string passwordToUpdate = $"{userToInsert.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int userId = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            userToInsert.Id = userId;

            int rowNumber = dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .Execute();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(userToInsert.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public async Task Insert_Update_In_Transaction_Async()
        {
            #region Arrange

            var userToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{userToInsert.Nick}_sarasa";
            string passwordToUpdate = $"{userToInsert.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int userId = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            userToInsert.Id = userId;

            int rowNumber = await dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User updatedUser = users.First();
            Assert.That(updatedUser.Id, Is.EqualTo(userToInsert.Id));
            Assert.That(updatedUser.Nick, Is.EqualTo(nickToUpdate));
            Assert.That(updatedUser.Password, Is.EqualTo(passwordToUpdate));

            #endregion
        }

        [Test]
        public void Insert_Delete_In_Transaction()
        {
            #region Arrange

            var userToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int userId = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalar();

            userToInsert.Id = userId;

            int rowNumber = dataContext
                .Delete<Model.User>()
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .Execute();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public async Task Insert_Delete_In_Transaction_Async()
        {
            #region Arrange

            var userToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int userId = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, userToInsert.Nick, userToInsert.Password)
                .ExecuteScalarAsync();

            userToInsert.Id = userId;

            int rowNumber = await dataContext
                .Delete<Model.User>()
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, userToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(1));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Insert_Update_In_Transaction_2_Same_Connection()
        {
            #region Arrange

            var user1ToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick1ToUpdate = $"{user1ToInsert.Nick}_sarasa";
            string password1ToUpdate = $"{user1ToInsert.Password}_sarasa";

            var user2ToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick2ToUpdate = $"{user2ToInsert.Nick}_sarasa";
            string password2ToUpdate = $"{user2ToInsert.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int user1Id = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalar();

            user1ToInsert.Id = user1Id;

            int rowNumber1 = dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nick1ToUpdate, password1ToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user1ToInsert.Id)
                .Execute();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            // Here we don't set 'ExecuteInTransaction' property to false
            // so, a new DbTransaction object will be created for the following queries.

            IEnumerable<Model.User> usersFirst = base.GetUsers();

            int user2Id = (int)dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalar();

            user2ToInsert.Id = user2Id;

            int rowNumber2 = dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nick2ToUpdate, password2ToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user2ToInsert.Id)
                .Execute();

            dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            IEnumerable<Model.User> usersSecond = base.GetUsers();

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

            var user1ToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick1ToUpdate = $"{user1ToInsert.Nick}_sarasa";
            string password1ToUpdate = $"{user1ToInsert.Password}_sarasa";

            var user2ToInsert = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nick2ToUpdate = $"{user2ToInsert.Nick}_sarasa";
            string password2ToUpdate = $"{user2ToInsert.Password}_sarasa";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;

            int user1Id = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, user1ToInsert.Nick, user1ToInsert.Password)
                .ExecuteScalarAsync();

            user1ToInsert.Id = user1Id;

            int rowNumber1 = await dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nick1ToUpdate, password1ToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user1ToInsert.Id)
                .ExecuteAsync();

            var dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            // Here we don't set 'ExecuteInTransaction' property to false
            // so, a new DbTransaction object will be created for the following queries.

            IEnumerable<Model.User> usersFirst = base.GetUsers();

            int user2Id = (int)await dataContext
                .Insert<Model.User>(x => new { x.Nick, x.Password }, user2ToInsert.Nick, user2ToInsert.Password)
                .ExecuteScalarAsync();

            user2ToInsert.Id = user2Id;

            int rowNumber2 = await dataContext
                .Update<Model.User>(x => new { x.Nick, x.Password }, nick2ToUpdate, password2ToUpdate)
                .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, user2ToInsert.Id)
                .ExecuteAsync();

            dbTransaction = dataContext.GetDbTransaction();
            dbTransaction.Commit();
            dbTransaction.Dispose();

            dataContext.ExecuteInTransaction = false;

            dataContext.Dispose();

            IEnumerable<Model.User> usersSecond = base.GetUsers();

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

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";
            string exceptionMessage = "Entity not found";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;
            DbTransaction dbTransaction = null;

            var exception = Assert.Throws<Exception>(() =>
            {
                try
                {
                    int userId = (int)dataContext
                        .Insert<Model.User>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                        .ExecuteScalar();

                    int rowNumber = dataContext
                        .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                        .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, -1)
                        .Execute();

                    if (rowNumber == 0)
                        throw new Exception(exceptionMessage);

                    dbTransaction = dataContext.GetDbTransaction();
                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction = dataContext.GetDbTransaction();
                    dbTransaction.Rollback();

                    throw;
                }
                finally
                {
                    dbTransaction.Dispose();
                    dataContext.ExecuteInTransaction = false;
                }
            });

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(exceptionMessage));

            IEnumerable<Model.User> users = base.GetUsers();
            Assert.That(users.Count(), Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void Transaction_Fails_Async()
        {
            #region Arrange

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            string nickToUpdate = $"{user.Nick}_sarasa";
            string passwordToUpdate = $"{user.Password}_sarasa";
            string exceptionMessage = "Entity not found";

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            dataContext.ExecuteInTransaction = true;
            DbTransaction dbTransaction = null;

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                try
                {
                    int userId = (int)await dataContext
                        .Insert<Model.User>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                        .ExecuteScalarAsync();

                    int rowNumber = await dataContext
                        .Update<Model.User>(x => new { x.Nick, x.Password }, nickToUpdate, passwordToUpdate)
                        .Where<Model.User>(x => x.Id, Common.Operators.EqualTo, -1)
                        .ExecuteAsync();

                    if (rowNumber == 0)
                        throw new Exception(exceptionMessage);

                    dbTransaction = dataContext.GetDbTransaction();
                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction = dataContext.GetDbTransaction();
                    dbTransaction.Rollback();

                    throw;
                }
                finally
                {
                    dbTransaction.Dispose();
                    dataContext.ExecuteInTransaction = false;
                }
            });

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(exceptionMessage));

            IEnumerable<Model.User> users = base.GetUsers();
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

            var dataContext = new DataContext(base._connectionString);

            Interfaces.IException exception = null;

            try
            {
                int userValueId = (int)dataContext
                    .Insert<Model.UserValue>(x => new { x.UserId, x.Value }, userId, value)
                    .ExecuteScalar();
            }
            catch (Exception ex)
            {
                exception = (Interfaces.IException)ex;
            }

            dataContext.Dispose();

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

            var dataContext = new DataContext(base._connectionString);

            Interfaces.IException exception = null;

            try
            {
                int userValueId = (int)await dataContext
                    .Insert<Model.UserValue>(x => new { x.UserId, x.Value }, userId, value)
                    .ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                exception = (Interfaces.IException)ex;
            }

            dataContext.Dispose();

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

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int rowNumber = dataContext
                .StoredProcedure<Model.SpInsertUser>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                .Execute();

            dataContext.Dispose();

            #endregion

            #region Assert

            //Assert.That(rowNumber, Is.EqualTo(1));
            //Oracle stored procedures do not return inserted rows number by default,
            //but you can change it by making it return in an explicit way and then use
            //the ExecuteAndRead<int>() method instead of Execute().

            IEnumerable<Model.User> users = base.GetUsers();

            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User insertedUser = users.First();
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

            var user = new Model.User
            {
                Nick = "Nick",
                Password = "Password",
            };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int rowNumber = await dataContext
                .StoredProcedure<Model.SpInsertUser>(x => new { x.Nick, x.Password }, user.Nick, user.Password)
                .ExecuteAsync();

            dataContext.Dispose();

            #endregion

            #region Assert

            //Assert.That(rowNumber, Is.EqualTo(1));
            //Oracle stored procedures do not return inserted rows number by default,
            //but you can change it by making it return in an explicit way and then use
            //the ExecuteAndRead<int>() method instead of Execute().

            IEnumerable<Model.User> users = base.GetUsers();

            Assert.That(users.Count(), Is.EqualTo(1));

            Model.User insertedUser = users.First();
            Assert.That(insertedUser.Id, Is.GreaterThan(0));
            Assert.That(insertedUser.Nick, Is.EqualTo(user.Nick));
            Assert.That(insertedUser.Password, Is.EqualTo(user.Password));

            #endregion
        }

        public void SpInsertUserAndGetId()
        {
            // Oracle stored procedures can't return values by default,
            // unless output variable is specified.
            // Use functions, instead.
        }

        public void SpGetUserById()
        {
            // Oracle stored procedures can't return values by default,
            // unless output variable is specified.
            // Use functions, instead.
        }

        public void SpGetUsers()
        {
            // Oracle stored procedures can't return values by default,
            // unless output variable is specified.
            // Use functions, instead.
        }

        #endregion

        #region Functions

        [Test]
        public void FnGetUsers()
        {
            #region Arrange

            base.CreateFnGetUsers();

            var user1ToInsert = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1ToInsert);

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2ToInsert);

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = dataContext
                .TableFunction<Model.FnGetUsers>()
                .ExecuteAndQuery<Model.User>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

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

            var user1ToInsert = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1ToInsert);

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2ToInsert);

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            IEnumerable<Model.User> users = await dataContext
                .TableFunction<Model.FnGetUsers>()
                .ExecuteAndQueryAsync<Model.User>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(users.Count(), Is.EqualTo(usersToInsert.Count()));

            foreach (var userToInsert in usersToInsert)
            {
                Model.User user = users.FirstOrDefault(x => x.Id == userToInsert.Id);

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

            var user1ToInsert = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1ToInsert);

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2ToInsert);

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int userId = dataContext
                .ScalarFunction<Model.FnGetUserIdByNick>(x => new { x.Nick }, user2ToInsert.Nick)
                .ExecuteAndReadScalar<int>();

            dataContext.Dispose();

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

            var user1ToInsert = new Model.User
            {
                Nick = "Nick1",
                Password = "Password1",
            };

            base.InsertUser(user1ToInsert);

            var user2ToInsert = new Model.User
            {
                Nick = "Nick2",
                Password = "Password2",
            };

            base.InsertUser(user2ToInsert);

            var usersToInsert = new List<Model.User> { user1ToInsert, user2ToInsert };

            #endregion

            #region Act

            var dataContext = new DataContext(base._connectionString);

            int userId = await dataContext
                .ScalarFunction<Model.FnGetUserIdByNick>(x => new { x.Nick }, user2ToInsert.Nick)
                .ExecuteAndReadScalarAsync<int>();

            dataContext.Dispose();

            #endregion

            #region Assert

            Assert.That(userId, Is.EqualTo(user2ToInsert.Id));

            #endregion
        }

        #endregion
    }
}
