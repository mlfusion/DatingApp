using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Infrastructure
{
    public abstract class Repository<T> where T : class
    {
        private readonly ILog _log;
        private readonly DataContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ILog log, DataContext context)
        {
            _log = log;
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async virtual Task<T> SelectAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async virtual Task<T> SelectAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async virtual Task<T> SelectIncludeAsync(Expression<Func<T, bool>> filter, params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            foreach (string include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async virtual Task<IEnumerable<T>> SelectAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async virtual Task<IEnumerable<T>> SelectAsync(Expression<Func<T, bool>> filter, int status = 0)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async virtual Task<IEnumerable<T>> SelectIncludeAsync(Expression<Func<T, bool>> filter, int status = 0, params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (string include in includes)
                    query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public virtual void Add(T entity)
        {
            IQueryable<T> query = _dbSet;

            _dbSet.Add(entity);
        }

        public async virtual Task AddAsync(T entity)
        {
            IQueryable<T> query = _dbSet;

            _dbSet.Add(entity);

            await Task.Yield();
        }

        public virtual void Update(T entity)
        {
            IQueryable<T> query = _dbSet;
            
            _dbSet.Update(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            IQueryable<T> query = _dbSet;
            
            _dbSet.Update(entity);

            await Task.Yield();
        }


        public void Delete(T t)
        {
            _context.Set<T>().Remove(t);
           
        }

        public async Task DeleteAsync(T t)
        {
            _context.Set<T>().Remove(t);
            await Task.Yield(); // _dbSet.AnyAsync();
        }

        public bool Save()
        {
           return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAync()
        {
           return await _context.SaveChangesAsync() > 0;
        }
        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public int Count(IEnumerable<T> t)
        {
            List<T> obj = new List<T>();
            foreach (T i in t)
                obj.Add(i);

            return obj.Count;    //_context..Set<T>().Count();
        }

        public async Task<int> CountAsync(IEnumerable<T> t)
        {
            List<T> obj = new List<T>();
            foreach (T i in t)
                obj.Add(i);

            await Task.Yield();
            return obj.Count;    //_context..Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
               return await _context.Set<T>().CountAsync();
        }
    }
}