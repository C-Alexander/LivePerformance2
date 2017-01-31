using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LivePerformance2.Contexts;
using LivePerformance2.Controllers;
using LivePerformance2.Repositories;
using LivePerformance2.Services;

namespace Tests
{
    public static class TestHelper
    {

        public static UserController UserControllerFactory()
        {
            return new UserController(DatabaseServiceFactory(),
                UserRepositoryFactory(), 
                LoginServiceFactory());
        }

        public static ILoginService LoginServiceFactory()
        {
            return new LoginService(UserRepositoryFactory());
        }

        public static IUserRepository UserRepositoryFactory()
        {
            return new UserRepository(UserContextFactory());
        }

        public static IUserContext UserContextFactory()
        {
            return new UserSQLContext(DatabaseServiceFactory()); //should be memory later
        }

        public static IDatabaseService DatabaseServiceFactory()
        {
            return new DatabaseService();
        }
    }
}
