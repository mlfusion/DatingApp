using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Common.Paging;

namespace DatingApp.API.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> SelectAsync();
        Task<IEnumerable<T>> SelectAsync(Expression<Func<T, bool>> filter, int status = 0);
        Task<T> SelectAsync(int id);
        Task<T> SelectAsync(Expression<Func<T, bool>> filter);
        Task<PagedList<T>> SelectAsync(Params param);
        Task<T> SelectIncludeAsync(Expression<Func<T, bool>> filter, params string[] includes);
        Task<IEnumerable<T>> SelectIncludeAsync(Expression<Func<T, bool>> filter, int status = 0, params string[] includes);
        Task<PagedList<T>> SelectIncludeAsync(Expression<Func<T, bool>> filter, Params param, int status = 0, params string[] includes);
        Task<DataTable> ExecuteDataTableAsync(string commandName, CommandType cmdType, SqlParameter[] sqlParameter);
        void Add(T entity);
        Task AddAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Delete(T t);
        Task DeleteAsync(T t);
        bool Save();
        Task<bool> SaveAync();
        int Count();
        // int Count(IEnumerable<T> t);
        // Task<int> CountAsync();
        // Task<int> CountAsync(IEnumerable<T> t);
        
    
    }
}