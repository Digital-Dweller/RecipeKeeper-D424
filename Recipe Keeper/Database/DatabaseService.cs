using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Recipe_Keeper.Database
{
    internal class DatabaseService
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

    }
}
