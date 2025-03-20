using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Recipe_Keeper.Classes.Utilities
{
    class EmailValidation
    {
        static string EmailVal_Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public static bool isValid(string inputEmail)

        { return Regex.IsMatch(inputEmail, EmailVal_Pattern); }
    }
}
