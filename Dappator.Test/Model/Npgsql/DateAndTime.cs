namespace Dappator.Test.Model.Npgsql
{
    [System.ComponentModel.Description("dateandtime")]
    public class DateAndTime
    {
        [System.ComponentModel.Description("id")]
        public int Id { get; set; }

        [System.ComponentModel.Description("\"datetime\"")]
        public DateTime DateTime { get; set; }

        [System.ComponentModel.Description("dateonly")]
        public DateTime DateOnly { get; set; }

        [System.ComponentModel.Description("timeonly")]
        public DateTime TimeOnly { get; set; }
    }
}
