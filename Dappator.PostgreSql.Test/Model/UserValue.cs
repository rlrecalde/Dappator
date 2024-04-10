namespace Dappator.PostgreSql.Test.Model
{
    [System.ComponentModel.Description("uservalue")]
    public class UserValue
    {
        [System.ComponentModel.Description("id")]
        public int Id { get; set; }

        [System.ComponentModel.Description("userid")]
        public int UserId { get; set; }

        [System.ComponentModel.Description("\"value\"")]
        public double Value { get; set; }
    }
}
