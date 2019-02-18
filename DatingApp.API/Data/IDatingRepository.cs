using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Infrastructure;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
    }
}