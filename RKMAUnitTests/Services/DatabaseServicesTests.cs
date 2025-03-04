using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Recipe_Keeper.Database;
using SQLite;

namespace RKMAUnitTests.Services
{
    public class DatabaseServicesTests
    {
        private readonly Mock<ISQLiteAsyncConnection> mockDatabaseConnection;
        private readonly Mock<AsyncTableQuery<dbUser>> mockTableQuery;
        private readonly DatabaseService databaseService;

        public DatabaseServicesTests() 
        {
            //Mock setup of DatabaseServices dependencies. 
            mockDatabaseConnection = new Mock<ISQLiteAsyncConnection>();
            mockTableQuery = new Mock<AsyncTableQuery<dbUser>>();
            databaseService = new DatabaseService();          
        }

        //Test to confirm the GetUserFromUsername method correctly returns the expected dbUser object.
        [Fact]
        public async Task GetUserFromUsername_return()
        {
            string username_test = "testusername";
            var user_test = new dbUser(username_test, "test@test.com", "hashedPass");
            var mockQuery = new Mock<AsyncTableQuery<dbUser>>();

            //Mock the database connection.
            mockDatabaseConnection.Setup(conn => conn.Table<dbUser>()).Returns(mockTableQuery.Object);

            //Mocks the where check in the LINQ function.
            mockTableQuery.Setup(q => q.Where(It.IsAny<Expression<Func<dbUser, bool>>>())).Returns(mockTableQuery.Object);

            //Mocks the FirstOrDefaultAsync condition in the LINQ function.
            mockTableQuery.Setup(q => q.FirstOrDefaultAsync()).ReturnsAsync(user_test);

            var user_return = await databaseService.GetUserFromUsername(username_test);

            Assert.NotNull(user_return);
            Assert.Equal(user_test.Username, user_return.Username);
        }
    }
}
