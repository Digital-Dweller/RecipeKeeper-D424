using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Classes
{
    public class UserSession
    {
        public int id { get; private set; }
        public string username { get; private set; }
        public bool IsLoggedIn { get; private set; }
        private readonly DatabaseService databaseService;
        public UserSession(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }
        public async Task Login(dbUser user)
        {
            this.id = user.id;
            this.username = user.Username;
            IsLoggedIn = true;
        }
        public async Task Logout()
        {
            Console.WriteLine($"Calling RemoveRemember me with username:{username}");
            await databaseService.RemoveRememberMe(username);
            this.id = 0;
            this.username = string.Empty;
            IsLoggedIn = false;

        }
    }
}
