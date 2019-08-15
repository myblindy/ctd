using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Users.Controllers;
using Services.Users.DBModel;
using Services.Users.Model;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class UserTests
    {
        private async Task<UsersContext> BuildUsersContext()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            await conn.OpenAsync();

            var options = new DbContextOptionsBuilder<UsersContext>()
                .UseSqlite(conn)
                .Options;

            var context = new UsersContext(options);

            await context.Database.EnsureCreatedAsync();

            // clear
            context.Users.RemoveRange(context.Users);
            context.Tokens.RemoveRange(context.Tokens);

            // dummy users
            await context.Users.AddRangeAsync(new[]
            {
                new User { UserId = 1, Username = "uname1", Password = "pword1" },
                new User { UserId = 2, Username = "uname3", Password = "pword3" },
            });
            await context.SaveChangesAsync();

            return context;
        }

        [TestMethod]
        public async Task LoginFail()
        {
            var usersContext = await BuildUsersContext();
            var authController = new AuthenticationController(usersContext);

            var result = await authController.Login(
                new AuthenticationLoginModel { Username = "uname", Password = "pword" });
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            result = await authController.Login(
                new AuthenticationLoginModel { Username = "uname2", Password = "pword2" });
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            result = await authController.Login(
                new AuthenticationLoginModel { Username = "una123312me", Password = "123asrdq" });
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task LoginSuccess()
        {
            using var usersContext = await BuildUsersContext();
            var authController = new AuthenticationController(usersContext);

            // log in
            dynamic result = await authController.Login(
                new AuthenticationLoginModel { Username = "uname1", Password = "pword1" });
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(result.StatusCode, 200);

            // get the token
            var token = (ulong)Support.Get(result.Value, "token");

            // check the token
            result = await authController.Check(token);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(result.StatusCode, 200);

            // get the username from the token
            var username = Support.Get(result.Value, "username");
            Assert.AreEqual(username, "uname1");
        }
    }
}
