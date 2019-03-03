using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Common.Paging;
using DatingApp.API.Infrastructure;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : Repository<User>,  IDatingRepository
    {
        private readonly ILog _log;
        private readonly DataContext _context;

        public DatingRepository(ILog log, DataContext context) :  base(log, context)
        {
            _log = log;
            _context = context;
        }

        public async Task<User> GetUser(int id)
        {
           // var user = await _context.Users.Include(p => p.Photos)
           //     .FirstOrDefaultAsync(u => u.Id == id);
            using(_log.BeginScope())
            {
                _log.Write($"Retrieving user id {id}.");
                var user = await base.SelectIncludeAsync(x => x.Id == id, "Photos");
                return user;
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            using(_log.BeginScope())
            {
                // var users = await _context.Users.Include(p => p.Photos).ToListAsync(); 
                _log.Write("Retrieving all users.");
                var users = await SelectIncludeAsync(null, 0, "Photos");
                _log.Write($"{Count()} users was found.");
                    
                return users;
            }
        }

        public Task<PagedList<User>> SelectAsync(Expression<Func<User, bool>> filter, Params param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> SelectIncludeAsync(Expression<Func<User, bool>> filter, Params param, int i = 0)
        {
            throw new NotImplementedException();
        }
    }
}