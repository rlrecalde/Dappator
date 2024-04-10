using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Dappator.MySql.Test
{
    public class QueryBuilder
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Insert()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(3));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(now));

            #endregion
        }

        [Test]
        public void Insert_With_Object()
        {
            #region Arrange

            var anyObject = new AnyClass
            {
                Id = 23,
                Name = "Name",
                Date = DateTime.Now,
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, anyObject)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(3));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(anyObject.Id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(anyObject.Name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(anyObject.Date));

            #endregion
        }

        [Test]
        public void Insert_Bulk()
        {
            #region Arrange

            var anyObject1 = new AnyClass
            {
                Id = 23,
                Name = "Name",
                Date = DateTime.Now,
            };

            var anyObject2 = new AnyClass
            {
                Id = anyObject1.Id + 5,
                Name = anyObject1.Name + "***",
                Date = DateTime.Now,
            };

            var arrayParams = new object[][] {
                new object[] { anyObject1.Id, anyObject1.Name, anyObject1.Date },
                new object[] { anyObject2.Id, anyObject2.Name, anyObject2.Date },
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, arrayParams)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) " +
                "VALUES " +
                "(@p0, @p1, @p2), " +
                "(@p3, @p4, @p5); " +
                "SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(arrayParams.Length));
            Assert.That(queryAndValues.Values, Is.EqualTo(arrayParams));

            #endregion
        }

        [Test]
        public void Insert_Bulk_With_Object()
        {
            #region Arrange

            var anyObject1 = new AnyClass
            {
                Id = 23,
                Name = "Name",
                Date = DateTime.Now,
            };

            var anyObject2 = new AnyClass
            {
                Id = anyObject1.Id + 5,
                Name = anyObject1.Name + "***",
                Date = DateTime.Now,
            };

            var anyObjects = new List<AnyClass> { anyObject1, anyObject2 };

            var arrayParams = new object[][] {
                new object[] { anyObject1.Id, anyObject1.Name, anyObject1.Date },
                new object[] { anyObject2.Id, anyObject2.Name, anyObject2.Date },
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, anyObjects)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) " +
                "VALUES " +
                "(@p0, @p1, @p2), " +
                "(@p3, @p4, @p5); " +
                "SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(arrayParams.Length));
            Assert.That(queryAndValues.Values, Is.EqualTo(arrayParams));

            #endregion
        }

        [Test]
        public void Update_Simple()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(3));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(now));

            #endregion
        }

        [Test]
        public void Update_Where()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;
            int conditionId = 14;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, conditionId)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2 " +
                "WHERE AnyClass.Id = @p3";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(4));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(now));
            Assert.That(queryAndValues.Values[3], Is.EqualTo(conditionId));

            #endregion
        }

        [Test]
        public void Update_Where_And()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;
            int conditionId = 14;
            string conditionName = "Luis";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, conditionId)
                .And<AnyClass>(x => x.Name, Common.Operators.GreaterThan, conditionName)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2 " +
                "WHERE AnyClass.Id = @p3 " +
                "AND AnyClass.Name > @p4";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(5));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(now));
            Assert.That(queryAndValues.Values[3], Is.EqualTo(conditionId));
            Assert.That(queryAndValues.Values[4], Is.EqualTo(conditionName));

            #endregion
        }

        [Test]
        public void Update_Where_Or()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;
            int conditionId = 14;
            string conditionName = "Luis";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, conditionId)
                .Or<AnyClass>(x => x.Name, Common.Operators.GreaterThan, conditionName)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2 " +
                "WHERE AnyClass.Id = @p3 " +
                "OR AnyClass.Name > @p4";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(5));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(now));
            Assert.That(queryAndValues.Values[3], Is.EqualTo(conditionId));
            Assert.That(queryAndValues.Values[4], Is.EqualTo(conditionName));

            #endregion
        }

        [Test]
        public void Update_Where_And_Or_With_Parenthesis()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;
            int conditionId = 14;
            string conditionName = "Luis";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, conditionId)
                .And<AnyClass>(x => x.Name, Common.Operators.GreaterThan, conditionName)
                .Or<AnyClass>(x => x.Name, Common.Operators.LessThan, conditionName, openParenthesisBeforeCondition: true)
                .And<AnyClass>(x => x.Id, Common.Operators.LessThanOrEqualTo, conditionId, openParenthesisBeforeCondition: true)
                .Or<AnyClass>(x => x.Name, Common.Operators.GreaterThanOrEqualTo, conditionName, closeParenthesisAfterCondition: true)
                .CloseParenthesis()
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2 " +
                "WHERE AnyClass.Id = @p3 " +
                "AND AnyClass.Name > @p4 " +
                "OR (AnyClass.Name < @p5 AND (AnyClass.Id <= @p6 OR AnyClass.Name >= @p7))";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(8));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(now));
            Assert.That(queryAndValues.Values[3], Is.EqualTo(conditionId));
            Assert.That(queryAndValues.Values[4], Is.EqualTo(conditionName));
            Assert.That(queryAndValues.Values[5], Is.EqualTo(conditionName));
            Assert.That(queryAndValues.Values[6], Is.EqualTo(conditionId));
            Assert.That(queryAndValues.Values[7], Is.EqualTo(conditionName));

            #endregion
        }

        [Test]
        public void Update_With_Object()
        {
            #region Arrange

            var anyObject = new AnyClass
            {
                Id = 23,
                Name = "Name",
                Date = DateTime.Now,
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, anyObject)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(3));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(anyObject.Id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(anyObject.Name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(anyObject.Date));

            #endregion
        }

        [Test]
        public void Delete_Where()
        {
            #region Arrange

            int id = 23;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Delete<AnyClass>()
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "DELETE FROM AnyClass " +
                "WHERE AnyClass.Id = @p0";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(1));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));

            #endregion
        }

        [Test]
        public void Select_Simple()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Where()
        {
            #region Arrange

            int id = 23;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "WHERE AnyClass.Id = @p0";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(1));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));

            #endregion
        }

        [Test]
        public void Select_InnerJoin()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_InnerJoin_Where_And()
        {
            #region Arrange

            int id = 23;
            bool active = true;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnotherClass.Active = @p1";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(2));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(active));

            #endregion
        }

        [Test]
        public void Select_LeftJoin()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "LEFT JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_LeftJoin_Where_And()
        {
            #region Arrange

            int id = 23;
            bool active = true;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "LEFT JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnotherClass.Active = @p1";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(2));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(active));

            #endregion
        }

        [Test]
        public void Select_OrderBy()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "ORDER BY AnyClass.Id ASC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Where_OrderBy()
        {
            #region Arrange

            int id = 23;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Descending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "WHERE AnyClass.Id = @p0 " +
                "ORDER BY AnyClass.Id DESC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(1));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_OrderBy()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "ORDER BY AnotherClass.Id ASC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_InnerJoin_Where_And_OrderBy()
        {
            #region Arrange

            int id = 23;
            bool active = true;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active)
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Descending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnotherClass.Active = @p1 " +
                "ORDER BY AnotherClass.Id DESC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(2));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(active));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_Where_And_OrderBy_ThenBy_Alias()
        {
            #region Arrange

            int id = 23;
            bool active = true;
            string anyClassAlias = "AnyClassAlias";
            string anotherClassAlias = "AnotherClassAlias";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date }, alias: anyClassAlias)
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active }, sourceAlias: anyClassAlias, targetAlias: anotherClassAlias)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id, alias: anyClassAlias)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active, alias: anotherClassAlias)
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Descending, alias: anotherClassAlias)
                .ThenBy<AnyClass>(x => x.Id, Common.Orders.Ascending, alias: anyClassAlias)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT {anyClassAlias}.Id, {anyClassAlias}.Name, {anyClassAlias}.Date, {anotherClassAlias}.Id, {anotherClassAlias}.Name, {anotherClassAlias}.Active FROM AnyClass AS {anyClassAlias} " +
                $"INNER JOIN AnotherClass AS {anotherClassAlias} ON {anyClassAlias}.Id = {anotherClassAlias}.Id " +
                $"WHERE {anyClassAlias}.Id = @p0 " +
                $"AND {anotherClassAlias}.Active = @p1 " +
                $"ORDER BY {anotherClassAlias}.Id DESC, {anyClassAlias}.Id ASC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(2));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(active));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_OrderBy()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "LEFT JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "ORDER BY AnotherClass.Id ASC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_LeftJoin_Where_And_OrderBy()
        {
            #region Arrange

            int id = 23;
            bool active = true;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active)
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Descending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "LEFT JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnotherClass.Active = @p1 " +
                "ORDER BY AnotherClass.Id DESC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(2));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(active));

            #endregion
        }

        [Test]
        public void Select_Where_OrderBy_ThenBy()
        {
            #region Arrange

            int id = 23;

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Descending)
                .ThenBy<AnyClass>(x => x.Name, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "WHERE AnyClass.Id = @p0 " +
                "ORDER BY AnyClass.Id DESC, AnyClass.Name ASC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(1));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit()
        {
            #region Arrange

            int limit = 23;
            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"LIMIT {limit}";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Offset()
        {
            #region Arrange

            int limit = 23;
            int offset = 10;
            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit, offset)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"LIMIT {offset}, {limit}";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Count()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id);
                    })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "SELECT COUNT(AnyClass.Id) FROM AnyClass";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Count_Distinct()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id, distinct: true);
                    })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "SELECT COUNT(DISTINCT AnyClass.Id) FROM AnyClass";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Fields_Count_Distinct()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Date },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Name, distinct: true);
                    })
                .GroupBy<AnyClass>(x => x.Id)
                .AndBy<AnyClass>(x => x.Date)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Date, COUNT(DISTINCT AnyClass.Name) FROM AnyClass " +
                "GROUP BY AnyClass.Id, AnyClass.Date";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Fields_Count_Distinct_Alias()
        {
            #region Arrange

            string countAlias = "'Number of Names'";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Date },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Name, distinct: true, alias: countAlias);
                    })
                .GroupBy<AnyClass>(x => x.Id)
                .AndBy<AnyClass>(x => x.Date)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Date, COUNT(DISTINCT AnyClass.Name) AS {countAlias} FROM AnyClass " +
                $"GROUP BY AnyClass.Id, AnyClass.Date";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Fields_Count_Alias_OrderBy()
        {
            #region Arrange

            string countAlias = "'Number of Names'";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Date },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Name, alias: countAlias);
                    })
                .GroupBy<AnyClass>(x => x.Id)
                .AndBy<AnyClass>(x => x.Date)
                .OrderBy<AnyClass>(countAlias, Common.Orders.Descending)
                .ThenBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Date, COUNT(AnyClass.Name) AS {countAlias} FROM AnyClass " +
                $"GROUP BY AnyClass.Id, AnyClass.Date " +
                $"ORDER BY {countAlias} DESC, AnyClass.Id ASC";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Count_Join()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id);
                    })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .GroupBy<AnotherClass>(x => x.Id)
                .AndBy<AnotherClass>(x => x.Name)
                .AndBy<AnotherClass>(x => x.Active)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT COUNT(AnyClass.Id), AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "GROUP BY AnotherClass.Id, AnotherClass.Name, AnotherClass.Active";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Count_Join_Count()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id);
                        a.Count<AnotherClass>(y => y.Active);
                    })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT COUNT(AnyClass.Id), COUNT(AnotherClass.Active) FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Count_Join_Count_Distinct()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id);
                        a.Count<AnotherClass>(y => y.Active, distinct: true);
                    })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT COUNT(AnyClass.Id), COUNT(DISTINCT AnotherClass.Active) FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Count_Join_Fields_Count_Distinct()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id);
                        a.Count<AnotherClass>(y => y.Active, distinct: true);
                    })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, y => new { y.Id, y.Name })
                .GroupBy<AnotherClass>(x => x.Id)
                .AndBy<AnotherClass>(x => x.Name)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT COUNT(AnyClass.Id), COUNT(DISTINCT AnotherClass.Active), AnotherClass.Id, AnotherClass.Name FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "GROUP BY AnotherClass.Id, AnotherClass.Name";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Count_Join_Fields_Count_Distinct_Alias()
        {
            #region Arrange

            string countAlias = "'Number of Actives'";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id);
                        a.Count<AnotherClass>(y => y.Active, distinct: true, alias: countAlias);
                    })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, y => new { y.Id, y.Name })
                .GroupBy<AnotherClass>(x => x.Id)
                .AndBy<AnotherClass>(x => x.Name)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT COUNT(AnyClass.Id), COUNT(DISTINCT AnotherClass.Active) AS {countAlias}, AnotherClass.Id, AnotherClass.Name FROM AnyClass " +
                $"INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                $"GROUP BY AnotherClass.Id, AnotherClass.Name";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Fields_Count_Max_Join_Fields_Sum_Min()
        {
            #region Arrange

            string countAlias = "'Count of Ids'";
            string maxAlias = "'Max of Date'";
            string sumAlias = "'Sum of Names'";
            string minAlias = "'Min of Actives'";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Name, alias: countAlias);
                        a.Max<AnyClass>(x => x.Date, alias: maxAlias);
                        a.Sum<AnotherClass>(y => y.Name, alias: sumAlias);
                        a.Min<AnotherClass>(y => y.Active, alias: minAlias);
                    })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, y => new { y.Id })
                .GroupBy<AnyClass>(x => x.Id)
                .AndBy<AnotherClass>(x => x.Id)
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .ThenBy<AnyClass>(countAlias, Common.Orders.Ascending)
                .ThenBy<AnyClass>(maxAlias, Common.Orders.Descending)
                .ThenBy<AnyClass>(sumAlias, Common.Orders.Ascending)
                .ThenBy<AnyClass>(minAlias, Common.Orders.Descending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT" +
                $" AnyClass.Id," +
                $" COUNT(AnyClass.Name) AS {countAlias}," +
                $" MAX(AnyClass.Date) AS {maxAlias}," +
                $" SUM(AnotherClass.Name) AS {sumAlias}," +
                $" MIN(AnotherClass.Active) AS {minAlias}," +
                $" AnotherClass.Id FROM AnyClass " +
                $"INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                $"GROUP BY AnyClass.Id, AnotherClass.Id " +
                $"ORDER BY AnyClass.Id ASC, {countAlias} ASC, {maxAlias} DESC, {sumAlias} ASC, {minAlias} DESC";
            
            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Fields_Count_Max_Join_Fields_Sum_Avg_Alias_All()
        {
            #region Arrange

            string countAlias = "'Count of Ids'";
            string maxAlias = "'Max of Date'";
            string sumAlias = "'Sum of Names'";
            string minAlias = "'Min of Actives'";
            string anyClassAlias = "AnyClassAlias";
            string anotherClassAlias = "AnotherClassAlias";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id }, alias: anyClassAlias,
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Name, alias: countAlias, tableAlias: anyClassAlias);
                        a.Max<AnyClass>(x => x.Date, alias: maxAlias, tableAlias: anyClassAlias);
                        a.Sum<AnotherClass>(y => y.Name, alias: sumAlias, tableAlias: anotherClassAlias);
                        a.Avg<AnotherClass>(y => y.Active, alias: minAlias, tableAlias: anotherClassAlias);
                    })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, y => new { y.Id }, sourceAlias: anyClassAlias, targetAlias: anotherClassAlias)
                .GroupBy<AnyClass>(x => x.Id, alias: anyClassAlias)
                .AndBy<AnotherClass>(x => x.Id, alias: anotherClassAlias)
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending, alias: anyClassAlias)
                .ThenBy<AnyClass>(countAlias, Common.Orders.Ascending)
                .ThenBy<AnyClass>(maxAlias, Common.Orders.Descending)
                .ThenBy<AnyClass>(sumAlias, Common.Orders.Ascending)
                .ThenBy<AnyClass>(minAlias, Common.Orders.Descending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT" +
                $" {anyClassAlias}.Id," +
                $" COUNT({anyClassAlias}.Name) AS {countAlias}," +
                $" MAX({anyClassAlias}.Date) AS {maxAlias}," +
                $" SUM({anotherClassAlias}.Name) AS {sumAlias}," +
                $" AVG({anotherClassAlias}.Active) AS {minAlias}," +
                $" {anotherClassAlias}.Id FROM AnyClass AS {anyClassAlias} " +
                $"INNER JOIN AnotherClass AS {anotherClassAlias} ON {anyClassAlias}.Id = {anotherClassAlias}.Id " +
                $"GROUP BY {anyClassAlias}.Id, {anotherClassAlias}.Id " +
                $"ORDER BY {anyClassAlias}.Id ASC, {countAlias} ASC, {maxAlias} DESC, {sumAlias} ASC, {minAlias} DESC";
            
            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Cast()
        {
            #region Arrange

            string castType = "BIGINT";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .Cast<AnyClass>(x => x.Id, castType)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = $"SELECT CAST(AnyClass.Id AS {castType}) AS Id, AnyClass.Name, AnyClass.Date FROM AnyClass";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_InnerJoin_Cast()
        {
            #region Arrange

            string castType = "BIGINT";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Cast<AnotherClass>(x => x.Id, castType)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, CAST(AnotherClass.Id AS {castType}) AS Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                $"INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Alias_Cast_LeftJoin_Alias_Cast()
        {
            #region Arrange

            string alias1 = "a1";
            string alias2 = "a2";
            string castType1 = "BIGINT";
            string castType2 = "DECIMAL(5,1)";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date }, alias: alias1)
                .Cast<AnyClass>(x => x.Id, castType1, alias1)
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active }, sourceAlias: alias1, targetAlias: alias2)
                .Cast<AnotherClass>(x => x.Id, castType2, alias: alias2)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                $"SELECT" +
                $" CAST({alias1}.Id AS {castType1}) AS Id," +
                $" {alias1}.Name," +
                $" {alias1}.Date," +
                $" CAST({alias2}.Id AS {castType2}) AS Id," +
                $" {alias2}.Name," +
                $" {alias2}.Active " +
                $"FROM AnyClass AS {alias1} " +
                $"LEFT JOIN AnotherClass AS {alias2} ON {alias1}.Id = {alias2}.Id";

            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.IsNull(queryAndValues.Values);

            #endregion
        }

        [Test]
        public void Select_Operators()
        {
            #region Arrange

            int id = 23;
            string name = "Name";
            DateTime date = DateTime.Now;
            object[] inValues = new object[] { date, date, date };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnyClass>(x => x.Name, Common.Operators.NotEqualTo, name)
                .Or<AnyClass>(x => x.Date, Common.Operators.GreaterThan, date)
                .And<AnyClass>(x => x.Name, Common.Operators.GreaterThanOrEqualTo, name)
                .Or<AnyClass>(x => x.Date, Common.Operators.LessThan, date)
                .And<AnyClass>(x => x.Name, Common.Operators.LessThanOrEqualTo, name)
                .Or<AnyClass>(x => x.Date, Common.Operators.IsNull)
                .And<AnyClass>(x => x.Name, Common.Operators.IsNotNull)
                .Or<AnyClass>(x => x.Date, Common.Operators.In, inValues)
                .And<AnyClass>(x => x.Name, Common.Operators.Like, name)
                .Or<AnyClass>(x => x.Date, Common.Operators.Between, date, date)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnyClass.Name <> @p1 " +
                "OR AnyClass.Date > @p2 " +
                "AND AnyClass.Name >= @p3 " +
                "OR AnyClass.Date < @p4 " +
                "AND AnyClass.Name <= @p5 " +
                "OR AnyClass.Date IS NULL " +
                "AND AnyClass.Name IS NOT NULL " +
                "OR AnyClass.Date IN (@p6, @p7, @p8) " +
                "AND AnyClass.Name LIKE @p9 " +
                "OR AnyClass.Date BETWEEN @p10 AND @p11";
            
            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(12));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(id));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[2], Is.EqualTo(date));
            Assert.That(queryAndValues.Values[3], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[4], Is.EqualTo(date));
            Assert.That(queryAndValues.Values[5], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[6], Is.EqualTo(inValues[0]));
            Assert.That(queryAndValues.Values[7], Is.EqualTo(inValues[1]));
            Assert.That(queryAndValues.Values[8], Is.EqualTo(inValues[2]));
            Assert.That(queryAndValues.Values[9], Is.EqualTo(name));
            Assert.That(queryAndValues.Values[10], Is.EqualTo(date));
            Assert.That(queryAndValues.Values[11], Is.EqualTo(date));

            #endregion
        }

        [Test]
        public void StoredProcedure()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .StoredProcedure<SpInsertUser>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "CALL SpInsertUser (@p0, @p1)";
            
            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(2));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(nick));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(password));

            #endregion
        }

        public void TableFunction()
        {
            // Unlike MSSQL functions, MySQL functions can't return a bunch of rows.
            // You have to use procedures, instead.
        }

        [Test]
        public void ScalarFunction()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var queryAndValues = dataContext
                .ScalarFunction<FnGetUserId>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(queryAndValues);

            string queryToValidate = "SELECT FnGetUserId (@p0, @p1)";
            
            Assert.That(queryAndValues.Query, Is.EqualTo(queryToValidate));
            Assert.That(queryAndValues.Values.Length, Is.EqualTo(2));
            Assert.That(queryAndValues.Values[0], Is.EqualTo(nick));
            Assert.That(queryAndValues.Values[1], Is.EqualTo(password));

            #endregion
        }

        #region Entities

        [System.ComponentModel.Description("AnyClass")]
        class AnyClass
        {
            [System.ComponentModel.Description("Id")]
            public int Id { get; set; }

            [System.ComponentModel.Description("Name")]
            public string Name { get; set; }

            [System.ComponentModel.Description("Date")]
            public DateTime Date { get; set; }
        }

        [System.ComponentModel.Description("AnotherClass")]
        class AnotherClass
        {
            [System.ComponentModel.Description("Id")]
            public int Id { get; set; }

            [System.ComponentModel.Description("Name")]
            public string Name { get; set; }

            [System.ComponentModel.Description("Active")]
            public bool Active { get; set; }
        }

        [System.ComponentModel.Description("SpInsertUser")]
        class SpInsertUser
        {
            public string Nick { get; set; }

            public string Password { get; set; }
        }

        [System.ComponentModel.Description("FnGetUsers")]
        class FnGetUsers
        {
        }

        [System.ComponentModel.Description("FnGetUserId")]
        class FnGetUserId
        {
            public string Nick { get; set; }

            public string Password { get; set; }
        }

        #endregion
    }
}