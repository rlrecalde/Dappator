namespace Dappator.Test
{
    public class See_Firebird_QueryBuilder
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Insert_Sql()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Insert_Bulk_Sql()
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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, arrayParams)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) " +
                "VALUES " +
                "(@p0, @p1, @p2), " +
                "(@p3, @p4, @p5); " +
                "SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";
            
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Insert_Sqlite()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;

            var sqlContext = new SqliteContext("");

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(last_insert_rowid() AS BIGINT)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Insert_Bulk_Sqlite()
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

            var sqlContext = new SqliteContext("");

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, arrayParams)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) " +
                "VALUES " +
                "(@p0, @p1, @p2), " +
                "(@p3, @p4, @p5); " +
                "SELECT CAST(last_insert_rowid() AS BIGINT)";

            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Insert_NpgSql()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;

            var sqlContext = new NpgsqlContext("Server=localhost");

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "INSERT INTO AnyClass (Id, Name, Date) VALUES (@p0, @p1, @p2) RETURNING CAST(id AS BIGINT)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Insert_Bulk_NpgSql()
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

            var sqlContext = new NpgsqlContext("Server=localhost");

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, arrayParams)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) " +
                "VALUES " +
                "(@p0, @p1, @p2), " +
                "(@p3, @p4, @p5) " +
                "RETURNING CAST(id AS BIGINT)";

            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Insert_MySql()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;

            var sqlContext = new MySqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) VALUES (@p0, @p1, @p2); " +
                "SELECT CAST(LAST_INSERT_ID() AS SIGNED)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Insert_Bulk_MySql()
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

            var sqlContext = new MySqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, arrayParams)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) " +
                "VALUES " +
                "(@p0, @p1, @p2), " +
                "(@p3, @p4, @p5); " +
                "SELECT CAST(LAST_INSERT_ID() AS SIGNED)";

            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Insert_Firebird()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;

            var sqlContext = new FirebirdContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Insert<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "INSERT INTO AnyClass (Id, Name, Date) VALUES (@p0, @p1, @p2) " +
                "RETURNING CAST(\"Id\" AS BIGINT);";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Update_Simple()
        {
            #region Arrange

            int id = 23;
            string name = "Roberto";
            DateTime now = DateTime.Now;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2";
            Assert.That(query, Is.EqualTo(queryToValidate));

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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, conditionId)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2 " +
                "WHERE AnyClass.Id = @p3";
            Assert.That(query, Is.EqualTo(queryToValidate));

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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, conditionId)
                .And<AnyClass>(x => x.Name, Common.Operators.GreaterThan, conditionName)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2 " +
                "WHERE AnyClass.Id = @p3 " +
                "AND AnyClass.Name > @p4";
            Assert.That(query, Is.EqualTo(queryToValidate));

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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Update<AnyClass>(x => new { x.Id, x.Name, x.Date }, id, name, now)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, conditionId)
                .Or<AnyClass>(x => x.Name, Common.Operators.GreaterThan, conditionName)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2 " +
                "WHERE AnyClass.Id = @p3 " +
                "OR AnyClass.Name > @p4";
            Assert.That(query, Is.EqualTo(queryToValidate));

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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "UPDATE AnyClass SET Id = @p0, Name = @p1, Date = @p2 " +
                "WHERE AnyClass.Id = @p3 " +
                "AND AnyClass.Name > @p4 " +
                "OR (AnyClass.Name < @p5 AND (AnyClass.Id <= @p6 OR AnyClass.Name >= @p7))";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Delete_Where()
        {
            #region Arrange

            int id = 23;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Delete<AnyClass>()
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "DELETE FROM AnyClass " +
                "WHERE AnyClass.Id = @p0";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Simple()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Where()
        {
            #region Arrange

            int id = 23;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "WHERE AnyClass.Id = @p0";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_InnerJoin()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_Where_And()
        {
            #region Arrange

            int id = 23;
            bool active = true;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnotherClass.Active = @p1";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_LeftJoin()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "LEFT JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_Where_And()
        {
            #region Arrange

            int id = 23;
            bool active = true;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "LEFT JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnotherClass.Active = @p1";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "ORDER BY AnyClass.Id ASC";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Where_OrderBy()
        {
            #region Arrange

            int id = 23;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Descending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "WHERE AnyClass.Id = @p0 " +
                "ORDER BY AnyClass.Id DESC";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_OrderBy()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "ORDER BY AnotherClass.Id ASC";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_Where_And_OrderBy()
        {
            #region Arrange

            int id = 23;
            bool active = true;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active)
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Descending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnotherClass.Active = @p1 " +
                "ORDER BY AnotherClass.Id DESC";
            Assert.That(query, Is.EqualTo(queryToValidate));

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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date }, alias: anyClassAlias)
                .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active }, sourceAlias: anyClassAlias, targetAlias: anotherClassAlias)
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id, alias: anyClassAlias)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active, alias: anotherClassAlias)
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Descending, alias: anotherClassAlias)
                .ThenBy<AnyClass>(x => x.Id, Common.Orders.Ascending, alias: anyClassAlias)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT {anyClassAlias}.Id, {anyClassAlias}.Name, {anyClassAlias}.Date, {anotherClassAlias}.Id, {anotherClassAlias}.Name, {anotherClassAlias}.Active FROM AnyClass AS {anyClassAlias} " +
                $"INNER JOIN AnotherClass AS {anotherClassAlias} ON {anyClassAlias}.Id = {anotherClassAlias}.Id " +
                $"WHERE {anyClassAlias}.Id = @p0 " +
                $"AND {anotherClassAlias}.Active = @p1 " +
                $"ORDER BY {anotherClassAlias}.Id DESC, {anyClassAlias}.Id ASC";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_OrderBy()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "LEFT JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "ORDER BY AnotherClass.Id ASC";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_Where_And_OrderBy()
        {
            #region Arrange

            int id = 23;
            bool active = true;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.Id, z => new { z.Id, z.Name, z.Active })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .And<AnotherClass>(x => x.Active, Common.Operators.EqualTo, active)
                .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Descending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date, AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "LEFT JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "WHERE AnyClass.Id = @p0 " +
                "AND AnotherClass.Active = @p1 " +
                "ORDER BY AnotherClass.Id DESC";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Where_OrderBy_ThenBy()
        {
            #region Arrange

            int id = 23;

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, id)
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Descending)
                .ThenBy<AnyClass>(x => x.Name, Common.Orders.Ascending)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                "WHERE AnyClass.Id = @p0 " +
                "ORDER BY AnyClass.Id DESC, AnyClass.Name ASC";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Sql()
        {
            #region Arrange

            int limit = 23;
            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"OFFSET 0 ROWS FETCH NEXT {limit} ROWS ONLY";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Offset_Sql()
        {
            #region Arrange

            int limit = 23;
            int offset = 10;
            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit, offset)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Sqlite()
        {
            #region Arrange

            int limit = 23;
            var sqlContext = new SqliteContext("");

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"LIMIT {limit}";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Offset_Sqlite()
        {
            #region Arrange

            int limit = 23;
            int offset = 10;
            var sqlContext = new SqliteContext("");

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit, offset)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"LIMIT {limit} OFFSET {offset}";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_NpgSql()
        {
            #region Arrange

            int limit = 23;
            var sqlContext = new NpgsqlContext("Server=localhost");

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"LIMIT {limit}";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Offset_NpgSql()
        {
            #region Arrange

            int limit = 23;
            int offset = 10;
            var sqlContext = new NpgsqlContext("Server=localhost");

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit, offset)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"LIMIT {limit} OFFSET {offset}";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_MySql()
        {
            #region Arrange

            int limit = 23;
            var sqlContext = new MySqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"LIMIT {limit}";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Offset_MySql()
        {
            #region Arrange

            int limit = 23;
            int offset = 10;
            var sqlContext = new MySqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit, offset)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"LIMIT {offset}, {limit}";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Firebird()
        {
            #region Arrange

            int limit = 23;
            var sqlContext = new FirebirdContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"OFFSET 0 ROWS FETCH NEXT {limit} ROWS ONLY";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Limit_Offset_Firebird()
        {
            #region Arrange

            int limit = 23;
            int offset = 10;
            var sqlContext = new FirebirdContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                .LimitOffset(limit, offset)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Name, AnyClass.Date FROM AnyClass " +
                $"ORDER BY AnyClass.Id ASC " +
                $"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Count()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id);
                    })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT COUNT(AnyClass.Id) FROM AnyClass";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Count_Distinct()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .Select<AnyClass>(x => new { },
                    aggregate: a =>
                    {
                        a.Count<AnyClass>(x => x.Id, distinct: true);
                    })
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT COUNT(DISTINCT AnyClass.Id) FROM AnyClass";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Fields_Count_Distinct()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT AnyClass.Id, AnyClass.Date, COUNT(DISTINCT AnyClass.Name) FROM AnyClass " +
                "GROUP BY AnyClass.Id, AnyClass.Date";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Fields_Count_Distinct_Alias()
        {
            #region Arrange

            string countAlias = "'Number of Names'";

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Date, COUNT(DISTINCT AnyClass.Name) AS {countAlias} FROM AnyClass " +
                $"GROUP BY AnyClass.Id, AnyClass.Date";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Fields_Count_Alias_OrderBy()
        {
            #region Arrange

            string countAlias = "'Number of Names'";

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT AnyClass.Id, AnyClass.Date, COUNT(AnyClass.Name) AS {countAlias} FROM AnyClass " +
                $"GROUP BY AnyClass.Id, AnyClass.Date " +
                $"ORDER BY {countAlias} DESC, AnyClass.Id ASC";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Count_Join()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT COUNT(AnyClass.Id), AnotherClass.Id, AnotherClass.Name, AnotherClass.Active FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "GROUP BY AnotherClass.Id, AnotherClass.Name, AnotherClass.Active";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Count_Join_Count()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT COUNT(AnyClass.Id), COUNT(AnotherClass.Active) FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Count_Join_Count_Distinct()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT COUNT(AnyClass.Id), COUNT(DISTINCT AnotherClass.Active) FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Count_Join_Fields_Count_Distinct()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                "SELECT COUNT(AnyClass.Id), COUNT(DISTINCT AnotherClass.Active), AnotherClass.Id, AnotherClass.Name FROM AnyClass " +
                "INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                "GROUP BY AnotherClass.Id, AnotherClass.Name";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void Select_Count_Join_Fields_Count_Distinct_Alias()
        {
            #region Arrange

            string countAlias = "'Number of Actives'";

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

            string queryToValidate = "" +
                $"SELECT COUNT(AnyClass.Id), COUNT(DISTINCT AnotherClass.Active) AS {countAlias}, AnotherClass.Id, AnotherClass.Name FROM AnyClass " +
                $"INNER JOIN AnotherClass ON AnyClass.Id = AnotherClass.Id " +
                $"GROUP BY AnotherClass.Id, AnotherClass.Name";
            Assert.That(query, Is.EqualTo(queryToValidate));

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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

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
            Assert.That(query, Is.EqualTo(queryToValidate));

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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

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
            Assert.That(query, Is.EqualTo(queryToValidate));

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

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
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

            Assert.IsNotNull(query);

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
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void StoredProcedure_Sql()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .StoredProcedure<SpInsertUser>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "EXEC SpInsertUser @p0, @p1";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void StoredProcedure_NpgSql()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var sqlContext = new NpgsqlContext("Server=localhost");

            #endregion

            #region Act

            string query = sqlContext
                .StoredProcedure<SpInsertUser>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "CALL SpInsertUser (@p0, @p1)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void StoredProcedure_MySql()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var sqlContext = new MySqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .StoredProcedure<SpInsertUser>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "CALL SpInsertUser (@p0, @p1)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void StoredProcedure_Firebird()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var sqlContext = new FirebirdContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .StoredProcedure<SpInsertUser>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "EXECUTE PROCEDURE SpInsertUser @p0, @p1";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void TableFunction_Sql()
        {
            #region Arrange

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .TableFunction<FnGetUsers>()
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT * FROM FnGetUsers ()";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void TableFunction_NpgSql()
        {
            #region Arrange

            var sqlContext = new NpgsqlContext("Server=localhost");

            #endregion

            #region Act

            string query = sqlContext
                .TableFunction<FnGetUsers>()
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT * FROM FnGetUsers ()";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void ScalarFunction_Sql()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var sqlContext = new SqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .ScalarFunction<FnGetUserId>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT FnGetUserId (@p0, @p1)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void ScalarFunction_NpgSql()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var sqlContext = new NpgsqlContext("Server=localhost");

            #endregion

            #region Act

            string query = sqlContext
                .ScalarFunction<FnGetUserId>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT FnGetUserId (@p0, @p1)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void ScalarFunction_MySql()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var sqlContext = new MySqlContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .ScalarFunction<FnGetUserId>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT FnGetUserId (@p0, @p1)";
            Assert.That(query, Is.EqualTo(queryToValidate));

            #endregion
        }

        [Test]
        public void ScalarFunction_Firebird()
        {
            #region Arrange

            string nick = "nick";
            string password = "password";

            var sqlContext = new FirebirdContext(null);

            #endregion

            #region Act

            string query = sqlContext
                .ScalarFunction<FnGetUserId>(x => new { x.Nick, x.Password }, nick, password)
                .GetQuery();

            #endregion

            #region Assert

            Assert.IsNotNull(query);

            string queryToValidate = "SELECT FnGetUserId (@p0, @p1) FROM RDB$DATABASE";
            Assert.That(query, Is.EqualTo(queryToValidate));

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