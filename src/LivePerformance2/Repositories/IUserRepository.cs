using System.Collections.Generic;
using LivePerformance2.Models;

namespace LivePerformance2.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        void Delete(User user);
        void Update(User user);
        ICollection<User> Read();
        int Login(User user);
        int Login(string username, string password);
        bool UserNameExists(string username);
    }
}
