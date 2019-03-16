using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Data;
using DatingApp.API.Models;

namespace DatingApp.API.Business
{
    public class LikeBus
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILog _log;

        public LikeBus(IRepositoryWrapper repository, ILog log)
        {
            _repository = repository;
            _log = log;
        }

        public async Task<Like> GetLike(int userId, int likeId)
        {
            using(_log.BeginScope())
            {
                var like = await _repository.Like.SelectAsync(x => x.LikeeId == userId && x.LikerId == likeId);
                return like;
            }
        }

        public async Task<Like> GetLike(int likeId)
        {
            using(_log.BeginScope())
            {
                var like = await _repository.Like.SelectAsync(x => x.LikerId == likeId);
                return like;
            }
        }

        public async Task<IEnumerable<Like>> GetAllLikes(int userId)
        {
            using(_log.BeginScope())
            {
                var likes = await _repository.Like.SelectAsync(x => x.LikerId == userId, 0);
                
                _log.Write($"{likes.Count()} likes was found.");
                return likes;
            }
        }
        public async Task<bool> AddLike(Like like)
        {
            using(_log.BeginScope())
            {
                await _repository.Like.AddAsync(like);
                _log.Write($"Added LikeeId={like.LikeeId} with LikerId={like.LikerId}");
                return await _repository.Like.SaveAync() ? true : false;
            }
        }

        public async Task<bool> DeleteLike(Like like)
        {
            using(_log.BeginScope())
            {
                await _repository.Like.DeleteAsync(like);
                _log.Write($"Delete LikeId={like.LikeeId}");
                return await _repository.Like.SaveAync() ? true : false;
            }
        }
    }

    public interface ILikeBus
    {
        Task<Like> GetLike(int likeId);
        Task<Like> GetLike(int userId, int likeId);
        Task<IEnumerable<Like>> GetAllLikes(int userId);
        Task<bool> AddLike(Like like);
        Task<bool> DeleteLike(Like like);
    }
}