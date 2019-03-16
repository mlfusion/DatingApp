using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Base;
using DatingApp.API.Business;
using DatingApp.API.Common;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
     [Route("api/user/{userid}/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeBus _likeBus;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public LikeController(ILikeBus likeBus, IMapper mapper, ILog log)
        {
            _likeBus = likeBus;
            _mapper = mapper;
            _log = log;
        }

        [HttpPost]
        public async Task<ApiResult<Like>> AddLikeUser(int userid, int likerid)
        {
            var like = await _likeBus.GetLike(userid, likerid);

            if (like != null)
                return ApiResult<Like>.BadRequest("You already like this user");

            if (_likeBus.GetLike(likerid) == null)
                return ApiResult<Like>.NotFound("This user ws not found");

            like = new Like {LikeeId = likerid, LikerId = userid};

            if(await _likeBus.AddLike(like))
                return ApiResult<Like>.Ok();

            return ApiResult<Like>.BadRequest("Failed to like user");
        }

        [HttpDelete]
        [Route("{likerid}")]
        public async Task<ApiResult<Like>> DeleteLikeUser(int userid, int likerid)
        {

            var like = new Like {LikeeId = likerid, LikerId = userid};

            if(await _likeBus.DeleteLike(like))
                return ApiResult<Like>.Ok();

            return ApiResult<Like>.BadRequest("Failed to delete like user");
        }
    }
}