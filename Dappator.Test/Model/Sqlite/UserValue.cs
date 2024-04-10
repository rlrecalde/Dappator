namespace Dappator.Test.Model.Firebirdite
{
    [System.ComponentModel.Description("[UserValue]")]
    public class UserValue : IUserValue
    {
        [System.ComponentModel.Description("[Id]")]
        public int Id { get; set; }

        [System.ComponentModel.Description("[UserId]")]
        public int UserId { get; set; }

        [System.ComponentModel.Description("[Value]")]
        public double Value { get; set; }
    }
}
