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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public DatabaseService()
        { }

        //Add a new user to the database.
        public async Task AddUser(dbUser newUser)
        {
            await dbConnection.InsertAsync(newUser);
        }

        //Check if user exists.
        public async Task<bool> IsUser(string username)
        {
            dbUser targetUser = await dbConnection.Table<dbUser>().Where(u => u.Username == username).FirstOrDefaultAsync();
            if (targetUser == null) { return false; }
            else { return true; }

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
        //Check if the database is empty.
        public async Task<bool> Database_isEmptyAsync()
        {
            Console.WriteLine("Checking if database is empty");
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
