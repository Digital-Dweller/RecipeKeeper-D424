using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Database
{
    public class DatabaseService
    {
        //The SQLite connection object.
        private readonly SQLiteAsyncConnection dbConnection;

        public DatabaseService(string dbPath)
        {
            try
            {
                //Create the SQLite connection object with access to read and write to the database.
                dbConnection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                //Enable foreign key relationships on the database.
                dbConnection.ExecuteAsync("PRAGMA foreign_keys = ON;").Wait();

                if (dbConnection == null)
                {
                    throw new InvalidOperationException("Failed to initialize the database connection.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public DatabaseService()
        { }

        //Try to login with the provided credentials.
        public async Task<bool> UserLogin(string username, string passHash)
        {
            dbUser targetUser = await GetUserFromUsername(username);
            Console.WriteLine($"Comparing password hashes input:{passHash} target:{targetUser.Password}");
            return passHash == targetUser.Password;
        }


        //Add a new user to the database.
        public async Task AddUser(dbUser newUser)
        {
            await dbConnection.InsertAsync(newUser);
        }

        //Check if user exists.
        public async Task<bool> IsUser(string username)
        {
            var targetUser = await dbConnection.Table<dbUser>().Where(u => u.Username == username).FirstOrDefaultAsync();
            if (targetUser != null) { return true; }
            else { return false; }
        }

        public async Task<dbUser> GetUserFromUsername(string username)
        {
            dbUser targetUser = await dbConnection.Table<dbUser>().Where(u => u.Username == username).FirstOrDefaultAsync();
            return targetUser;
        }

        //Create the default tables in the SQLite database.
        public async Task CreateDefaultTables()
        {
            await dbConnection.CreateTableAsync<dbDirection>();
            await dbConnection.CreateTableAsync<dbIngredient>();
            await dbConnection.CreateTableAsync<dbRecipe>();
            await dbConnection.CreateTableAsync<dbUnit>();
            await dbConnection.CreateTableAsync<dbUser>();
        }

        //Check if there are any users in the database.
        public async Task<bool> dbUser_isEmptyAsync()
        {
            try
            {
                var userCount = await dbConnection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM dbUser;");
                Console.WriteLine( "Usercount is:" + userCount );
                if (userCount == 0)
                { return true; }
                else { return false; }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Check if the database is empty.
        public async Task<bool> Database_isEmptyAsync()
        {
            try
            {
                var tableCount = await dbConnection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='dbDirection';");
                tableCount += await dbConnection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='dbIngredient';");
                tableCount += await dbConnection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='dbRecipe';");
                tableCount += await dbConnection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='dbUnit';");
                tableCount += await dbConnection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='dbUser';");

                return tableCount == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }    

}
