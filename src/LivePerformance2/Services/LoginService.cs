using System;
using System.Data.SqlClient;
using LivePerformance2.Enumerations;
using LivePerformance2.Repositories;
using LivePerformance2.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace LivePerformance2.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _userRepo;


        public LoginService(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }

        public bool IsLoggedIn(ISession session)
        {
            if (session != null && session.GetString(ContextData.UserId.ToString()) != null)
            {
                return true;
            }
            return false;
        }

        public void Logout(ISession session)
        {
            session.LoadAsync();
            session.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextSession"></param>
        /// <param name="user"></param>
        /// <returns>Wether the login was successful</returns>
        public bool Login(ISession httpContextSession, string username, string password)
        {
            try
            {
                var id = _userRepo.Login(username, password);
                httpContextSession.SetInt32(ContextData.UserId.ToString(), id);
                return true;
            }
            catch (InvalidOperationException o)
            {
                Log.Error("No connection");
                return false;
            }
        }

        //public bool IsEmployee(ISession session)
        //{
        //    if (IsLoggedIn(session))
        //    {
        //        return _userRepo.Read(session.GetInt32(ContextData.UserId.ToString()).GetValueOrDefault()).IsEmployee;
        //    }

        //    return false;
        //}
    }
}