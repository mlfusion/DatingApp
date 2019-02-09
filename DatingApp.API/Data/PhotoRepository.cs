using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;

        public PhotoRepository(DataContext context)
    {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
        _context.Remove(entity);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var obj = await _context.Photos.Include(u => u.User)
                    .FirstOrDefaultAsync(x => x.Id == id);

            return obj;
        }

        public async Task<Photo> GetPhoto(Expression<Func<Photo, bool>> where)
        {
            return await _context.Photos.FirstOrDefaultAsync(where);
        }

        public async Task<IEnumerable<Photo>> GetPhotos()
        {
            var obj = await _context.Photos.ToListAsync(); 
            
            return obj;
        }

        public async Task<bool> SaveAll()
        {
           return await _context.SaveChangesAsync() > 0;
        }
    }
}