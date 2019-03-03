using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Common.Paging;
using DatingApp.API.Infrastructure;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data 
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        private readonly ILog _log;
        private readonly DataContext _context;

        public PhotoRepository(ILog log, DataContext context) : base(log, context)
    {
            _log = log;
            _context = context;
        }
        public new void Add (Photo entity)
        {
            base.Add(entity);
        }

        public new void Delete(Photo entity)
        {
           base.Delete(entity);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var obj = await base.SelectIncludeAsync(x => x.Id == id, "User");
            // _context.Photos.Include(u => u.User)
            //        .FirstOrDefaultAsync();
            return obj;
        }

        public async Task<Photo> GetPhoto(Expression<Func<Photo, bool>> where)
        {
            return await _context.Photos.FirstOrDefaultAsync(where);
        }

        public async Task<Photo> GetPhoto(Expression<Func<Photo, bool>> filter = null, 
                        Func<IQueryable<Photo>, IOrderedQueryable<Photo>> orderBy = null, 
                        params Expression<Func<Photo, object>>[] includes)
        {
            IQueryable<Photo> query = _context.Set<Photo>();
    
            foreach (Expression<Func<Photo, object>> include in includes)
                query = query.Include(include);
    
            if (filter != null)
                query = query.Where(filter);
    
            if (orderBy != null)
                query = orderBy(query);
    
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Photo>> GetPhotos()
        {
            var obj = await _context.Photos.ToListAsync(); 
            
            return obj;
        }

   public async Task<IEnumerable<Photo>> GetPhotos(Expression<Func<Photo, bool>> filter = null, 
                    Func<IQueryable<Photo>, IOrderedQueryable<Photo>> orderBy = null, 
                    params Expression<Func<Photo, object>>[] includes)
    {
        IQueryable<Photo> query = _context.Set<Photo>();
 
        foreach (Expression<Func<Photo, object>> include in includes)
            query = query.Include(include);
 
        if (filter != null)
            query = query.Where(filter);
 
        if (orderBy != null)
            query = orderBy(query);
 
        return await query.ToListAsync();
    }

        public async Task<bool> SaveAll()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public Task<PagedList<Photo>> SelectAsync(Expression<Func<Photo, bool>> filter, Params param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Photo>> SelectIncludeAsync(Expression<Func<Photo, bool>> filter, Params param, int i = 0)
        {
            throw new NotImplementedException();
        }
    }
}