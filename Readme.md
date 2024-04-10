# Dappator - Strongly Typed Query Builder + Data Context with Dapper for .NET

Yes, it's an ORM.

## Packages

| Package | Description | NuGet |
| ------- | ----------- | ----- |
| Dappator | The core library | |
| Dappator.Sql | Strongly Typed Query Builder + Data Context with Dapper for MSSQL | [![Dapper](https://img.shields.io/nuget/v/Dapper.svg)](https://www.nuget.org/packages/Dapper/) |
| Dappator.MySql | Strongly Typed Query Builder + Data Context with Dapper for MySql | [![Dapper](https://img.shields.io/nuget/v/Dapper.svg)](https://www.nuget.org/packages/Dapper/) |
| Dappator.Oracle | Strongly Typed Query Builder + Data Context with Dapper for Oracle | [![Dapper](https://img.shields.io/nuget/v/Dapper.svg)](https://www.nuget.org/packages/Dapper/) |
| Dappator.PostgreSql | Strongly Typed Query Builder + Data Context with Dapper for PostgreSql | [![Dapper](https://img.shields.io/nuget/v/Dapper.svg)](https://www.nuget.org/packages/Dapper/) |
| Dappator.Sqlite | Strongly Typed Query Builder + Data Context with Dapper for Sqlite | [![Dapper](https://img.shields.io/nuget/v/Dapper.svg)](https://www.nuget.org/packages/Dapper/) |

> [!IMPORTANT]
> You don't have to install "Dappator" package.
> Whatever the provider you want to use, you have to install "Dappator._Provider_" package.

## Usage

Declare the class that is gonna be used as the Model:
``` csharp
using System.ComponentModel;

namespace Whatever
{
    [Description("dbo.User")]
    public class User
    {
        [Description("Id")]
        public int Id { get; set; }

        [Description("Nick")]
        public string Nick { get; set; }

        [Description("[Password]")]
        public string Password { get; set; }
    }
}
```
> [!NOTE]
> _Description attribute over the class represents the table name._
> _Description attributes over the properties represent the column names._

Then, instantiate a DataContext object and use it:
``` csharp
namespace Whatever
{
    public class DataAccess
    {
        public async Task Insert(User user)
        {
            using var dataContext = new Dappator.Sql.DataContext("connectionString");

            long userId = await dataContext
                .Insert<User>(x => new { x.Nick, x.Password }, user)
                .ExecuteScalarAsync();

            user.Id = (int)userId;
        }

        public async Task<User> GetById(int userId)
        {
            using var dataContext = new Dappator.Sql.DataContext("connectionString");

            User user = await dataContext
                .Select<User>(x => new { x.Id, x.Nick, x.Password })
                .Where<User>(x => x.Id, Dappator.Common.Operators.EqualTo, userId)
                .QueryFirstOrDefaultAsync<User>();

            return user;
        }
    }
}
```

## Contents
- [How does it work](#how-does-it-work)
- [DataContext](#datacontext)
- [DataContext - Best practices for IoC](#datacontext---best-practices-for-ioc)
- [QueryBuilder Methods](#queryBuilder-methods)
- [Insert()](#insert())
- [Returning()](#returning())
- [Update()](#update())
- [Delete()](#delete())
- [Where() \[for Update & Delete\]](#where()-\[for-update-&-delete\])
- [And() & Or() \[from Update's & Delete's Where\]](#and()-&-or()-\[from-update's-&-delete's-where\])
- [CloseParenthesis()](#closeparenthesis())
- [Select()](#select())
- [Aggregate functions \[Count(), Max(), Min(), Sum() & Avg()\]](#aggregate-functions-\[count(),-max(),-min(),-sum()-&-avg()\])
- [Cast()](#cast())
- [InnerJoin() & LeftJoin()](#innerjoin()-&-leftjoin())
- [Where() \[for Select\]](#where()-\[for-select\])
- [And() & Or() \[from Select's Where\]](#and()-&-or()-\[from-select's-where\])
- [GroupBy()](#groupby())
- [AndBy()](#andby())
- [OrderBy()](#orderby())
- [ThenBy()](#thenby())
- [LimitOffset()](#limitoffset())
- [StoredProcedure()](#storedprocedure())
- [TableFunction()](#tablefunction())
- [ScalarFunction()](#scalarfunction())
- [GetQuery() & SetQuery()](#getquery()-&-setquery())
- [Managing Transactions](#managing-transactions)
- [Handling exceptions](#handling-exceptions)
- [Validations](#validations)
- [Tests & Samples](#tests-&-samples)
  - [QueryBuilder test class](#querybuilder-test-class)
  - [QueryBuilderError test class](#querybuildererror-test-class)
  - [Mock test class](#mock-test-class)
  - [Functional test class](#functional-test-class)

## How does it work

When DataContext is instantiated, it creates a DbConnection object with the supplied connectionstring.
Of course, it depends on the provider you are using. For example, for Dappator.Sql it creates a SqlConnection object; for Dappator.MySql, it creates a MySqlConnection object, etcetera.

Then, certain methods are available for creating the query.
Each of these methods makes available other methods. For example:
Select() makes available Where().
Where() makes available And(), Or().
All of them make available GroupBy(), OrderBy().
Etcetera.
Finally, the executable methods:
For Insert() -> ExecuteScalar().
For Update() & Delete() -> Execute().
For Select() -> Query(), QuerySingle(), QueryFirst(), etcetera.
All of the executable methods from Dapper (the extension methods that Dapper offers) are available.

Then, when one of those executable methods is called, the built query is sent to Dapper along with the supplied values.

Finally, when DataContext object is disposed, it closes and disposes the DbConnection object.

You can think about DataContext as a three parts object:
1. It's a QueryBuilder, in charge of creating a string query in a strongly typed manner, with no need to hardcode strings anymore.
2. A DbConnection manager, where you don't have to create, open, close and dispose a connection manually anymore.
3. A query executer through Dapper, where you don't need to use Dapper anymore.

Now, let's take a look in more detail.

## DataContext

It implements IDataContext interface, in order to be used on IoC.
It is responsible of managing the DbConnection object.

It has the following properties and methods:
- ConnectionString (readonly): Gets the connectionstring.
- DbConnection (readonly): Gets the DbConnection object.
- ExecuteInTransaction: Boolean for managing transactions.
- TransactionIsolationLevel: Sets the IsolationLevel for the Transaction.
- Buffered: Sends a boolean to Dapper for its "buffered" parameter.
- SetCommandTimeout(): Sets the command timeout.
- GetDbTransaction(): Gets the DbTransaction object.
- Select(): Starts the construction of the "SELECT" statement.
- Insert(): Starts the construction of the "INSERT" statement.
- Update(): Starts the construction of the "UPDATE" statement.
- Delete(): Starts the construction of the "DELETE" statement.
- StoredProcedure(): Starts the construction of the "EXEC [stored procedure name] [values]" statement.
- TableFunction(): Starts the construction of the "SELECT * FROM [function name] ([values])" statement.
- ScalarFunction(): Starts the construction of the "SELECT [function name] ([values])" statement.
- SetQuery(): Just sends a string query to Dapper, in case you don't want to build it with the available methods.
- Close() / CloseAsync(): Closes the DbConnection object.

## DataContext - Best practices for IoC

The basic configuration would be:
``` csharp
using Microsoft.Extensions.DependencyInjection;

namespace Whatever
{
    public static class IoC
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            services.AddScoped<Dappator.IDataContext>(serviceProvider =>
            {
                return new Dappator.Sql.DataContext(connectionString);
            });
        }
    }
}
```
And then, you inject Dappator.IDataContext wherever you want.

But sometimes, you need more than one connection to the database, let's say for CQRS compliance.
Sometimes, you want to do functional tests using a Sqlite in-memory database (which needs two different connections/providers for the same injected interface).
And sometimes, you do a Background Service (which needs a Singleton injection) besides your regular access to the database (where you also need two different connections).

So, best practice for IoC configuration is not what we've seen before.
Best practice is:

First, declare your own interface which implements Dappator.IDataContext:
``` csharp
namespace Whatever.Model
{
    public interface IMyDatabaseContext : Dappator.IDataContext
    {
    }
}
```
Then, declare your own DataContext class which derives from Dappator.Sql.DataContext and implements your interface:
``` csharp
namespace Whatever.Model
{
    public class MyDatabaseContext : Dappator.Sql.DataContext, IMyDatabaseContext
    {
        public MyDatabaseContext(string connectionString) : base(connectionString)
        {
        }
    }
}
```
Finally, you configure your IoC like this:
``` csharp
namespace Whatever
{
    public static class IoC
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            services.AddScoped<Model.IMyDatabaseContext>(serviceProvider =>
            {
                return new Model.MyDatabaseContext(connectionString);
            });
        }
    }
}
```
That way, if you need two different connections for, let's say, CQRS compliance, you just create two different interfaces with two different DataContexts.
If you need a Sqlite in-memory database connection for your tests, you just create one interface and two different DataContexts that implement that same interface.

One last interesting thing about Dappator.Sqlite package:
DataContext object for this particular package has two different constructors: one for the "string connectionstring" parameter, and the other one for "SqliteConnection" parameter; and both of them have an extra boolean parameter named "preventClosing".
This is because we know if we want to use a Sqlite in-memory database on our tests, we must create a connection once and never close it until the test is complete.
So, instead of configuring our IoC with a connectionstring for Sqlite, we configure it with a SqliteConnection we create outside the IoC configuration and, by setting "preventClosing" parameter to true, we tell DataContext to not close the connection when it is disposed or even when we explicitly call "DataContext.Close()" method in our code.
But why would we call "DataContext.Close()" method in our code, if we know everytime DataContext is disposed, it closes the connection to the database?
Well, in case you do a Background service and configure the DataContext as a Singleton object, it is gonna be alive for ever. And maybe, you don't want the connection to the database to remains open for ever. So, in that case, everytime your Background service needs to access the database, you call "DataContext.Close()" method at the end of your code.
And don't worry, the next time you ask DataContext for a new query to be executed, it opens a new connection in case it was previously closed.

## QueryBuilder Methods

The following table shows every method availabe from DataContext object and the ones that are available by them.

| Method | Available methods |   | Method | Available methods |
| ------ | --------------- | - | ------ | --------------- |
| Insert() | Returning() | | Select() | Cast() |
| | ExecuteScalar() | | | InnerJoin() |
| Update() | Where() | | | LeftJoin() |
| | Execute() | | | Where() |
| Delete() | Where() | | | GroupBy() |
| | Execute() | | | OrderBy() |
| StoredProcedure | Execute() | | | Query() |
| | ExecuteAndRead() | | | |
| TableFunction() | ExecuteAndQuery() | | | |
| ScalarFunction() | ExecuteAndReadScalar() | | | |
| Where() [_from Update & Delete_] | And() | | Where() [_From Select_] | And() |
| | Or() | | | Or() |
| | CloseParenthesis() | | | CloseParenthesis() |
| | Execute() | | | GroupBy() |
| | | | | OrderBy() |
| | | | | Query() |
| And(), Or() & CloseParenthesis() [_from Update's & Delete's Where_] | And() | | And(), Or() & CloseParenthesis() [_from Select's Where_] | And() |
| | Or() | | | Or() |
| | CloseParenthesis() | | | CloseParenthesis() |
| | Execute() | | | GroupBy() |
| | | | | OrderBy() |
| | | | | Query() |
| Cast() | Cast() | | InnerJoin() & LeftJoin() | Cast() |
| | InnerJoin() | | | InnerJoin() |
| | LeftJoin() | | | LeftJoin() |
| | Where() | | | Where() |
| | GroupBy() | | | GroupBy() |
| | OrderBy() | | | OrderBy() |
| | Query() | | | Query() |
| GroupBy() | AndBy() | | AndBy() | AndBy() |
| | OrderBy() | | | OrderBy() |
| | Query() | | | Query() |
| OrderBy() | ThenBy() | | ThenBy() | ThenBy() |
| | LimitOffset() | | | LimitOffset() |
| | Query() | | | Query() |
| LimitOffset() | Query() | | | |

## Insert()

It has three overloads:
``` csharp
Insert<T>(Expression<Func<T, dynamic>> properties, T entity)
```
``` csharp
Insert<T>(Expression<Func<T, dynamic>> properties, IEnumerable<T> entities)
```
``` csharp
Insert<T>(Expression<Func<T, dynamic>> properties, params object[] values)
```
For `properties` parameter, a lambda expression with an anonymous object containing properties of T must be supplied:
``` csharp
Insert<Model>(x => new { x.Prop1, x.Prop2, ... }, 
```
First overload:
`entity` parameter: An object of type T which contains the values to insert.

Second overload:
`entities` parameter: A collection of objects of type T for bulk insertion.

Third overload:
`values` parameter: Comma separated values to insert (for simple insertion) or an array of arrays of objects (for bulk insertion).
For example:
``` csharp
// Simple Insert
Insert<User>(x => new { x.Nick, x.Password }, "nick", "pass")

// Bulk Insert
var users = new object[][] {
    new object[] { "nick1", "pass1" },
    new object[] { "nick2", "pass2" },
};

Insert<User>(x => new { x.Nick, x.Password }, users)
```

So, this method creates the following sql query:
(recall declared class User at the begining)
``` sql
-- Simple Insert (first or third overload with comma separated values)
INSERT INTO dbo.User (Nick, [Password]) VALUES (@p0, @p1);
SELECT CAST(SCOPE_IDENTITY() AS BIGINT)

-- Bulk Insert (second or third overload with array of arrays)
INSERT INTO dbo.User (Nick, [Password]) VALUES (@p0, @p1), (@p2, @p3);
SELECT CAST(SCOPE_IDENTITY() AS BIGINT)
```
Note that the SELECT used on those statements is just for when Dappator.Sql package is used.
The following statements for returning the identity/autoincrement column value are for the following packages:
``` sql
-- Dappator.MySql:
SELECT CAST(LAST_INSERT_ID() AS SIGNED)
```
``` sql
-- Dappator.Sqlite:
SELECT CAST(last_insert_rowid() AS BIGINT)
```
``` sql
-- Dappator.Oracle:
RETURNING CAST(Id AS NUMBER)
```
``` sql
-- Dappator.PostgreSql:
RETURNING CAST(Id AS BIGINT)
```
For these last two cases, it is assumed that the identity/autoincrement column's name is "Id". But, what happens if you named that column differently?

## Returning()

It is used for Oracle and PostgreSql when you named the identity/autoincrement column different than "Id", for returning its value after using Insert().
``` csharp
Returning<T>(Expression<Func<T, object>> property) 
```
For `property` parameter, a lambda expression with a property of T must be supplied.
For example:
(assuming the identity/autoincrement column's name is "MyPkId")
``` csharp
long myId = dataContext
    .Insert<User>(x => new { x.Nick, x.Password }, "nick", "pass")
    .Returning<User>(x => x.MyPkId)
    .ExecuteScalar();
```
Since Insert() method always creates the "RETURNING CAST(Id AS ...)" string (for Oracle and PostgreSql) after the "INSERT INTO..." statement, introducing the Returning() method makes it replace/change the substring "Id" with whatever string was set on Description attribute for the property.

## Update()

It has two overloads:
``` csharp
Update<T>(Expression<Func<T, dynamic>> properties, T entity)
```
``` csharp
Update<T>(Expression<Func<T, dynamic>> properties, params object[] values)
```
For `properties` parameter, a lambda expression with an anonymous object containing properties of T must be supplied:
``` csharp
Update<Model>(x => new { x.Prop1, x.Prop2, ... }, 
```
First overload:
`entity` parameter: An object of type T which contains the values to update.

Second overload:
`values` parameter: Comma separated values to update.

## Delete()

``` csharp
Delete<T>()
```
Its usage is as easy as:
``` csharp
Delete<User>()
```
...for building the sql query:
``` sql
DELETE FROM dbo.User
```

## Where() [for Update & Delete]

``` csharp
Where<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null)
```
For `property` parameter, a lambda expression with a property of T must be supplied.
For `op` parameter, an enum value from Dappator.Common.Operators must be supplied.
For `value` parameter, a value can be supplied.
For `valueTo` parameter, a value can be supplied.

Special cases:
- In case 'Dappator.Common.Operators.In' is used, 'value' parameter has to be an Array.
- In case 'Dappator.Common.Operators.Like' is used, 'value' parameter has to include '%' symbols if it's necessary.
- In case 'Dappator.Common.Operators.IsNull' or 'Dappator.Common.Operators.IsNotNull' is used, 'value' parameter must not be supplied.
- In case 'Dappator.Common.Operators.Between' is used, 'valueTo' parameter must also be supplied.
- For every operator except for 'Between', 'valueTo' parameter must not be supplied.

For example:
``` csharp
int rowNumber = dataContext
    .Delete<User>()
    .Where<User>(x => x.Id, Dappator.Common.Operators.EqualTo, userId)
    .Execute();

if (rowNumber == 0)
    throw new Exception("Entity not found");
```
As you can see, getting the quantity of affected rows by the statement could be useful for telling that something went wrong.

## And() & Or() [from Update's & Delete's Where]

``` csharp
And<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false)
```
``` csharp
Or<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false)
```
They work pretty much the same as Where(), except for two extra boolean parameters:
`openParenthesisBeforeCondition` & `closeParenthesisAfterCondition`

The idea is to be able to enclosure conditions between parenthesis.
For example, let's say we have the following filter:
``` sql
AND condition1 OR (condition2 AND condition3)
```
- First AND doesn't need to open or close parenthesis.
- OR needs to open parenthesis before condition2.
- Second AND needs to close parenthesis after condition3.

So, in that case, we will have something like:
``` csharp
int rowNumber = await dataContext
    .Update<User>(x => new { x.Nick, x.Password }, "updatedNick", "updatedPass")
    .Where<User>(x => x.Id, Dappator.Common.Operators.EqualTo, userId)
    .And<User>(x => x.Nick, Dappator.Common.Operators.Like, "%ick%")
    .Or<User>(x => x.Nick, Dappator.Common.Operators.IsNull, openParenthesisBeforeCondition: true)
    .And<User>(x => x.Password, Dappator.Common.Operators.IsNotNull, closeParenthesisAfterCondition: true)
    .ExecuteAsync();
```

But, what if we have a more complex filter where we need to open several parenthesis and close all of them at the end (or many of them at the middle and the rest at the end)?

## CloseParenthesis()

Adds a ")" string to the query.

Let's say we have the following filter:
``` sql
AND condition1 OR (condition2 AND (condition3 OR condition4))
```
We need to close two parenthesis at the end.
So, in that case, we will have something like:
``` csharp
int rowNumber = await dataContext
    .Update<User>(x => new { x.Nick, x.Password }, "updatedNick", "updatedPass")
    .Where<User>(x => x.Id, Dappator.Common.Operators.EqualTo, userId)
    .And<User>(x => x.Nick, Dappator.Common.Operators.Like, "%ick%")
    .Or<User>(x => x.Nick, Dappator.Common.Operators.IsNull, openParenthesisBeforeCondition: true)
    .And<User>(x => x.Password, Dappator.Common.Operators.IsNotNull, openParenthesisBeforeCondition: true)
    .Or<User>(x => x.Password, Dappator.Common.Operators.IsNull, closeParenthesisAfterCondition: true)
    .CloseParenthesis()
    .ExecuteAsync();
```
And, of course, you can continue adding And(), Or() & CloseParenthesis() methods after CloseParenthesis().

## Select()

``` csharp
Select<T>(Expression<Func<T, dynamic>> properties, bool distinct = false, string alias = null, Action<Interfaces.IQueryBuilderAggregate> aggregate = null)
```
For `properties` parameter, a lambda expression with an anonymous object containing properties of T must be supplied.
For `distinct` parameter, a boolean that indicates whether a "DISTINCT" string has to be introduced in the SELECT statement.
For `alias` parameter, a string for the "AS _alias_" that represents the table when it's needed.
For `aggregate` parameter, an Action for aggregate functions.

We've already seen an example of this method at the beginning.
We'll see an example of the "alias" parameter usage later.
So now, let's focus on aggregate functions.

## Aggregate functions [Count(), Max(), Min(), Sum() & Avg()]

Example:
``` csharp
SomeClass some = await dataContext
    .Select<User>(x => new { },
        aggregate: a =>
        {
            a.Count<User>(x => x.Id, alias: "Ids");
        })
    .QueryFirstOrDefaultAsync<SomeClass>();
```
Here, we assumed that we declared a "SomeClass" class with a property named "Ids".
Then, we didn't set any property to the Select() statement because we just want a Count() over the Id column.
Then, we introduced an Action<> that allows us to call these five aggregate functions.
Finally, we set an alias to that Count().
This is important because, after the query is executed, the resulting data has to be mapped to an entity by Dapper and, of course, Dapper needs a name for every column of the resulting data in order to map them to the properties of that entity.

This example makes the following sql query:
``` sql
SELECT COUNT(dbo.User.Id) AS Ids FROM dbo.User
```

And, of course, you can add as many aggregate functions as you want:
``` csharp
SomeClass some = await dataContext
    .Select<User>(x => new { },
        aggregate: a =>
        {
            a.Count<User>(x => x.Id, alias: "Ids");
            a.Min<User>(x => x.Payment, alias: "MinimumPayment");
            a.Avg<User>(x => x.Salary, alias: "AverageSalary");
            a.Count<User>(x => x.Date, alias: "Dates");
        })
    .QueryFirstOrDefaultAsync<SomeClass>();
```

Now, let's see in more detail each of them.
``` csharp
Count<T>(Expression<Func<T, object>> property, bool distinct = false, string cast = null, string alias = null, string tableAlias = null)
```
``` csharp
Max<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null)
```
``` csharp
Min<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null)
```
``` csharp
Sum<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null)
```
``` csharp
Avg<T>(Expression<Func<T, object>> property, string cast = null, string alias = null, string tableAlias = null)
```
For `property` parameter, a lambda expression with a property of T must be supplied.
For `distinct` parameter (only available on Count()), a boolean that indicates whether a "DISTINCT" string has to be introduced in the COUNT statement.
For `cast` parameter, a string representing the data type for casting the resulting value (we will see an example later).
For `alias` parameter, a string for the "AS _alias_" that represents the column name of the resulting value.
For `tableAlias` parameter, a string for the "_alias_._ColumnName_" that represents the table alias used on the Select() statement.

## Cast()

``` csharp
Cast<T>(Expression<Func<T, object>> property, string castType, string alias = null)
```

It is used after Select() for casting some of the columns.
For example:
``` csharp
User user = await dataContext
    .Select<User>(x => new { x.Id, x.Salary, x.Payment })
    .Cast<User>(x => x.Salary, "DECIMAL(5,2)")
    .Cast<User>(x => x.Payment, "BIGINT")
    .QueryFirstOrDefaultAsync<User>();
```
This example makes the following sql query:
``` sql
SELECT
   dbo.User.Id, 
   CAST(dbo.User.Salary AS DECIMAL(5,2)) AS Salary, 
   CAST(dbo.User.Payment AS BIGINT) AS Payment 
FROM dbo.User
```
For `property` parameter, a lambda expression with a property of T must be supplied.
For `castType` parameter, a string representing the data type for casting the resulting value.
For `alias` parameter, a string for the "_alias.ColumnName_" that represents the table alias used on the Select() statement.

## InnerJoin() & LeftJoin()

``` csharp
InnerJoin<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>> properties = null, string sourceAlias = null, string targetAlias = null)
```
``` csharp
LeftJoin<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>> properties = null, string sourceAlias = null, string targetAlias = null)
```
`T1` corresponds to source table.
`T2` corresponds to target table.
For `propertyEntity1` parameter, a lambda expression with a property of T1 must be supplied.
For `propertyEntity2` parameter, a lambda expression with a property of T2 must be supplied.
For `properties` parameter, a lambda expression with an anonymous object containing properties of T2 must be supplied (the ones that you want to include in the SELECT statement).
For `sourceAlias` parameter, a string that represents the alias used on the Select() statement.
For `targetAlias` parameter, a string for the "AS _alias_" that represents the table T2.

Let's start with a simple example:
``` csharp
IEnumerable<User> users = (await dataContext
    .Select<User>(x => new { x.Id, x.Nick, x.Password })
    .LeftJoin<User, Car>(x => x.Id, y => y.UserId, y => new { y.Id, y.Brand })
    .QueryAsync<User, Car, User>(mapper)).Distinct();
```
This example makes the following sql query:
``` sql
SELECT
   dbo.User.Id,
   dbo.User.Nick,
   dbo.User.Password,
   dbo.Car.Id,
   dbo.Car.Brand
FROM dbo.User
LEFT JOIN dbo.Car
   ON dbo.User.Id = dbo.Car.UserId
```

But what if we want to join a table with itself?
Here comes in action "alias" parameters we were talking about on Select(), aggregate functions and Cast().

Let's complicate the sql query:
``` sql
SELECT
   u1.Id,
   u1.Nick,
   COUNT(u1.Payment) AS User1Payments,
   u2.Id,
   u2.Nick,
   CAST(u2.Salary AS BIGINT) AS Salary,
   AVG(u2.Payment) AS User2AveragePayment
FROM dbo.User AS u1
INNER JOIN dbo.User AS u2
   ON u1.Id = u2.AnotherId
```
Of course, GROUP BY is missing. We will talk about it later.

So, this sql query would be:
``` csharp
IEnumerable<SomeClass> some = await dataContext
    .Select<User>(x => new { x.Id, x.Nick }, alias: "u1",
        aggregate: a =>
        {
            a.Count<User>(x => x.Payment, alias: "User1Payments", tableAlias: "u1");
            a.Avg<User>(x => x.Payment, alias: "User2AveragePayment", tableAlias: "u2");
        })
    .InnerJoin<User, User>(x => x.Id, y => y.AnotherId, y => new { y.Id, y.Nick, y.Salary }, sourceAlias: "u1", targetAlias: "u2")
    .Cast<User>(x => x.Salary, "BIGINT", alias: "u2")
    .QueryAsync<SomeClass>();
```

## Where() [For Select]

``` csharp
Where<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, string alias = null)
```
This works pretty much the same as Where() for Update & Delete, except for the extra `alias` parameter.
As we've seen before, this parameter is just for determining the table where the filter is gonna be applied, in case we made some joins over same tables and we set aliases on them.

## And() & Or() [From Select's Where]

``` csharp
And<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false, string alias = null)
```
``` csharp
Or<T>(Expression<Func<T, object>> property, Common.Operators op, object value = null, object valueTo = null, bool openParenthesisBeforeCondition = false, bool closeParenthesisAfterCondition = false, string alias = null)
```
They work pretty much the same as And() & Or() from Update's & Delete's Where, except for the extra `alias` parameter.
As we've seen before, this parameter is just for determining the table where the filter is gonna be applied, in case we made some joins over same tables and we set aliases on them.

## GroupBy()

``` csharp
GroupBy<T>(Expression<Func<T, object>> property, string alias = null)
```
For `property` parameter, a lambda expression with a property of T must be supplied.
For `alias` parameter, a string for the "_alias.ColumnName_" that represents the table alias used on the Select() or xxxJoin() statements.

For example:
``` csharp
IEnumerable<SomeClass> some = await dataContext
    .Select<User>(x => new { x.Id },
        aggregate: a =>
        {
            a.Count<User>(x => x.Nick, alias: "Nicks");
        })
    .GroupBy<User>(x => x.Id)
    .QueryAsync<SomeClass>();
```
This example makes the following sql query:
``` sql
SELECT dbo.User.Id, COUNT(dbo.User.Nick) AS Nicks FROM dbo.User
GROUP BY dbo.User.Id
```

## AndBy()

``` csharp
AndBy<T>(Expression<Func<T, object>> property, string alias = null)
```
For `property` parameter, a lambda expression with a property of T must be supplied.
For `alias` parameter, a string for the "_alias.ColumnName_" that represents the table alias used on the Select() or xxxJoin() statements.

Since GroupBy() only allows us to set just one property, in case we need to add more columns to the "GROUP BY" statement, we use AndBy() (as many as we want).

For Example:
``` csharp
IEnumerable<SomeClass> some = await dataContext
    .Select<User>(x => new { x.Id, x.Password },
        aggregate: a =>
        {
            a.Count<User>(x => x.Nick, alias: "Nicks");
        })
    .GroupBy<User>(x => x.Id)
    .AndBy<User>(x => x.Password)
    .QueryAsync<SomeClass>();
```
This example makes the following sql query:
``` sql
SELECT dbo.User.Id, dbo.User.[Password], COUNT(dbo.User.Nick) AS Nicks 
FROM dbo.User
GROUP BY dbo.User.Id, dbo.User.[Password]
```

## OrderBy()

It has two overloads:
``` csharp
OrderBy<T>(Expression<Func<T, object>> property, Common.Orders order, string alias = null)
```
``` csharp
OrderBy<T>(string alias, Common.Orders order)
```
First overload:
For `property` parameter, a lambda expression with a property of T must be supplied.
For `order` parameter, an enum value from Dappator.Common.Orders must be supplied.
For `alias` parameter, a string for the "_alias.ColumnName_" that represents the table alias used on the Select() or xxxJoin() statements.

Second overload:
For `alias` parameter, a string that represents the alias used on an aggregate function.
For `order` parameter, an enum value from Dappator.Common.Orders must be supplied.

For example:
``` csharp
// Order By a Property
IEnumerable<User> users = await dataContext
    .Select<User>(x => new { x.Id, x.Nick, x.Password })
    .OrderBy<User>(x => x.Id, Dappator.Common.Orders.Descending)
    .QueryAsync<User>();

// Order By the aggregate function
IEnumerable<SomeClass> some = await dataContext
    .Select<User>(x => new { x.Id, x.Password },
        aggregate: a =>
        {
            a.Count<User>(x => x.Nick, alias: "Nicks");
        })
    .OrderBy<User>("Nicks", Dappator.Common.Orders.Ascending)
    .QueryAsync<SomeClass>();
```
This example makes the following sql queries:
``` sql
SELECT dbo.User.Id, dbo.User.Nick, dbo.User.[Password] FROM dbo.User
ORDER BY dbo.User.Id DESC

SELECT dbo.User.Id, dbo.User.[Password], COUNT(dbo.User.Nick) AS Nicks 
FROM dbo.User
ORDER BY Nicks ASC
```

## ThenBy()

It has two overloads:
``` csharp
ThenBy<T>(Expression<Func<T, object>> property, Common.Orders order, string alias = null)
```
``` csharp
ThenBy<T>(string alias, Common.Orders order)
```
First overload:
For `property` parameter, a lambda expression with a property of T must be supplied.
For `order` parameter, an enum value from Dappator.Common.Orders must be supplied.
For `alias` parameter, a string for the "_alias.ColumnName_" that represents the table alias used on the Select() or xxxJoin() statements.

Second overload:
For `alias` parameter, a string that represents the alias used on an aggregate function.
For `order` parameter, an enum value from Dappator.Common.Orders must be supplied.

Since OrderBy() only allows us to set just one property, in case we need to add more columns to the "ORDER BY" statement, we use ThenBy() (as many as we want).

For example:
``` csharp
IEnumerable<SomeClass> some = await dataContext
    .Select<User>(x => new { x.Id, x.Password },
        aggregate: a =>
        {
            a.Count<User>(x => x.Nick, alias: "Nicks");
        })
    .OrderBy<User>(x => x.Id, Dappator.Common.Orders.Ascending)
    .ThenBy<User>(x => x.Password, Dappator.Common.Orders.Descending)
    .ThenBy<User>("Nicks", Dappator.Common.Orders.Ascending)
    .QueryAsync<SomeClass>();
```
This example makes the following sql queries:
``` sql
SELECT dbo.User.Id, dbo.User.[Password], COUNT(dbo.User.Nick) AS Nicks 
FROM dbo.User
ORDER BY dbo.User.Id ASC, dbo.User.[Password] DESC, Nicks ASC
```

## LimitOffset()

``` csharp
LimitOffset(int limit, int offset = 0)
```

For example:
``` csharp
IEnumerable<User> users = await dataContext
    .Select<User>(x => new { x.Id, x.Nick, x.Password })
    .OrderBy<User>(x => x.Id, Dappator.Common.Orders.Ascending)
    .LimitOffset(10, 20)
    .QueryAsync<User>();
```
This example makes the following sql queries:
``` sql
-- For MSSQL & Oracle:
SELECT dbo.User.Id, dbo.User.Nick, dbo.User.[Password] FROM dbo.User
ORDER BY dbo.User.Id ASC
OFFSET 20 ROWS FETCH NEXT 10 ROWS ONLY

-- For PostgreSql & Sqlite:
SELECT dbo.User.Id, dbo.User.Nick, dbo.User.[Password] FROM dbo.User
ORDER BY dbo.User.Id ASC
LIMIT 10 OFFSET 20

-- For MySql:
SELECT dbo.User.Id, dbo.User.Nick, dbo.User.[Password] FROM dbo.User
ORDER BY dbo.User.Id ASC
LIMIT 20, 10
```

Given that PostgreSql, Sqlite & MySql have the ability of just setting LIMIT with no OFFSET when it's not needed, in case you do the following sentence:
``` csharp
IEnumerable<User> users = await dataContext
    .Select<User>(x => new { x.Id, x.Nick, x.Password })
    .OrderBy<User>(x => x.Id, Dappator.Common.Orders.Ascending)
    .LimitOffset(1) // offset = 0
    .QueryAsync<User>();
```
...the resulting sql queries would be:
``` sql
-- For MSSQL & Oracle:
SELECT dbo.User.Id, dbo.User.Nick, dbo.User.[Password] FROM dbo.User
ORDER BY dbo.User.Id ASC
OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY

-- For PostgreSql, Sqlite & MySql:
SELECT dbo.User.Id, dbo.User.Nick, dbo.User.[Password] FROM dbo.User
ORDER BY dbo.User.Id ASC
LIMIT 1
```

## StoredProcedure()

It executes a stored procedure.

``` csharp
StoredProcedure<T>(Expression<Func<T, dynamic>> properties = null, params object[] values)
```
For `properties` parameter, a lambda expression with an anonymous object containing properties of T can be supplied.
For `values` parameter, comma separated values can be supplied.

First of all, class T represents the stored procedure; that means, its name and its parameters.

For example, we declare the following class:
``` csharp
namespace Whatever
{
    [Description("dbo.InsertUser")]
    public class SpInsertUser
    {
        public string Nick { get; set; }

        public string Password { get; set; }
    }
}
```
We don't need to set Description attributes on the properties.

Then:
``` csharp
int rowNumber = await dataContext
    .StoredProcedure<SpInsertUser>(x => new { x.Nick, x.Password }, "nick", "pass")
    .ExecuteAsync();
```
Since we are calling Execute() / ExecuteAsync(), it returns the quantity of affected rows by the stored procedure execution for MSSQL & MySql.
Oracle & PostgreSql don't return anything by default, unless it is specified.

In case you want the stored procedure to return some data, you use ExecuteAndRead\<T\>() method where T is the type to be returned.

Example 1:
``` csharp
int userId = (int)await dataContext
    .StoredProcedure<SpInsertUserAndGetId>(x => new { x.Nick, x.Password }, "nick", "pass")
    .ExecuteAndReadAsync<long>();
```

Example 2:
``` csharp
User user = await dataContext
    .StoredProcedure<SpGetUserById>(x => new { x.Id }, 23)
    .ExecuteAndReadAsync<User>();
```

Example 3:
``` csharp
IEnumerable<User> users = await dataContext
    .StoredProcedure<SpGetUsers>()
    .ExecuteAndReadAsync<IEnumerable<User>>();
```
As you can see at this last example, "properties" and "values" parameters are not mandatory. So, in this case, "SpGetUsers" class doesn't have any properties. It just exists to declare the name of the stored procedure by its Description attribute.

But take into account:
- PostgreSql stored procedures can't return a bunch of data, so Example 3 is not possible.
- Oracle stored procedures can't return any value at all, unless output parameter is specified. So, Examples 1, 2 & 3 are not possible.

StoredProcedure() method makes the following queries:
``` sql
-- For MSSQL:
EXEC storedProcedureName @p0, @p1, ...

-- For MySql & PostgreSql:
CALL storedProcedureName (@p0, @p1, ...)

-- For Oracle:
CALL storedProcedureName (:p0, :p1, ...)
```

## TableFunction()

It executes a table-valued function.
``` csharp
TableFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values)
```
For `properties` parameter, a lambda expression with an anonymous object containing properties of T can be supplied.
For `values` parameter, comma separated values can be supplied.

It works pretty much the same as StoredProcedure().
T must be a class where its Description attribute corresponds to the name of the function and its properties, to its parameters.
But we use ExecuteAndQuery\<T\>() to return an IEnumerable\<T\>.
For example:
``` csharp
IEnumerable<User> users = await dataContext
    .TableFunction<FnGetUsers>()
    .ExecuteAndQueryAsync<User>();
```

But take into account:
- MySql functions can't return a bunch of data, so this example is not possible.

TableFunction() method makes the following queries:
``` sql
-- For MSSQL & PostgreSql:
SELECT * FROM functionName (@p0, @p1, ...)

-- For Oracle:
SELECT * FROM TABLE(functionName (:p0, :p1, ...))
```

## ScalarFunction()

It executes a scalar-valued function.
``` csharp
ScalarFunction<T>(Expression<Func<T, dynamic>> properties = null, params object[] values)
```
For `properties` parameter, a lambda expression with an anonymous object containing properties of T can be supplied.
For `values` parameter, comma separated values can be supplied.

It works pretty much the same as StoredProcedure() & TableFunction(), but we use ExecuteAndReadScalar\<T\>() to return a value of type T.
T must be a primitive type (int, string, bool, etc...).
For example:
``` csharp
int userId = await dataContext
    .ScalarFunction<FnGetUserIdByNick>(x => new { x.Nick }, "nick")
    .ExecuteAndReadScalarAsync<int>();
```

It makes the following queries:
``` sql
-- For MSSQL, MySql & PostgreSql:
SELECT functionName (@p0, @p1, ...)

-- For Oracle:
SELECT functionName (:p0, :p1, ...) FROM DUAL
```

## GetQuery() & SetQuery()

We've already barely mentioned SetQuery() at the beginning, but we didn't say anything about GetQuery() yet.

GetQuery() is always available.
It allows us to get the string built query at any moment, instead of executing it through Dapper, along with the supplied values.
It returns an object of type IQueryAndValues which has two properties: the string for the query and an array of objects for the values.

The purposes of this method are two:
1. To use DataContext object just as a QueryBuilder and continue using Dapper as before.
2. To have the possibility of adding/removing/changing any substring from the query before the execution.

SetQuery() is one of the methods available from DataContext object.
It has two input parameters: a string for the query and an array of objects for the values (params object[]).
It gets the query and the values in order to send them to Dapper by any of its available executable methods.

The purposes of this method are two:
1. To use DataContext object just as a connection manager and executer (instead of using Dapper and managing the connection by your own), but not using it as a QueryBuilder.
2. To have the possibility of executing a modified string query after using GetQuery().

The following example shows both of these methods in action:
``` csharp
var queryAndValues = dataContext
    .Select<User>(x => new { x.Id, x.Nick, x.Password })
    .Where<User>(x => x.Id, Dappator.Common.Operators.GreaterThan, 0)
    .And<User>(x => x.Nick, Dappator.Common.Operators.Like, "%N%")
    .GetQuery();

string modifiedQuery = queryAndValues.Query + "Something to add";

IEnumerable<User> users = dataContext
    .SetQuery(modifiedQuery, queryAndValues.Values)
    .Query<User>();
```

The other thing you can do for testing Dappator before implementing it in your code is:
(Suppose you have a hardcoded string query with Dapper in your code)
1. Replace your hardcoded string query with the Dappator QueryBuilder part, getting the built query with GetQuery() method and sending it to Dapper.
2. Once you are sure that everything is working fine, replace Dapper and everything related to the DbConnection object with Dappator (DataContext) and begin using Dappator from now on.

## Managing Transactions

The boolean property "ExecuteInTransaction" from DataContext object, when it's set to _true_, makes the creation of a DbTransaction object when the first (of many) executable method is about to send the query to Dapper. Then, all of the following executable methods (including the first one, of course) use that DbTransaction object until "ExecuteInTransaction" property is set back to _false_.

The method "GetDbTransaction()" from DataContext object returns that created DbTransaction object in order to accomplish a Commit(), Rollback() and Dispose() when it's necessary.

That way, a simple way to execute two statements in a transaction would be:
``` csharp
dataContext.ExecuteInTransaction = true;

int userId = (int)await dataContext
    .Insert<User>(x => new { x.Nick, x.Password }, "nick", "pass")
    .ExecuteScalarAsync();

int carId = (int)await dataContext
    .Insert<Car>(x => new { x.UserId, x.Brand }, userId, "brand")
    .ExecuteScalarAsync();

var dbTransaction = dataContext.GetDbTransaction();
dbTransaction.Commit();
dbTransaction.Dispose();

dataContext.ExecuteInTransaction = false;
```
It's important to set "ExecuteInTransaction" property to false once we finish the transaction. That way, the next statement we execute will not create a new DbTransaction object.

As best practice, we can create a class in charge of managing transactions in our data access layer in order to separate concerns.
``` csharp
using System.Data.Common;

namespace Whatever
{
    public class TransactionManager : ITransactionManager
    {
        private readonly Model.IMyDatabaseContext _myDatabaseContext;

        public TransactionManager(Model.IMyDatabaseContext myDatabaseContext)
        {
            this._myDatabaseContext = myDatabaseContext;
        }

        public async Task ExecuteAsync(Func<Task> func)
        {
            this._myDatabaseContext.ExecuteInTransaction = true;
            DbTransaction dbTransaction = null;

            try
            {
                await func();

                dbTransaction = this._myDatabaseContext.GetDbTransaction();
                dbTransaction.Commit();
            }
            catch (Exception)
            {
                dbTransaction = this._myDatabaseContext.GetDbTransaction();
                dbTransaction.Rollback();

                throw;
            }
            finally
            {
                dbTransaction.Dispose();
                this._myDatabaseContext.ExecuteInTransaction = false;
            }
        }
    }
}
```
Then, use it like this:
``` csharp
namespace Whatever
{
    public class DataAccess
    {
        private readonly Model.IMyDatabaseContext _myDatabaseContext;
        private readonly ITransactionManager _transactionManager;

        public DataAccess(Model.IMyDatabaseContext myDatabaseContext,
                          ITransactionManager transactionManager)
        {
            this._myDatabaseContext = myDatabaseContext;
            this._transactionManager = transactionManager;
        }

        public async Task Insert(User user, Car car)
        {
            await this._transactionManager.ExecuteAsync(async () =>
            {
                long userId = await this._myDatabaseContext 
                    .Insert<User>(x => new { x.Nick, x.Password }, user)
                    .ExecuteScalarAsync();

                user.Id = (int)userId;
                car.UserId = (int)userId;

                long carId = await this._myDatabaseContext 
                    .Insert<Car>(x => new { x.UserId, x.Brand }, car)
                    .ExecuteScalarAsync();

                car.Id = (int)carId;
            });
        }
    }
}
```

Finally, DataContext has the property "TransactionIsolationLevel" where you can set the kind of transaction you want. It's nullable and by default is null.

## Handling exceptions

Sometimes, you create a query that is malformed somehow (for example, it has an aggregate function with no group by) or it is not malformed but the database is unable to execute it for some reason, and the exception raised by the sql engine is not so clear, so it's difficult to understand what it's going on.

DataContext object implements Try-Catch on every executable method and raises a custom exception object that includes the string built query as a property. That way, you are able to see the query that is causing the exception.

This custom exception object is named "DappatorException", but it is internal. You don't have access to it. Fortunately, it implements IException interface (declared in Dappator.Interfaces namespace) which is public.

So, the idea is to handle exceptions like this:
``` csharp
try
{
    int userId = (int)await dataContext
        .Insert<User>(x => new { x.Nick, x.Password }, "nick", "pass")
        .ExecuteScalarAsync();
}
catch (Exception ex)
{
    Dappator.Interfaces.IException exception = (Dappator.Interfaces.IException)ex;

    Console.WriteLine(exception.Query);
    // or
    this._logger.LogException(ex, exception.Query);

    throw;
}
```

## Validations

DataContext has some validations before sending the query to Dapper.

For example, if some method (such as, Insert()) expects a parameter with a lambda expression with an anonymous object containing properties but receives a null, an empty anonymous object or a lambda expression with just a property (like x => x.Prop), an ArgumentException() with a clear friendly message is raised.

Another validation is, for example, that the quantity of properties has to be the same amount as the quantity of values sent in a 'params object[]' parameter.

An another validation is that every class used as T on every method must have a Description attribute for the class and Description attributes for its properties (except for StoredProcedure(), TableFunction() and ScalarFunction() as we've seen earlier).
So, this validation could present a problem for the following case:
Imagine you need to join two tables with a relationship of 'one to many' and you want the resulting data into one object which has, by composition, a colletion of objects for the second table.
Recall the example from LeftJoin section earlier:
``` csharp
IEnumerable<User> users = (await dataContext
    .Select<User>(x => new { x.Id, x.Nick, x.Password })
    .LeftJoin<User, Car>(x => x.Id, y => y.UserId, y => new { y.Id, y.Brand })
    .QueryAsync<User, Car, User>(mapper)).Distinct();
```
Here, we are using one of the many Query<T, T, T...>() methods Dapper offers and, of course, DataContext does too.
So, you declare both 'User' and 'Car' classes as models, where you set Description attributes on their properties. But, User class needs to have a collection property for 'Car' objects that doesn't correspond to any column from User table.
So, you declare User class like this:
``` csharp
namespace Whatever
{
    [Description("dbo.User")]
    public class User
    {
        [Description("Id")]
        public int Id { get; set; }

        [Description("Nick")]
        public string Nick { get; set; }

        [Description("[Password]")]
        public string Password { get; set; }

        public IList<Car> Cars { get; set; }
    }
}
```
So now, since DataContext validates that every property must have a Description attribute, an ArgumentException() is raised.

The solution is just as simple as putting an empty Description attribute on 'Cars' property (or any string you want):
``` csharp
namespace Whatever
{
    [Description("dbo.User")]
    public class User
    {
        [Description("Id")]
        public int Id { get; set; }

        [Description("Nick")]
        public string Nick { get; set; }

        [Description("[Password]")]
        public string Password { get; set; }

        [Description("")]
        public IList<Car> Cars { get; set; }
    }
}
```

Every validation message comes from constants variables declared at "Dappator.Internal.Constants" class. You can take a look at it to see all the validations DataContext does.

## Mocking DataContext and all of its methods

We know DataContext implements IDataContext. So, with this interface in mind, we can mock every main method (Select(), Insert(), Update(), etcetera).
But then, each of them returns an interface that makes available new methods.
So, all we need to do is to take a look at the interface a method returns to mock it.

For example:
Select() returns IQueryBuilderJoin.
This interface makes available InnerJoin(), Where(), Cast(), etcetera.
Where() returns IQueryBuilderAndOrGroupByOrderBy.
This interface makes available And(), Or(), OrderBy(), etcetera.
OrderBy() returns another interface, but also all of them implement IQueryBuilderQuery in order to have Query() method available at any time.

So, if we have the following query:
``` csharp
IEnumerable<User> users = await dataContext
    .Select<User>(x => new { x.Id, x.Nick, x.Password })
    .Where<User>(x => x.Id, Dappator.Common.Operators.GreaterThan, 23)
    .OrderBy<User>(x => x.Id, Dappator.Common.Orders.Ascending)
    .QueryAsync<User>();
```
We will need to create the following mock variables:
- One for IQueryBuilderQuery that mocks QueryAsync<User>() method and returns a collection of 'User' objects.
- One for IQueryBuilderAndOrGroupByOrderBy that mocks OrderBy<User>() method and returns the previous mock object.
- One for IQueryBuilderJoin that mocks Where<User>() method and returns the previous mock object.
- And one for IDataContext (or IMyDatabaseContext) that mocks Select<User>() method and returns the previous mock object.

You can see some examples of this in the Mock test class discussed later.

## Tests & Samples

Every test is a sample. You can see Dappator in action on them.
Tests are in 'Dappator._Provider_.Test' projects.
For every provider, tests are the same.
So, inside 'Dappator._Provider_.Test' project you can find the following test classes:
- QueryBuilder
- QueryBuilderError
- Mock
- Functional

### QueryBuilder test class

In this test class, tests are meant to verify the QueryBuilder part of DataContext.
All of them end up with GetQuery() method and assert the string built query along with the values supplied.

So, taking a look at the Assert part of each test can give you an insight about how the query is built.
Also, they can give you more examples about building such queries that have not been provided at this documentation.

### QueryBuilderError test class

In this test class, tests are meant to verify all the validations DataContext does on methods parameters.
These are examples about how to not use methods from DataContext.
Also, they can be useful when you get an ArgumentException() from your code because what you are doing is probably on one of these tests.

### Mock test class

In this test class, tests are meant to verify methods from a Data Access Layer, like in the real world, by mocking DataContext and all of its methods.

At the bottom of this class, ICustomContext interface and CustomContext class are defined.
Right above them, there is a SomeDataClass class which has some CRUD methods. All of them make use of CustomContext to do some CRUD action.
Then, every test mock whatever they need and verify the result.

This is a good starting point to begin mocking DataContext.

### Functional test class

In this test class, tests are meant to make use of Dappator in a real world scenario.
If you want to run these tests:
1. Create an empty database.
2. Configure its connectionstring at the beginning of the test class, in Setup() method.

Setup() method is in charge of creating the tables tests need.

This test class has six regions:
- Region "Insert, Update, Delete, Select": They test these four CRUD actions on the database, synchronously and asynchronously.
- Region "Select Aggregate": They test the five aggregate functions on the database, synchronously and asynchronously.
- Region "Transactions": They test CRUD actions in transaction on the database, synchronously and asynchronously, including a failure transaction for testing the Rollback() action.
- Region "Error Handling": They test an error raised from the database as an IException object, synchronously and asynchronously.
- Region "Stored Procedures": They test the execution of stored procedures on the database, synchronously and asynchronously.
- Region "Functions": They test the execution of functions on the database, synchronously and asynchronously.

This is a good starting point to begin using Dappator.