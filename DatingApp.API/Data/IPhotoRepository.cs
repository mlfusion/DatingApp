using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotos();
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetPhoto(Expression<Func<Photo, bool>> where);
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
    }
}