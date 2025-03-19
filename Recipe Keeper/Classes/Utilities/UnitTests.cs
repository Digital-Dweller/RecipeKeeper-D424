using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe_Keeper.Database;
using SQLite;

namespace Recipe_Keeper.Classes.Utilities
{
    public class UnitTests
    {
        private DatabaseService databaseService;

        public UnitTests(DatabaseService dbService)
        {
            databaseService = dbService;
        }

        //Test the DatabaseService AddUser method.
        public async Task TestAddUser()
        {
            var newUser = new dbUser { Username = "TestUser", Email = "test@example.com", Password = "TestPassword" };
            await databaseService.AddUser(newUser);
            var userExists = await databaseService.IsUser(newUser.Username);
            Console.WriteLine(userExists ? "TestAddUser Passed" : "TestAddUser Failed");
            var createdUser = await databaseService.GetUserFromUsername(newUser.Username);
            if (createdUser != null)
            { await databaseService.DeleteUser(createdUser); }
        }

        //Test the DatabaseService AddRecipe method.
        public async Task TestAddRecipe()
        {
            var newRecipe = new dbRecipe { Title = "Test Recipe", UserId = 1 };
            await databaseService.AddRecipe(newRecipe);
            var recipeExists = await databaseService.IsRecipe(newRecipe.Title, newRecipe.UserId);
            Console.WriteLine(recipeExists ? "TestAddRecipe Passed" : "TestAddRecipe Failed");
            var createdRecipe = await databaseService.GetRecipeID(newRecipe.Title, newRecipe.UserId);
            if (createdRecipe != 0)
            { await databaseService.DeleteRecipe(createdRecipe); }
        }

        //Test the DatabaseService UpdateUser method.
        public async Task TestUpdateUser()
        {
            var newUser = new dbUser { Username = "TempUser", Email = "temp@example.com", Password = "TempPassword" };
            await databaseService.AddUser(newUser);
            var createdUser = await databaseService.GetUserFromUsername(newUser.Username);
            if (createdUser != null)
            {
                createdUser.Email = "updated@example.com";
                await databaseService.UpdateUser(createdUser);
                var updatedUser = await databaseService.GetUserFromId(createdUser.id);
                Console.WriteLine(updatedUser.Email == "updated@example.com" ? "TestUpdateUser Passed" : "TestUpdateUser Failed");
                await databaseService.DeleteUser(createdUser); 
            }
        }

        //Test the DatabaseService DeleteRecipe method.
        public async Task TestDeleteRecipe()
        {
            var newRecipe = new dbRecipe { Title = "Recipe to Delete", UserId = 1 };
            await databaseService.AddRecipe(newRecipe);
            var createdRecipeId = await databaseService.GetRecipeID(newRecipe.Title, newRecipe.UserId);
            if (createdRecipeId != 0)
            {
                await databaseService.DeleteRecipe(createdRecipeId);
                var deletedRecipe = await databaseService.GetRecipe(createdRecipeId);
                Console.WriteLine(deletedRecipe == null ? "TestDeleteRecipe Passed" : "TestDeleteRecipe Failed");
            }
        }

        //Run all unit tests.
        public async Task RunAllTests()
        {
            await TestAddUser();
            await TestAddRecipe();
            await TestUpdateUser();
            await TestDeleteRecipe();
        }
    }
}
