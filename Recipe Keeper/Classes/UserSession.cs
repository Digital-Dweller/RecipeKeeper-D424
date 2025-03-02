using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Classes
{
    internal class UserSession
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public bool IsLoggedIn { get; set; }
        private DatabaseService databaseService { get; set; }
        public UserSession(bool IsLoggedIn, DatabaseService databaseService)
        {
            this.IsLoggedIn = IsLoggedIn;
            this.databaseService = databaseService;
        }
    }
}
