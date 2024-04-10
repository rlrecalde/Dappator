namespace Dappator.Test.Model.MySql
{
    [System.ComponentModel.Description("DateAndTime")]
    public class DateAndTime
    {
        [System.ComponentModel.Description("Id")]
        public int Id { get; set; }

        [System.ComponentModel.Description("`DateTime`")]
        public DateTime DateTime { get; set; }

        [System.ComponentModel.Description("DateOnly")]
        public DateTime DateOnly { get; set; }

        [System.ComponentModel.Description("TimeOnly")]
        public DateTime TimeOnly { get; set; }
    }
}
