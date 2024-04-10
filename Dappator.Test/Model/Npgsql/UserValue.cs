namespace Dappator.Test.Model.Npgsql
{
    [System.ComponentModel.Description("uservalue")]
    public class UserValue : IUserValue
    {
        [System.ComponentModel.Description("id")]
        public int Id { get; set; }

        [System.ComponentModel.Description("userid")]
        public int UserId { get; set; }

        [System.ComponentModel.Description("\"value\"")]
        public double Value { get; set; }
    }
}
