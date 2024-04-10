using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Dappator.MySql.Test
{
    public class QueryBuilderError
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Insert_Class_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClassNoDescription>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ClassDescriptionAttribute.Replace("{entityName}", "AnyClassNoDescription")));

            #endregion
        }

        [Test]
        public void Insert_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClassPropertyNoDescription>(x => new { x.Name, x.Date }, "The Name", DateTime.Now)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Insert_Properties_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(null)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesNotNull));

            #endregion
        }

        [Test]
        public void Insert_Properties_Not_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => x.Name, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAnonymous));

            #endregion
        }

        [Test]
        public void Insert_Properties_Empty_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAnonymousWithProperties));

            #endregion
        }

        [Test]
        public void Insert_Properties_Quantity_Do_Not_Match_Values_Quantity()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now, "Something else")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesMatchProperties));

            #endregion
        }

        [Test]
        public void Insert_Properties_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnotherClass>(x => new { x.Id, x.AnyClassProperty }, 23, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Insert_Properties_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnotherClass>(x => new { x.Id, x.CollectionProperty }, 23, new List<int> { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Insert_Values_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Name, x.Date })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesSupplied));

            #endregion
        }

        [Test]
        public void Insert_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Bytes }, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesType));

            #endregion
        }

        [Test]
        public void Insert_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Id }, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesType));

            #endregion
        }

        [Test]
        public void Insert_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Strings, x.Name }, new string[] { "1", "2" }, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAndValuesByteArray));

            #endregion
        }

        [Test]
        public void Insert_Bulk_Array_Not_Array()
        {
            #region Arrange

            var arrayParams = new object[]
            {
                new object[] { "Name", DateTime.Now },
                "Name",
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Name, x.Date }, arrayParams)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesType));

            #endregion
        }

        [Test]
        public void Insert_Bulk_Array_Do_Not_Match_Properties()
        {
            #region Arrange

            var arrayParams = new object[]
            {
                new object[] { "Name", DateTime.Now },
                new object[] { "Name", DateTime.Now, "Something else" },
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Name, x.Date }, arrayParams)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesArrayMatchProperties));

            #endregion
        }

        [Test]
        public void Insert_Bulk_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var arrayParams = new object[]
            {
                new object[] { 23 },
                new object[] { 24 },
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Bytes }, arrayParams)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesType));

            #endregion
        }

        [Test]
        public void Insert_Bulk_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var arrayParams = new object[]
            {
                new object[] { new byte[] { 1, 2 } },
                new object[] { new byte[] { 3, 4 } },
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Id }, arrayParams)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesType));

            #endregion
        }

        [Test]
        public void Insert_Bulk_With_StringArray_Property_Value()
        {
            #region Arrange

            var arrayParams = new object[]
            {
                new object[] { new string[] { "1", "2" }, "Name1" },
                new object[] { new string[] { "3", "4" }, "Name2" },
            };

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Insert<AnyClass>(x => new { x.Strings, x.Name }, arrayParams)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAndValuesByteArray));

            #endregion
        }

        [Test]
        public void Update_Class_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClassNoDescription>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ClassDescriptionAttribute.Replace("{entityName}", "AnyClassNoDescription")));

            #endregion
        }

        [Test]
        public void Update_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClassPropertyNoDescription>(x => new { x.Name, x.Date }, "The Name", DateTime.Now)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Update_Properties_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(null)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesNotNull));

            #endregion
        }

        [Test]
        public void Update_Properties_Not_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => x.Name, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAnonymous));

            #endregion
        }

        [Test]
        public void Update_Properties_Empty_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAnonymousWithProperties));

            #endregion
        }

        [Test]
        public void Update_Properties_Quantity_Do_Not_Match_Values_Quantity()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now, "Something else")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesMatchProperties));

            #endregion
        }

        [Test]
        public void Update_Properties_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnotherClass>(x => new { x.Id, x.AnyClassProperty }, 23, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Update_Properties_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnotherClass>(x => new { x.Id, x.CollectionProperty }, 23, new List<int> { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Update_Values_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesSupplied));

            #endregion
        }

        [Test]
        public void Update_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Bytes }, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesType));

            #endregion
        }

        [Test]
        public void Update_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesType));

            #endregion
        }

        [Test]
        public void Update_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Strings, x.Name }, new string[] { "1", "2" }, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAndValuesByteArray));

            #endregion
        }

        [Test]
        public void Delete_Class_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClassNoDescription>()
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ClassDescriptionAttribute.Replace("{entityName}", "AnyClassNoDescription")));

            #endregion
        }

        [Test]
        public void Select_Class_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClassNoDescription>(x => new { x.Name, x.Date })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ClassDescriptionAttribute.Replace("{entityName}", "AnyClassNoDescription")));

            #endregion
        }

        [Test]
        public void Select_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClassPropertyNoDescription>(x => new { x.Name, x.Date })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_Properties_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(null)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesNotNull));

            #endregion
        }

        [Test]
        public void Select_Properties_Not_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => x.Name)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAnonymous));

            #endregion
        }

        [Test]
        public void Select_Properties_Empty_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesEmptyAggregateNotNull));

            #endregion
        }

        [Test]
        public void Select_Properties_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.AnyClassProperty })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_Properties_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.CollectionProperty })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_Count_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClassPropertyNoDescription>(x => new { x.Id },
                        aggregate: a =>
                        {
                            a.Count<AnyClassPropertyNoDescription>(x => x.Name);
                        })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_Count_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id },
                        aggregate: a =>
                        {
                            a.Count<AnyClass>(null);
                        })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Select_Count_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id },
                        aggregate: a =>
                        {
                            a.Count<AnyClass>(x => new { x.Name });
                        })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_Count_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id },
                        aggregate: a =>
                        {
                            a.Count<AnotherClass>(x => x.AnyClassProperty);
                        })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_Count_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id },
                        aggregate: a =>
                        {
                            a.Count<AnotherClass>(x => x.CollectionProperty);
                        })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_Cast_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id })
                    .Cast<AnyClass>(null, "something")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Select_Cast_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id })
                    .Cast<AnyClass>(x => new { x.Id }, "something")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_Cast_Alias_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id })
                    .Cast<AnyClass>(x => x.Id, null)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.CastNotNullOrEmpty));

            #endregion
        }

        [Test]
        public void Select_Cast_Property_Not_Found_At_Select()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id })
                    .Cast<AnyClass>(x => x.Date, "DATETIME")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.CastMatchProperty));

            #endregion
        }

        [Test]
        public void Update_Where_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClassPropertyNoDescription>(x => new { x.Date }, DateTime.Now)
                    .Where<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Update_Where_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Update_Where_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Update_Where_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnotherClass>(x => new { x.Id }, 23)
                    .Where<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Update_Where_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnotherClass>(x => new { x.Id }, 23)
                    .Where<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Update_Where_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Update_Where_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Update_Where_And_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClassPropertyNoDescription>(x => new { x.Date }, DateTime.Now)
                    .Where<AnyClassPropertyNoDescription>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Update_Where_And_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Update_Where_And_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Update_Where_And_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnotherClass>(x => new { x.Id }, 23)
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Update_Where_And_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnotherClass>(x => new { x.Id }, 23)
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Update_Where_And_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_And_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_And_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_And_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_And_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_And_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Update_Where_And_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_And_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_And_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_And_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Update_Where_Or_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClassPropertyNoDescription>(x => new { x.Date }, DateTime.Now)
                    .Where<AnyClassPropertyNoDescription>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Update_Where_Or_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Update_Where_Or_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Update_Where_Or_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnotherClass>(x => new { x.Id }, 23)
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Update_Where_Or_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnotherClass>(x => new { x.Id }, 23)
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Update_Where_Or_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_Or_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_Or_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_Or_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_Or_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Update_Where_Or_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Update_Where_Or_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_Or_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_Or_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Update_Where_Or_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Update<AnyClass>(x => new { x.Id }, 23)
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Delete_Where_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClassPropertyNoDescription>()
                    .Where<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Delete_Where_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Delete_Where_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Delete_Where_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnotherClass>()
                    .Where<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Delete_Where_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnotherClass>()
                    .Where<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Delete_Where_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Delete_Where_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Delete_Where_And_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClassPropertyNoDescription>()
                    .Where<AnyClassPropertyNoDescription>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Delete_Where_And_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Delete_Where_And_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Delete_Where_And_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnotherClass>()
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Delete_Where_And_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnotherClass>()
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Delete_Where_And_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_And_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_And_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_And_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_And_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_And_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Delete_Where_And_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_And_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_And_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_And_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClassPropertyNoDescription>()
                    .Where<AnyClassPropertyNoDescription>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnotherClass>()
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnotherClass>()
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Delete_Where_Or_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Delete<AnyClass>()
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Select_Where_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClassPropertyNoDescription>(x => new { x.Id })
                    .Where<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_Where_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Select_Where_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_Where_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id })
                    .Where<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_Where_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id })
                    .Where<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_Where_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Select_Where_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Select_Where_And_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClassPropertyNoDescription>(x => new { x.Id })
                    .Where<AnyClassPropertyNoDescription>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_Where_And_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Select_Where_And_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_Where_And_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id })
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_Where_And_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id })
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_Where_And_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_And_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_And_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_And_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_And_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_And_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Select_Where_And_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_And_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_And_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_And_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .And<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Select_Where_Or_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClassPropertyNoDescription>(x => new { x.Id })
                    .Where<AnyClassPropertyNoDescription>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClassPropertyNoDescription>(x => x.Name, Common.Operators.EqualTo, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_Where_Or_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(null, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyNotNull));

            #endregion
        }

        [Test]
        public void Select_Where_Or_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => new { x.Id }, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_Where_Or_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id })
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnotherClass>(x => x.AnyClassProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_Where_Or_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id })
                    .Where<AnotherClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnotherClass>(x => x.CollectionProperty, Common.Operators.EqualTo, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_Where_Or_Value_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotIsNullValueSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_Or_IsNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.IsNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_Or_IsNotNull_Value_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.IsNotNull, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorIsNullValueNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_Or_Not_Between_Value_And_ValueTo_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23, 24)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorNotBetweenValueToNotSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_Or_Between_ValueTo_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.Between, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueToSupplied));

            #endregion
        }

        [Test]
        public void Select_Where_Or_Between_Value_And_ValueTo_Not_Same_Type()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.Between, 23, "24")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorBetweenValueValueToSameType));

            #endregion
        }

        [Test]
        public void Select_Where_Or_In_Value_Not_Array()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.In, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.OperatorInValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_Or_With_ByteArray_Property_But_Not_ByteArray_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Bytes, Common.Operators.EqualTo, 23)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_Or_With_ByteArray_Value_But_Not_ByteArray_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Id, Common.Operators.EqualTo, new byte[] { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueArray));

            #endregion
        }

        [Test]
        public void Select_Where_Or_With_StringArray_Property_Value()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .Where<AnyClass>(x => x.Id, Common.Operators.EqualTo, 23)
                    .Or<AnyClass>(x => x.Strings, Common.Operators.EqualTo, new string[] { "1", "2" })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyAndValueByteArray));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_Class_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .InnerJoin<AnyClass, AnyClassNoDescription>(x => x.Id, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ClassDescriptionAttribute.Replace("{entityName}", "AnyClassNoDescription")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Date })
                    .InnerJoin<AnyClass, AnyClassPropertyNoDescription>(x => x.Name, y => y.Name)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_PropertyEntity1_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.Active })
                    .InnerJoin<AnotherClass, AnyClass>(null, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForNameNotNull.Replace("{propertyName}", "propertyEntity1")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_PropertyEntity1_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.Active })
                    .InnerJoin<AnotherClass, AnyClass>(x => new { x.Id }, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "propertyEntity1")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_With_Class_PropertyEntity1()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.Active })
                    .InnerJoin<AnotherClass, AnyClass>(x => x.AnyClassProperty, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_With_Collection_PropertyEntity1()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.Active })
                    .InnerJoin<AnotherClass, AnyClass>(x => x.CollectionProperty, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_PropertyEntity2_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Name })
                    .InnerJoin<AnyClass, AnotherClass>(x => x.Id, null)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForNameNotNull.Replace("{propertyName}", "propertyEntity2")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_PropertyEntity2_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Name })
                    .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => new { y.Id })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "propertyEntity2")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_With_Class_PropertyEntity2()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Name })
                    .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.AnyClassProperty)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_InnerJoin_With_Collection_PropertyEntity2()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Name })
                    .InnerJoin<AnyClass, AnotherClass>(x => x.Id, y => y.CollectionProperty)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_Class_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name, x.Date })
                    .LeftJoin<AnyClass, AnyClassNoDescription>(x => x.Id, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ClassDescriptionAttribute.Replace("{entityName}", "AnyClassNoDescription")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Date })
                    .LeftJoin<AnyClass, AnyClassPropertyNoDescription>(x => x.Name, y => y.Name)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_PropertyEntity1_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.Active })
                    .LeftJoin<AnotherClass, AnyClass>(null, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForNameNotNull.Replace("{propertyName}", "propertyEntity1")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_PropertyEntity1_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.Active })
                    .LeftJoin<AnotherClass, AnyClass>(x => new { x.Id }, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "propertyEntity1")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_With_Class_PropertyEntity1()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.Active })
                    .LeftJoin<AnotherClass, AnyClass>(x => x.AnyClassProperty, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_With_Collection_PropertyEntity1()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Id, x.Active })
                    .LeftJoin<AnotherClass, AnyClass>(x => x.CollectionProperty, y => y.Id)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_PropertyEntity2_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Name })
                    .LeftJoin<AnyClass, AnotherClass>(x => x.Id, null)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForNameNotNull.Replace("{propertyName}", "propertyEntity2")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_PropertyEntity2_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Name })
                    .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => new { y.Id })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "propertyEntity2")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_With_Class_PropertyEntity2()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Name })
                    .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.AnyClassProperty)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_LeftJoin_With_Collection_PropertyEntity2()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Id, x.Name })
                    .LeftJoin<AnyClass, AnotherClass>(x => x.Id, y => y.CollectionProperty)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_GroupBy_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name },
                        aggregate: a =>
                        {
                            a.Max<AnyClass>(x => x.Date);
                        })
                    .GroupBy<AnyClass>(null)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForNameNotNull.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_GroupBy_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name },
                        aggregate: a =>
                        {
                            a.Max<AnyClass>(x => x.Date);
                        })
                    .GroupBy<AnyClass>(x => new { x.Name })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_GroupBy_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Active },
                        aggregate: a =>
                        {
                            a.Max<AnotherClass>(x => x.Id);
                        })
                    .GroupBy<AnotherClass>(x => x.AnyClassProperty)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_GroupBy_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Active },
                        aggregate: a =>
                        {
                            a.Max<AnotherClass>(x => x.Id);
                        })
                    .GroupBy<AnotherClass>(x => x.CollectionProperty)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_AndBy_Property_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name },
                        aggregate: a =>
                        {
                            a.Max<AnyClass>(x => x.Date);
                        })
                    .GroupBy<AnyClass>(x => x.Name)
                    .AndBy<AnyClass>(null)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForNameNotNull.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_AndBy_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name },
                        aggregate: a =>
                        {
                            a.Max<AnyClass>(x => x.Date);
                        })
                    .GroupBy<AnyClass>(x => x.Name)
                    .AndBy<AnyClass>(x => new { x.Name })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_AndBy_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Active },
                        aggregate: a =>
                        {
                            a.Max<AnotherClass>(x => x.Id);
                        })
                    .GroupBy<AnotherClass>(x => x.Active)
                    .AndBy<AnotherClass>(x => x.AnyClassProperty)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_AndBy_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Active },
                        aggregate: a =>
                        {
                            a.Max<AnotherClass>(x => x.Id);
                        })
                    .GroupBy<AnotherClass>(x => x.Active)
                    .AndBy<AnotherClass>(x => x.CollectionProperty)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClassPropertyNoDescription>(x => new { x.Id })
                    .OrderBy<AnyClassPropertyNoDescription>(x => x.Name, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Alias_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name })
                    .OrderBy<AnyClass>(null, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.AliasNotNullOrEmpty));

            #endregion
        }

        [Test]
        public void Select_OrderBy_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name })
                    .OrderBy<AnyClass>(x => new { x.Name }, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_OrderBy_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Active })
                    .OrderBy<AnotherClass>(x => x.AnyClassProperty, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_OrderBy_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Active })
                    .OrderBy<AnotherClass>(x => x.CollectionProperty, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void Select_ThenBy_Property_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClassPropertyNoDescription>(x => new { x.Id })
                    .OrderBy<AnyClassPropertyNoDescription>(x => x.Id, Common.Orders.Ascending)
                    .ThenBy<AnyClassPropertyNoDescription>(x => x.Name, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyDescriptionAttribute.Replace("{propertyName}", "Name")));

            #endregion
        }

        [Test]
        public void Select_ThenBy_Alias_Null()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name })
                    .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                    .ThenBy<AnyClass>(null, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.AliasNotNullOrEmpty));

            #endregion
        }

        [Test]
        public void Select_ThenBy_Property_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnyClass>(x => new { x.Name })
                    .OrderBy<AnyClass>(x => x.Id, Common.Orders.Ascending)
                    .ThenBy<AnyClass>(x => new { x.Name }, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyForm.Replace("{propertyName}", "property")));

            #endregion
        }

        [Test]
        public void Select_ThenBy_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Active })
                    .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Ascending)
                    .ThenBy<AnotherClass>(x => x.AnyClassProperty, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void Select_ThenBy_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .Select<AnotherClass>(x => new { x.Active })
                    .OrderBy<AnotherClass>(x => x.Id, Common.Orders.Ascending)
                    .ThenBy<AnotherClass>(x => x.CollectionProperty, Common.Orders.Ascending)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void StoredProcedure_Class_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .StoredProcedure<AnyClassNoDescription>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ClassDescriptionAttribute.Replace("{entityName}", "AnyClassNoDescription")));

            #endregion
        }

        [Test]
        public void StoredProcedure_Properties_Not_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .StoredProcedure<AnyClass>(x => x.Name, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAnonymous));

            #endregion
        }

        [Test]
        public void StoredProcedure_Properties_Empty_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .StoredProcedure<AnyClass>(x => new { })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesNotEmptyButNull));

            #endregion
        }

        [Test]
        public void StoredProcedure_Properties_Quantity_Do_Not_Match_Values_Quantity()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .StoredProcedure<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now, "Something else")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesMatchProperties));

            #endregion
        }

        [Test]
        public void StoredProcedure_Properties_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .StoredProcedure<AnotherClass>(x => new { x.Id, x.AnyClassProperty }, 23, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void StoredProcedure_Properties_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .StoredProcedure<AnotherClass>(x => new { x.Id, x.CollectionProperty }, 23, new List<int> { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void StoredProcedure_Values_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .StoredProcedure<AnyClass>(x => new { x.Name, x.Date })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesSupplied));

            #endregion
        }

        [Test]
        public void ScalarFunction_Class_With_No_DescriptionAttribute()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .ScalarFunction<AnyClassNoDescription>(x => new { x.Name, x.Date }, "Name", DateTime.Now)
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ClassDescriptionAttribute.Replace("{entityName}", "AnyClassNoDescription")));

            #endregion
        }

        [Test]
        public void ScalarFunction_Properties_Not_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .ScalarFunction<AnyClass>(x => x.Name, "Name")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesAnonymous));

            #endregion
        }

        [Test]
        public void ScalarFunction_Properties_Empty_Anonymous()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .ScalarFunction<AnyClass>(x => new { })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertiesNotEmptyButNull));

            #endregion
        }

        [Test]
        public void ScalarFunction_Properties_Quantity_Do_Not_Match_Values_Quantity()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .ScalarFunction<AnyClass>(x => new { x.Name, x.Date }, "Name", DateTime.Now, "Something else")
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesMatchProperties));

            #endregion
        }

        [Test]
        public void ScalarFunction_Properties_With_Class_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .ScalarFunction<AnotherClass>(x => new { x.Id, x.AnyClassProperty }, 23, new AnyClass())
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "AnyClassProperty")));

            #endregion
        }

        [Test]
        public void ScalarFunction_Properties_With_Collection_Property()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .ScalarFunction<AnotherClass>(x => new { x.Id, x.CollectionProperty }, 23, new List<int> { 1, 2 })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.PropertyPrimitive.Replace("{propertyName}", "CollectionProperty")));

            #endregion
        }

        [Test]
        public void ScalarFunction_Values_Not_Supplied()
        {
            #region Arrange

            var dataContext = new DataContext(null);

            #endregion

            #region Act

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var queryAndValues = dataContext
                    .ScalarFunction<AnyClass>(x => new { x.Name, x.Date })
                    .GetQuery();
            });

            #endregion

            #region Assert

            Assert.That(exception.Message, Is.EqualTo(Internal.Constants.ValuesSupplied));

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

            [System.ComponentModel.Description("Bytes")]
            public byte[] Bytes { get; set; }

            [System.ComponentModel.Description("Strings")]
            public string[] Strings { get; set; }
        }

        [System.ComponentModel.Description("AnotherClass")]
        class AnotherClass
        {
            [System.ComponentModel.Description("Id")]
            public int Id { get; set; }

            [System.ComponentModel.Description("AnyClassProperty")]
            public AnyClass AnyClassProperty { get; set; }

            [System.ComponentModel.Description("CollectionProperty")]
            public IEnumerable<int> CollectionProperty { get; set; }

            [System.ComponentModel.Description("Active")]
            public bool Active { get; set; }
        }

        class AnyClassNoDescription
        {
            [System.ComponentModel.Description("Id")]
            public int Id { get; set; }

            [System.ComponentModel.Description("Name")]
            public string Name { get; set; }

            [System.ComponentModel.Description("Date")]
            public DateTime Date { get; set; }
        }

        [System.ComponentModel.Description("AnyClassPropertyNoDescription")]
        class AnyClassPropertyNoDescription
        {
            [System.ComponentModel.Description("Id")]
            public int Id { get; set; }

            public string Name { get; set; }

            [System.ComponentModel.Description("Date")]
            public DateTime Date { get; set; }
        }

        #endregion
    }
}
