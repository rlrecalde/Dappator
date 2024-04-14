Dappator.Oracle NuGet Package 1.0.1.0 README
============================================

April 2024

This package supports .NET Framework 4.6.1 and above, .NET Standard 2.0 and above, .NET 5, .NET 6, .NET 7 and .NET 8.


Release Notes
=============

Implemented Dapper version 2.1.44.
Main change: Now it is ready for working with 'DateOnly' and 'TimeOnly' but, 
             since Oracle does not allow 'Date' and 'Time' columns, it is not possible for now.


Dependencies:
============

- Dappator 1.0.1.0
- Dapper 2.1.44
- Oracle.ManagedDataAccess.Core 3.21.130 (for every version of .NET, except for .NET Framework 4 and .NET Standard)
- Oracle.ManagedDataAccess.Core 2.19.180 (for .NET Framework 4 and .NET Standard)


Links:
=====

For documentation: https://www.nuget.org/packages/Dappator.Oracle/
For Tests & Samples: https://github.com/rlrecalde/Dappator#tests--samples
