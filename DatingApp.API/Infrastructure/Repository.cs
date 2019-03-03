using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Common.Paging;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Infrastructure
{
    public class Repository<T> where T : class
    {
        private readonly ILog _log;
        private readonly DataContext _context;
        private readonly DbSet<T> _dbSet;
        private static int _count {get;set;}
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
            IQueryable<T> query = _dbSet;

            _count = query.Count();

            return await query.ToListAsync();
        }

        public async virtual Task<PagedList<T>> SelectAsync(Params param)
        {
            IQueryable<T> query = _dbSet;

            _count = query.Count();

            //var items = await query.Skip((param.PageNumber - 1) * param.PageSize)
            //    .Take(param.PageSize).ToListAsync();

            return await PagedList<T>.CreateAsync(query, param.PageNumber, param.PageSize);
        }

        public async virtual Task<IEnumerable<T>> SelectAsync(Expression<Func<T, bool>> filter, int status = 0)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            _count = query.Count();

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

                _count = query.Count();

            return await query.ToListAsync();
        }

        public async virtual Task<PagedList<T>> SelectIncludeAsync(Expression<Func<T, bool>> filter, Params param, int status = 0, params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (string include in includes)
                    query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

                _count = query.Count();

            //return await query.Skip((param.PageNumber - 1) * param.PageSize)
            //    .Take(param.PageSize).ToListAsync();
            return await PagedList<T>.CreateAsync(query, param.PageNumber, param.PageSize);
        }

         public async Task<DataTable> ExecuteDataTableAsync(string commandName, CommandType cmdType, SqlParameter[] sqlParameter)
        {
            DataTable table = null;
            using (_context)
            {
                var cc = _context.Database.GetDbConnection().ConnectionString;//.Connection.ConnectionString;

                using (SqlConnection con = new SqlConnection(cc))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = cmdType;
                        cmd.CommandText = commandName;

                        if (sqlParameter != null)
                        {
                            cmd.Parameters.AddRange(sqlParameter);
                        }

                        try
                        {
                            if (con.State != ConnectionState.Open)
                            {
                                con.Open();
                            }

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                table = new DataTable();
                                da.Fill(table);
                            }
                        }
                        catch (Exception ex)
                        {
                           _log.Write(ex);

                            throw;
                        }
                    }
                }

            }

            await Task.Yield();

            return table;
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
            return _count; // _context.Set<T>().Count();
        }

        // public int Count()
        // {
        //     List<T> obj = new List<T>();
        //     foreach (T i in t)
        //         obj.Add(i);

        //     return obj.Count;    //_context..Set<T>().Count();
        // }

        // public async Task<int> CountAsync(IEnumerable<T> t)
        // {
        //    IQueryable<T> query = _dbSet;
           
        //    return await query.CountAsync();    //_context..Set<T>().Count();
        // }

        // public async Task<int> CountAsync()
        // {
        //        return await _count; // _context.Set<T>().CountAsync();
        // }
    }
}