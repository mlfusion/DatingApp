using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DatingApp.API.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> SelectAsync();
        Task<T> SelectAsync(int id);
        Task<T> SelectAsync(Expression<Func<T, bool>> filter);
        Task<T> SelectIncludeAsync(Expression<Func<T, bool>> filter, params string[] includes);
        Task<IEnumerable<T>> SelectIncludeAsync(Expression<Func<T, bool>> filter, int status = 0, params string[] includes);
        void Add(T entity);
        Task AddAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Delete(T t);
        Task DeleteAsync(T t);
        bool Save();
        Task<bool> SaveAync();
        int Count();
        int Count(IEnumerable<T> t);
        Task<int> CountAsync();
        Task<int> CountAsync(IEnumerable<T> t);
        
    
    }
}