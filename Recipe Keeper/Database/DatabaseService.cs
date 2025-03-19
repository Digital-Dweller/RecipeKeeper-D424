using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Recipe_Keeper.Database;
using Recipe_Keeper.Classes.Utilities;
using Recipe_Keeper.Classes;

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
        public async Task<bool> UserLogin(string username, string pass)
        {
            dbUser targetUser = await GetUserFromUsername(username);
            return PasswordHandler.PasswordCompare(pass, targetUser.Password);
        }


        //Add a new user to the database.
        public async Task AddUser(dbUser newUser)
        {
            await dbConnection.InsertAsync(newUser);
        }

        public async Task AddRecipe(dbRecipe newRecipe)
        {
            await dbConnection.InsertAsync(newRecipe);
        }
        public async Task AddIngredient(dbIngredient newIngredient)
        {
            await dbConnection.InsertAsync(newIngredient);
        }
        public async Task AddDirection(dbDirection newDirection)
        {
            await dbConnection.InsertAsync(newDirection);
        }

        //Check if user exists.
        public async Task<bool> IsUser(string username)
        {
            var targetUser = await dbConnection.Table<dbUser>().Where(u => u.Username == username).FirstOrDefaultAsync();
            if (targetUser != null) { return true; }
            else { return false; }
        }

        public async Task<bool> IsRecipe(string recipeName, int userId)
        {
            var targetRecipe = await dbConnection.Table<dbRecipe>().Where(r => r.Title == recipeName && r.UserId == userId).FirstOrDefaultAsync();
            if (targetRecipe != null) { return true; }
            else { return false; }
        }

        public async Task<bool> IsRecipeTitle(string recipeName, int recipeId, int userId)
        {
            var targetRecipe = await dbConnection.Table<dbRecipe>().Where(r => r.Title == recipeName && r.UserId == userId && r.Id != recipeId).FirstOrDefaultAsync();
            if (targetRecipe != null) { return true; }
            else { return false; }
        }

        public async Task<dbUser> GetUserFromId(int id)
        {
            dbUser targetUser = await dbConnection.Table<dbUser>().Where(u => u.id == id).FirstOrDefaultAsync();
            return targetUser;
        }

        public async Task<dbUser> GetUserFromUsername(string username)
        {
            dbUser targetUser = await dbConnection.Table<dbUser>().Where(u => u.Username == username).FirstOrDefaultAsync();
            return targetUser;
        }

        public async Task<int> GetRecipeID(string recipeName, int userId)
        {
            var targetRecipe = await dbConnection.Table<dbRecipe>().Where(r => r.Title == recipeName && r.UserId == userId).FirstOrDefaultAsync();
            return targetRecipe?.Id ?? 0;
        }

        public async Task<dbRecipe> GetRecipe(int recipeId)
        {
            return await dbConnection.Table<dbRecipe>().Where(r => r.Id == recipeId).FirstOrDefaultAsync();
        }

        public async Task<List<Recipe>> GetRecipes(int userId)
        {
            List<Recipe> returnList = new List<Recipe>();
            List<dbRecipe> userRecipes = await dbConnection.Table<dbRecipe>().Where(r => r.UserId == userId).ToListAsync();
            if (userRecipes != null)
            {
                var recipesList = userRecipes.Select(async userRecipe => new Recipe
                {
                    Id = userRecipe.Id,
                    Title = userRecipe.Title,
                    Author = userRecipe.Author,
                    Description = userRecipe.Description,
                    Category = userRecipe.Category,
                    Favorited = userRecipe.Favorited,
                    ImagePath = userRecipe.ImagePath,
                    Ingredients = await GetIngredients(userRecipe.Id),
                    Directions = await GetDirections(userRecipe.Id),
                }).ToList();

                Recipe[] recipesArray = await Task.WhenAll(recipesList);
                returnList = recipesArray.ToList();
            }
            return returnList;
        }
        public async Task<dbIngredient> GetIngredient(int IngredientId)
        {
            return await dbConnection.Table<dbIngredient>().Where(i => i.Id == IngredientId).FirstOrDefaultAsync();
        }
        public async Task<List<dbIngredient>> GetIngredients(int recipeId)
        {
            List<dbIngredient> recipeIngredients = await dbConnection.Table<dbIngredient>().Where(i => i.RecipeId == recipeId).ToListAsync();
            return recipeIngredients;
        }
        public async Task<dbDirection> GetDirection(int DirectionId)
        {
            return await dbConnection.Table<dbDirection>().Where(d => d.Id == DirectionId).FirstOrDefaultAsync();
        }
        public async Task<List<dbDirection>> GetDirections(int recipeId)
        {
            List<dbDirection> recipeDirections = await dbConnection.Table<dbDirection>().Where(i => i.RecipeId == recipeId).ToListAsync();
            return recipeDirections;
        }

        public async Task UpdateRecipe(dbRecipe targetRecipe)
        {
            await dbConnection.UpdateAsync(targetRecipe);
        }

        public async Task UpdateUser(dbUser targetUser)
        {
            await dbConnection.UpdateAsync(targetUser);
        }
        public async Task UpdateUsername(int userId, string username)
        {
            dbUser targetUser = await GetUserFromId(userId);
            targetUser.Username = username;
            await UpdateUser(targetUser);
        }
        public async Task UpdateEmail(int userId, string email)
        {
            dbUser targetUser = await GetUserFromId(userId);
            targetUser.Email = email;
            await UpdateUser(targetUser);
        }
        public async Task UpdatePassword(int userId, string newPassHash)
        {
            dbUser targetUser = await GetUserFromId(userId);
            targetUser.Password = newPassHash;
            await UpdateUser(targetUser);
        }

        public async Task DeleteRecipe(int recipeId)
        {
            dbRecipe targetRecipe = await GetRecipe(recipeId);
            await dbConnection.DeleteAsync(targetRecipe);
        }

        public async Task DeleteIngredient(dbIngredient targetIngredient)
        {
            await dbConnection.DeleteAsync(targetIngredient);
        }

        public async Task DeleteDirection(dbDirection targetDirection)
        {
            await dbConnection.DeleteAsync(targetDirection);
        }
        public async Task DeleteUser(dbUser targetUser)
        {
            await dbConnection.DeleteAsync(targetUser);
        }


        public async Task SetRememberMe(dbUser targetUser)
        {
            targetUser.Remembered = true;
            await dbConnection.UpdateAsync(targetUser);
        }
        public async Task<dbUser> CheckRememberMe()
        {
            dbUser targetUser = await dbConnection.Table<dbUser>().Where(u => u.Remembered == true).FirstOrDefaultAsync();
            return targetUser;
        }
        public async Task RemoveRememberMe(string username)
        {
            Console.WriteLine($"Removing user:{username}");
            dbUser targetUser = await GetUserFromUsername(username);
            targetUser.Remembered = false;
            await dbConnection.UpdateAsync(targetUser);
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
        public async Task PrintAllUsers()
        {
            List<dbUser> allUsers = await dbConnection.Table<dbUser>().ToListAsync();
            if (allUsers.Count > 0)
            {
                foreach (dbUser user in allUsers) 
                { Console.WriteLine($"Username:{user.Username} Email:{user.Email} Password:{user.Password} Remembered:{user.Remembered}"); }
            }
        }
        public async Task PrintAllRecipes()
        {
            Console.WriteLine("Checking recipes");
            List<dbRecipe> allRecipes = await dbConnection.Table<dbRecipe>().ToListAsync();
            if (allRecipes.Count > 0)
            {
                foreach (dbRecipe Recipe in allRecipes)
                { Console.WriteLine($"Recipename:{Recipe.Title} RecipeId: {Recipe.Id} UserId: {Recipe.UserId}"); }
            }
        }

        public async Task PrintAllIngredients()
        {
            Console.WriteLine("Checking ingredients");
            List<dbIngredient> allIngredients = await dbConnection.Table<dbIngredient>().ToListAsync();
            if (allIngredients.Count > 0)
            {
                foreach (dbIngredient Ingredient in allIngredients)
                { Console.WriteLine($"Ingredientname:{Ingredient.Title} RecipeId: {Ingredient.RecipeId}"); }
            }
        }

    }    

}
