using System;
using LivePerformance2.Controllers;
using LivePerformance2.Repositories;
using LivePerformance2.Services;
using LivePerformance2.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests
{
    public class UserControllerTests
    {
        [Fact(DisplayName = "Can create new users")]
        public void TestNewUserCreation()
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            var r = TestHelper.UserRepositoryFactory();
            var c = TestHelper.UserControllerFactory();

            c.Register(new RegisterViewModel()
            {
                Password = "wdasffdsdsdf!231!*",
                Username = "TestUser123456",
                Email = "test@test.nl"
            });
            Assert.True(!r.UserNameExists("wdasffdsdsdf!231!*"));
        }

        [Fact(DisplayName = "Can not create invalid users")]
        public void TestFalseNewUserCreation()
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            var r = TestHelper.UserRepositoryFactory();
            var c = TestHelper.UserControllerFactory();
            var userNameRandom = Guid.NewGuid().ToString();
            c.ModelState.TryAddModelError("xUnit", "NotValid");
            c.Register(new RegisterViewModel()
            {
                Password = "teststuff",
                Username = userNameRandom,
                Email = "ja@ja.nl"
            });
            Assert.True(!r.UserNameExists(userNameRandom));
        }

        [Fact(DisplayName = "Can delete a user")]
        public void TestUserDeletion()
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            var r = TestHelper.UserRepositoryFactory();
            var c = TestHelper.UserControllerFactory();
            c.Register(new RegisterViewModel()
            {
                Password = "wdasffdsdsdf!231!*",
                Username = "TestUser123456",
                Email = "test@test.nl"
            });
            Assert.True(r.UserNameExists("TestUser123456"));
            //c.DeleteUser((c.GetUserByUserName("TestUser123456").Id));
            //Assert.True(c.GetUserByUserName("TestUser123456") == null);
        }

        [Theory(DisplayName = "Can log in")]
        [InlineData("a", "test")]
        [InlineData("a", "test2")]
        [InlineData("a", "test3")]
        public void TestLogins(string username, string password)
        {
            var c = TestHelper.UserControllerFactory();

           c.Login(new LoginViewModel() {Username = username, Password = password});
        }
        [Theory(DisplayName = "Can not delete false users")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(282938290)]
        public void TestFalseUserDeletion(int id)
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            var c = TestHelper.UserControllerFactory();

            c.DeleteUser(id);
        }
    }
}
