using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Infrastructure;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task<IEnumerable<Photo>> GetPhotos();
        Task<IEnumerable<Photo>> GetPhotos(Expression<Func<Photo, bool>> filter = null, 
                                            Func<IQueryable<Photo>, IOrderedQueryable<Photo>> orderBy = null, 
                                            params Expression<Func<Photo, object>>[] includes);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetPhoto(Expression<Func<Photo, bool>> where);
        Task<Photo> GetPhoto(Expression<Func<Photo, bool>> filter = null, 
                                Func<IQueryable<Photo>, IOrderedQueryable<Photo>> orderBy = null, 
                                params Expression<Func<Photo, object>>[] includes);
         
         Task<bool> SaveAll();
    }
}