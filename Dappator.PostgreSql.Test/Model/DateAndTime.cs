using System;

namespace Dappator.PostgreSql.Test.Model
{
    [System.ComponentModel.Description("dateandtime")]
    public class DateAndTime
    {
        [System.ComponentModel.Description("id")]
        public int Id { get; set; }

        [System.ComponentModel.Description("\"datetime\"")]
        public DateTime DateTime { get; set; }

#if NET6_0_OR_GREATER
        [System.ComponentModel.Description("dateonly")]
        public DateOnly DateOnly { get; set; }

        [System.ComponentModel.Description("timeonly")]
        public TimeOnly TimeOnly { get; set; }
#endif
    }
}
