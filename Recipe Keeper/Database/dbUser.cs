using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Recipe_Keeper.Database
{
    internal class dbUser
    {
        [PrimaryKey, AutoIncrement]
        required public int Id { get; set; }
        required public string Username { get; set; }
        required public string Password { get; set; }
        required public string Email { get; set; }
    }
}
