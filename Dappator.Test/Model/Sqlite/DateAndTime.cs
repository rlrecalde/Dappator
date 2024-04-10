namespace Dappator.Test.Model.Firebirdite
{
    [System.ComponentModel.Description("[DateAndTime]")]
    public class DateAndTime
    {
        [System.ComponentModel.Description("[Id]")]
        public int Id { get; set; }

        [System.ComponentModel.Description("[DateTime]")]
        public string DateTime { get; set; }

        [System.ComponentModel.Description("[DateOnly]")]
        public string DateOnly { get; set; }

        [System.ComponentModel.Description("[TimeOnly]")]
        public string TimeOnly { get; set; }
    }
}
