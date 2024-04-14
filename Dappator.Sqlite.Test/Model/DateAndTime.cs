using System;

namespace Dappator.Sqlite.Test.Model
{
    [System.ComponentModel.Description("[DateAndTime]")]
    public class DateAndTime
    {
        [System.ComponentModel.Description("[Id]")]
        public int Id { get; set; }

        [System.ComponentModel.Description("[DateTime]")]
        public DateTime DateTime { get; set; }

#if NET6_0_OR_GREATER
        [System.ComponentModel.Description("[DateOnly]")]
        public DateOnly DateOnly { get; set; }

        [System.ComponentModel.Description("[TimeOnly]")]
        public TimeOnly TimeOnly { get; set; }
#endif
    }
}
