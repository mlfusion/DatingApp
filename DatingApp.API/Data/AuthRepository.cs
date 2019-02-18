using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Infrastructure;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        private readonly ILog _log;

        public AuthRepository(ILog log, DataContext context) : base(log, context)
        {
            _log = log;
        }

        public async Task<User> Login(string username, string password)
        {
            //var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username);

            var user = await base.SelectIncludeAsync(x => x.Username == username, "Photos");

            if (user == null)
                return null;

            if (!VerfiyPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerfiyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
              var computerhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computerhash.Length; i++)
                {
                    if (computerhash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);
        
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            //user.Created = DateTime.Now;

            await base.AddAsync(user);
            await base.SaveAync(); //_context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public async Task<bool> UserExits(string username)
        {
            var ret = await base.SelectAsync(x => x.Username == username);

            if (ret != null)
           // if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}