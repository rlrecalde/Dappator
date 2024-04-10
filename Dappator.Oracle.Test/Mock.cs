using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dappator.Oracle.Test
{
    public class Mock
    {
        private Mock<ICustomContext> _customContextMock;
        private Mock<Interfaces.IQueryBuilderReturning> _queryBuilderReturningMock;
        private Mock<Interfaces.IQueryBuilderQuery> _queryBuilderQueryMock;
        private Mock<Interfaces.IQueryBuilderAndOr> _queryBuilderAndOrMock;
        private Mock<Interfaces.IQueryBuilderWhere> _queryBuilderWhereMock;
        private Mock<Interfaces.IQueryBuilderAndOr> _firstQueryBuilderAndOrMock;
        private Mock<Interfaces.IQueryBuilderAndOr> _secondQueryBuilderAndOrMock;
        private Mock<Interfaces.IQueryBuilderJoin> _queryBuilderJoinMock;
        private Mock<Interfaces.IQueryBuilderAndOrGroupByOrderBy> _firstQueryBuilderAndOrOrderByMock;
        private Mock<Interfaces.IQueryBuilderAndOrGroupByOrderBy> _secondQueryBuilderAndOrOrderByMock;
        private Mock<Interfaces.IQueryBuilderAndOrGroupByOrderBy> _thirdQueryBuilderAndOrOrderByMock;
        private Mock<Interfaces.IQueryBuilderLimitOffsetThenBy> _firstQueryBuilderLimitOffsetThenByMock;
        private Mock<Interfaces.IQueryBuilderLimitOffsetThenBy> _secondQueryBuilderLimitOffsetThenByMock;
        private Mock<Interfaces.IQueryBuilderJoin> _queryBuilderInnerJoinMock;
        private Mock<Interfaces.IQueryBuilderJoin> _queryBuilderLeftJoinMock;
        private Mock<Interfaces.IQueryBuilderAndOrGroupByOrderBy> _queryBuilderAndOrOrderByMock;

        [SetUp]
        public void Setup()
        {
            this._customContextMock = new Mock<ICustomContext>();

            // For Insert()
            this._queryBuilderReturningMock = new Mock<Interfaces.IQueryBuilderReturning>();

            // For Update()
            this._queryBuilderAndOrMock = new Mock<Interfaces.IQueryBuilderAndOr>();

            // For Update() and Delete()
            this._queryBuilderWhereMock = new Mock<Interfaces.IQueryBuilderWhere>();

            // For Delete()
            this._firstQueryBuilderAndOrMock = new Mock<Interfaces.IQueryBuilderAndOr>();
            this._secondQueryBuilderAndOrMock = new Mock<Interfaces.IQueryBuilderAndOr>();

            // For Select() and Select_With_Joins()
            this._queryBuilderJoinMock = new Mock<Interfaces.IQueryBuilderJoin>();
            this._queryBuilderQueryMock = new Mock<Interfaces.IQueryBuilderQuery>();

            // For Select()
            this._firstQueryBuilderAndOrOrderByMock = new Mock<Interfaces.IQueryBuilderAndOrGroupByOrderBy>();
            this._secondQueryBuilderAndOrOrderByMock = new Mock<Interfaces.IQueryBuilderAndOrGroupByOrderBy>();
            this._thirdQueryBuilderAndOrOrderByMock = new Mock<Interfaces.IQueryBuilderAndOrGroupByOrderBy>();
            this._firstQueryBuilderLimitOffsetThenByMock = new Mock<Interfaces.IQueryBuilderLimitOffsetThenBy>();
            this._secondQueryBuilderLimitOffsetThenByMock = new Mock<Interfaces.IQueryBuilderLimitOffsetThenBy>();

            // For Select_With_Joins()
            this._queryBuilderInnerJoinMock = new Mock<Interfaces.IQueryBuilderJoin>();
            this._queryBuilderLeftJoinMock = new Mock<Interfaces.IQueryBuilderJoin>();
            this._queryBuilderAndOrOrderByMock = new Mock<Interfaces.IQueryBuilderAndOrGroupByOrderBy>();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public async Task Insert()
        {
            #region Arrange

            var anyObject = new AnyClass
            {
                Name = "Nick",
                Date = DateTime.Now,
            };

            long idToReturn = 23;

            this._queryBuilderReturningMock.Setup(x => x.ExecuteScalarAsync())
                                               .ReturnsAsync(idToReturn);

            this._customContextMock.Setup(x => x.Insert<AnyClass>(It.IsAny<Expression<Func<AnyClass, dynamic>>>(), It.IsAny<object[]>()))
                                   .Returns(this._queryBuilderReturningMock.Object);

            #endregion

            #region Act

            var someDataClass = new SomeDataClass(this._customContextMock.Object);

            await someDataClass.Insert(anyObject);

            #endregion

            #region Assert

            Assert.That(anyObject.Id, Is.EqualTo(idToReturn));

            #endregion
        }

        [Test]
        public async Task Update()
        {
            #region Arrange

            var anyObject = new AnyClass
            {
                Id = 23,
                Name = "Nick",
                Date = DateTime.Now,
            };

            int rowNumberToReturn = 1;

            this._queryBuilderAndOrMock.Setup(x => x.ExecuteAsync())
                                       .ReturnsAsync(rowNumberToReturn);

            this._queryBuilderWhereMock.Setup(x => x.Where<AnyClass>(It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Common.Operators>(), It.IsAny<object>(), It.IsAny<object>()))
                                       .Returns(this._queryBuilderAndOrMock.Object);

            this._customContextMock.Setup(x => x.Update<AnyClass>(It.IsAny<Expression<Func<AnyClass, dynamic>>>(), It.IsAny<object[]>()))
                                   .Returns(this._queryBuilderWhereMock.Object);

            #endregion

            #region Act

            var someDataClass = new SomeDataClass(this._customContextMock.Object);

            int rowNumber = await someDataClass.Update(anyObject);

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(rowNumberToReturn));

            #endregion
        }

        [Test]
        public async Task Delete()
        {
            #region Arrange

            var anyObject = new AnyClass
            {
                Id = 23,
                Name = "Nick",
                Date = DateTime.Now,
            };

            int rowNumberToReturn = 1;

            this._secondQueryBuilderAndOrMock.Setup(x => x.ExecuteAsync())
                                             .ReturnsAsync(rowNumberToReturn);

            this._firstQueryBuilderAndOrMock.Setup(x => x.And<AnyClass>(It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Common.Operators>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<bool>()))
                                            .Returns(this._secondQueryBuilderAndOrMock.Object);

            this._queryBuilderWhereMock.Setup(x => x.Where<AnyClass>(It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Common.Operators>(), It.IsAny<object>(), It.IsAny<object>()))
                                       .Returns(this._firstQueryBuilderAndOrMock.Object);

            this._customContextMock.Setup(x => x.Delete<AnyClass>())
                                   .Returns(this._queryBuilderWhereMock.Object);

            #endregion

            #region Act

            var someDataClass = new SomeDataClass(this._customContextMock.Object);

            int rowNumber = await someDataClass.Delete(anyObject);

            #endregion

            #region Assert

            Assert.That(rowNumber, Is.EqualTo(rowNumberToReturn));

            #endregion
        }

        [Test]
        public async Task Select()
        {
            #region Arrange

            var anyObjectSearch = new AnyClass
            {
                Name = "Nick",
                Date = DateTime.Now,
            };

            var anyObjectsToReturn = new List<AnyClass> { new AnyClass { Id = 1, Name = "Name", Date = DateTime.Now } };

            this._queryBuilderQueryMock.Setup(x => x.QueryAsync<AnyClass>())
                                       .ReturnsAsync(anyObjectsToReturn);

            this._secondQueryBuilderLimitOffsetThenByMock.Setup(x => x.LimitOffset(It.IsAny<int>(), It.IsAny<int>()))
                                                         .Returns(() =>
                                                         {
                                                             var response = this._queryBuilderQueryMock.Object;

                                                             return response;
                                                         });

            this._firstQueryBuilderLimitOffsetThenByMock.Setup(x => x.ThenBy<AnyClass>(It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Common.Orders>(), It.IsAny<string>()))
                                                        .Returns(() =>
                                                        {
                                                            var response = this._secondQueryBuilderLimitOffsetThenByMock.Object;

                                                            return response;
                                                        });

            this._thirdQueryBuilderAndOrOrderByMock.Setup(x => x.OrderBy<AnyClass>(It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Common.Orders>(), It.IsAny<string>()))
                                                   .Returns(this._firstQueryBuilderLimitOffsetThenByMock.Object);

            this._secondQueryBuilderAndOrOrderByMock.Setup(x => x.Or<AnyClass>(It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Common.Operators>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>()))
                                                    .Returns(this._thirdQueryBuilderAndOrOrderByMock.Object);

            this._firstQueryBuilderAndOrOrderByMock.Setup(x => x.And<AnyClass>(It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Common.Operators>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>()))
                                                   .Returns(this._secondQueryBuilderAndOrOrderByMock.Object);

            this._queryBuilderJoinMock.Setup(x => x.Where<AnyClass>(It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Common.Operators>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<string>()))
                                      .Returns(this._firstQueryBuilderAndOrOrderByMock.Object);

            this._customContextMock.Setup(x => x.Select<AnyClass>(It.IsAny<Expression<Func<AnyClass, dynamic>>>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<Action<Interfaces.IQueryBuilderAggregate>>()))
                                   .Returns(this._queryBuilderJoinMock.Object);

            #endregion

            #region Act

            var someDataClass = new SomeDataClass(this._customContextMock.Object);

            IEnumerable<AnyClass> anyObjects = await someDataClass.Select(anyObjectSearch);

            #endregion

            #region Assert

            Assert.That(anyObjects.Count(), Is.EqualTo(anyObjectsToReturn.Count()));

            foreach (var anyObjectToReturn in anyObjectsToReturn)
            {
                AnyClass anyObject = anyObjects.FirstOrDefault(x => x.Id == anyObjectToReturn.Id);

                Assert.IsNotNull(anyObject);
                Assert.That(anyObject.Name, Is.EqualTo(anyObjectToReturn.Name));
                Assert.That(anyObject.Date, Is.EqualTo(anyObjectToReturn.Date));
            }

            #endregion
        }

        [Test]
        public async Task Select_With_Joins()
        {
            #region Arrange

            var anotherObjectSearch = new AnotherClass
            {
                Id = 23,
            };

            var anotherObjectToReturn = new AnotherClass
            {
                Id = anotherObjectSearch.Id,
                Width = 26.99,
                Height = 27.01,
                AnyObjects = new List<AnyClass>
                {
                    new AnyClass
                    {
                        Id = 24,
                        AnotherObjectId = anotherObjectSearch.Id,
                        Name = "Nick",
                        Date = DateTime.Now,
                    },
                },
                AnyOtherObjects = new List<AnyOtherClass>
                {
                    new AnyOtherClass
                    {
                        Id = 25,
                        AnotherObjectId = anotherObjectSearch.Id,
                        Name = "Name",
                        Age = 99,
                    },
                },
            };

            var anotherObjectsToReturn = new List<AnotherClass> { anotherObjectToReturn };

            this._queryBuilderAndOrOrderByMock.Setup(x => x.QueryAsync<AnotherClass, AnyClass, AnyOtherClass, AnotherClass>(It.IsAny<Func<AnotherClass, AnyClass, AnyOtherClass, AnotherClass>>()))
                                              .ReturnsAsync(anotherObjectsToReturn);

            this._queryBuilderJoinMock.Setup(x => x.Where<AnotherClass>(It.IsAny<Expression<Func<AnotherClass, object>>>(), It.IsAny<Common.Operators>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<string>()))
                                      .Returns(this._queryBuilderAndOrOrderByMock.Object);

            this._queryBuilderLeftJoinMock.Setup(x => x.LeftJoin<AnotherClass, AnyOtherClass>(It.IsAny<Expression<Func<AnotherClass, object>>>(), It.IsAny<Expression<Func<AnyOtherClass, object>>>(), It.IsAny<Expression<Func<AnyOtherClass, dynamic>>>(), It.IsAny<string>(), It.IsAny<string>()))
                                          .Returns(this._queryBuilderJoinMock.Object);

            this._queryBuilderInnerJoinMock.Setup(x => x.InnerJoin<AnotherClass, AnyClass>(It.IsAny<Expression<Func<AnotherClass, object>>>(), It.IsAny<Expression<Func<AnyClass, object>>>(), It.IsAny<Expression<Func<AnyClass, dynamic>>>(), It.IsAny<string>(), It.IsAny<string>()))
                                           .Returns(this._queryBuilderLeftJoinMock.Object);

            this._customContextMock.Setup(x => x.Select<AnotherClass>(It.IsAny<Expression<Func<AnotherClass, dynamic>>>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<Action<Interfaces.IQueryBuilderAggregate>>()))
                                   .Returns(this._queryBuilderInnerJoinMock.Object);

            #endregion

            #region Act

            var someDataClass = new SomeDataClass(this._customContextMock.Object);

            IEnumerable<AnotherClass> anotherObjects = await someDataClass.SelectWithJoins(anotherObjectSearch);

            #endregion

            #region Assert

            Assert.That(anotherObjects.Count(), Is.EqualTo(anotherObjectsToReturn.Count()));

            foreach (var anotherObjectInResponse in anotherObjectsToReturn)
            {
                AnotherClass anotherObject = anotherObjects.FirstOrDefault(x => x.Id == anotherObjectInResponse.Id);

                Assert.IsNotNull(anotherObject);
                Assert.That(anotherObject.Width, Is.EqualTo(anotherObjectInResponse.Width));
                Assert.That(anotherObject.Height, Is.EqualTo(anotherObjectInResponse.Height));

                Assert.IsNotNull(anotherObject.AnyObjects);
                Assert.That(anotherObject.AnyObjects.Count(), Is.EqualTo(anotherObjectInResponse.AnyObjects.Count()));

                foreach (var anyObjectInResponse in anotherObjectInResponse.AnyObjects)
                {
                    AnyClass anyObject = anotherObject.AnyObjects.FirstOrDefault(x => x.Id == anyObjectInResponse.Id);

                    Assert.IsNotNull(anyObject);
                    Assert.That(anyObject.Name, Is.EqualTo(anyObjectInResponse.Name));
                    Assert.That(anyObject.Date, Is.EqualTo(anyObjectInResponse.Date));
                }

                Assert.IsNotNull(anotherObject.AnyOtherObjects);
                Assert.That(anotherObject.AnyOtherObjects.Count(), Is.EqualTo(anotherObjectInResponse.AnyOtherObjects.Count()));

                foreach (var anyOtherObjectInResponse in anotherObjectInResponse.AnyOtherObjects)
                {
                    AnyOtherClass anyOtherObject = anotherObject.AnyOtherObjects.FirstOrDefault(x => x.Id == anyOtherObjectInResponse.Id);

                    Assert.IsNotNull(anyOtherObject);
                    Assert.That(anyOtherObject.Name, Is.EqualTo(anyOtherObjectInResponse.Name));
                    Assert.That(anyOtherObject.Age, Is.EqualTo(anyOtherObjectInResponse.Age));
                }
            }

            #endregion
        }

        #region Entities

        class AnyClass
        {
            public int Id { get; set; }

            public int AnotherObjectId { get; set; }

            public string Name { get; set; }

            public DateTime Date { get; set; }
        }

        class AnyOtherClass
        {
            public int Id { get; set; }

            public int AnotherObjectId { get; set; }

            public string Name { get; set; }

            public int Age { get; set; }
        }

        class AnotherClass
        {
            public int Id { get; set; }

            public double Width { get; set; }

            public double Height { get; set; }

            public IList<AnyClass> AnyObjects { get; set; }

            public IList<AnyOtherClass> AnyOtherObjects { get; set; }
        }

        #endregion

        class SomeDataClass
        {
            private readonly ICustomContext _customContext;

            public SomeDataClass(ICustomContext customContext)
            {
                this._customContext = customContext;
            }

            public async Task Insert(AnyClass anyObject)
            {
                int id = (int)await this._customContext
                    .Insert<AnyClass>(x => new { x.Name, x.Date }, anyObject.Name, anyObject.Date)
                    .ExecuteScalarAsync();

                anyObject.Id = id;
            }

            public async Task<int> Update(AnyClass anyObject)
            {
                int rowNumber = await this._customContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, anyObject.Name, anyObject.Date)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, anyObject.Id)
                    .ExecuteAsync();

                return rowNumber;
            }

            public async Task<int> Delete(AnyClass anyObject)
            {
                int rowNumber = await this._customContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Name, Common.Operators.EqualTo, anyObject.Name)
                    .And<AnyClass>(x => x.Id, Common.Operators.NotEqualTo, anyObject.Id)
                    .ExecuteAsync();

                return rowNumber;
            }

            public async Task<IEnumerable<AnyClass>> Select(AnyClass anyObject)
            {
                IEnumerable<AnyClass> anyObjects = await this._customContext
                    .Select<AnyClass>(x => new { x.Id, x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, anyObject.Id)
                    .And<AnyClass>(x => x.Name, Common.Operators.Like, $"%{anyObject.Name}%")
                    .Or<AnyClass>(x => x.Date, Common.Operators.LessThan, DateTime.Now)
                    .OrderBy<AnyClass>(x => x.Id, Common.Orders.Descending)
                    .ThenBy<AnyClass>(x => x.Name, Common.Orders.Ascending)
                    .LimitOffset(10, 20)
                    .QueryAsync<AnyClass>();

                return anyObjects;
            }

            public async Task<IEnumerable<AnotherClass>> SelectWithJoins(AnotherClass anotherObject)
            {
                IEnumerable<AnotherClass> anotherObjects = (await this._customContext
                    .Select<AnotherClass>(x => new { x.Id, x.Width, x.Height })
                    .InnerJoin<AnotherClass, AnyClass>(x => x.Id, y => y.AnotherObjectId, z => new { z.Id, z.Name, z.Date })
                    .LeftJoin<AnotherClass, AnyOtherClass>(x => x.Id, y => y.AnotherObjectId, z => new { z.Id, z.Name, z.Age })
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, anotherObject.Id)
                    .QueryAsync<AnotherClass, AnyClass, AnyOtherClass, AnotherClass>(this.Mapper))
                    .Distinct();

                return anotherObjects;
            }

            private AnotherClass Mapper(AnotherClass anotherObject, AnyClass anyObject, AnyOtherClass anyOtherObject)
            {
                // Here we have to map 'AnotherClass', 'AnyClass' & 'AnyOtherClass' objects received
                // on each row of the SELECT statement in order to return just one 'AnotherClass' object
                // composed by the others.
                // For Example:

                if (anotherObject.AnyObjects == null)
                    anotherObject.AnyObjects = new List<AnyClass>();

                anotherObject.AnyObjects.Add(anyObject);

                if (anotherObject.AnyOtherObjects == null)
                    anotherObject.AnyOtherObjects = new List<AnyOtherClass>();

                if (anyOtherObject != null)     // <-- this verification is because we did a LEFT JOIN
                    anotherObject.AnyOtherObjects.Add(anyOtherObject);

                return anotherObject;
            }
        }
    }

    public interface ICustomContext : IDataContext
    {
    }

    public class CustomContext : DataContext, ICustomContext
    {
        public CustomContext(string connectionString) : base(connectionString)
        {
        }
    }
}
