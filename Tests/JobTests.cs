using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Controllers;
using Services.DBModel;
using Services.Users.Controllers;
using Services.Users.DBModel;
using Services.Users.Model;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class JobTests
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

        private async Task<JobsContext> BuildJobsContext()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            await conn.OpenAsync();

            var options = new DbContextOptionsBuilder<JobsContext>()
                .UseSqlite(conn)
                .Options;

            var context = new JobsContext(options);

            await context.Database.EnsureCreatedAsync();

            // clear
            context.Jobs.RemoveRange(context.Jobs);

            // dummy jobs
            await context.Jobs.AddRangeAsync(new[]
            {
                new Job { JobId = 1, Name = "job 1" },
                new Job { JobId = 2, Name = "job 2" },
                new Job { JobId = 3, Name = "job 3" },
            });
            await context.SaveChangesAsync();

            return context;
        }

        [TestMethod]
        public async Task ListJobs()
        {
            using var usersContext = await BuildUsersContext();
            var authController = new AuthenticationController(usersContext);

            using var jobsContext = await BuildJobsContext();
            var jobsController = new JobAidController(jobsContext, authController);

            // log in
            dynamic result = await authController.Login(
                new AuthenticationLoginModel { Username = "uname1", Password = "pword1" });
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(result.StatusCode, 200);

            // get the token
            var token = (ulong)Support.Get(result.Value, "token");

            // get the list of jobs
            result = await jobsController.ListJobs(token);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(result.StatusCode, 200);

            // test the username
            var username = (string)Support.Get(result.Value, "username");
            Assert.AreEqual(username, "uname1");

            // test the array of jobs
            dynamic jobs = Support.Get(result.Value, "jobs");
            foreach (var job in jobs)
            {
                var jobId = (int)Support.Get(job, "JobId");
                var name = (string)Support.Get(job, "Name");
                Assert.IsNotNull(name);
            }
        }

        [TestMethod]
        public async Task UnauthorizedListAttempt()
        {
            using var usersContext = await BuildUsersContext();
            var authController = new AuthenticationController(usersContext);

            using var jobsContext = await BuildJobsContext();
            var jobsController = new JobAidController(jobsContext, authController);

            // get the list of jobs using a random, unexisting token
            var result = await jobsController.ListJobs(239048923);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }
    }
}
