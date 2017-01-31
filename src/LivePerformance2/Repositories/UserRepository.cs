using System.Collections.Generic;
using System.Linq;
using LivePerformance2.Contexts;
using LivePerformance2.Models;

namespace LivePerformance2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IUserContext _context;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public UserRepository(IUserContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Add(user);
        }

        public void Delete(User user)
        {
            _context.Delete(user);
        }

        public void Update(User user)
        {
            _context.Update(user);
        }

        public ICollection<User> Read()
        {
            return _context.Read();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Userid of logged in user or fitting exception</returns>
        public int Login(User user)
        {
            return Login(user.Username, user.Password);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Userid of logged in user or fitting exception</returns>
        public int Login(string username, string password)
        {
            return _context.Login(username, password);
        }

        public bool UserNameExists(string username)
        {
            return _context.UserNameExists(username);
        }
    }
}
