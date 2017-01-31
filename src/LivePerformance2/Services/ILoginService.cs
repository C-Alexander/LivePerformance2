using LivePerformance2.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace LivePerformance2.Services
{
    public interface ILoginService
    {
        bool IsLoggedIn(ISession session);
        void Logout(ISession session);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="user"></param>
        /// <returns>Wether the login was successful</returns>
        bool Login(ISession session, string username, string password);

    }
}