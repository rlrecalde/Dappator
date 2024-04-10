namespace Dappator.Test.Model.Npgsql
{
    [System.ComponentModel.Description("\"user\"")]
    public class User : IUser
    {
        [System.ComponentModel.Description("id")]
        public int Id { get; set; }

        [System.ComponentModel.Description("nick")]
        public string Nick { get; set; }

        [System.ComponentModel.Description("\"password\"")]
        public string Password { get; set; }
    }
}
