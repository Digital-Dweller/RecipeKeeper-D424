using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Recipe_Keeper.Database
{
    public class dbUser
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        private string username;
        private string password;
        private string email;
        public dbUser(){}
        public dbUser(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
        //Getters and setters with encapsulation support.
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
    }
}
