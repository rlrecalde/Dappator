﻿namespace Dappator.Sql.Test.Model
{
    [System.ComponentModel.Description("[User]")]
    public class User
    {
        [System.ComponentModel.Description("[Id]")]
        public int Id { get; set; }

        [System.ComponentModel.Description("[Nick]")]
        public string Nick { get; set; }

        [System.ComponentModel.Description("[Password]")]
        public string Password { get; set; }
    }
}
