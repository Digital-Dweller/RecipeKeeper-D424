using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt;

namespace Recipe_Keeper.Classes.Utilities
{
    public class PasswordHandler
    {
        //Return a hash value of the password.
        public static string HashPassword(string password)
        { return BCrypt.Net.BCrypt.HashPassword(password); }

        //Check if the provided password matches the password hashed.
        public static bool PasswordCompare(string password, string password_hash)
        { return BCrypt.Net.BCrypt.Verify(password, password_hash); }
    }
}
