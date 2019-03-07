using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Base;
using DatingApp.API.Business;
using DatingApp.API.Common;
using DatingApp.API.Common.ActionFilters;
using DatingApp.API.Common.Paging;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    
    [Authorize]
    //[ServiceFilter(typeof(UserFilterAttribute))] // Action Filter is in place
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
{
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        private readonly IUserBus _userbus;

        public UsersController(IDatingRepository repo, IMapper mapper, ILog log, IUserBus userbus)
        {
            _repo = repo;
            _mapper = mapper;
            _log = log;
            _userbus = userbus;
        }

        [HttpGet] 
        public async Task<IActionResult> GetUsers()
        {
            using(_log.BeginScope())
            {
                _log.Write($"Retrieving all active users.");
                var users = await _repo.GetUsers();

                if (users == null)
                    return NotFound();

                 _log.Write($"{_repo.Count()} user was return.");

                // Convert with AutoMapper User => UserForListDto
                var usersToMap = _mapper.Map<IEnumerable<UserForListDto>>(users);

                return Ok(usersToMap);
            }
        }

        [HttpGet]
        [Route("all")] 
        public async Task<ApiResult<IEnumerable<UserForListDto>>> GetAllUsers([FromQuery] UserParams param)
        {
            using(_log.BeginScope())
            {
                _log.Write($"Retrieving all active users.");
                var users = await _userbus.GetUsersByQuerySearch(param);

                if (users == null)
                    return ApiResult<IEnumerable<UserForListDto>>.NotFound("No users was found.");
                
                // Add pagination in response header
                Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

                // Convert with AutoMapper User => UserForListDto
                var usersToMap = _mapper.Map<IEnumerable<UserForListDto>>(users);

                return ApiResult<IEnumerable<UserForListDto>>.Ok(usersToMap);
            }
        }

        [HttpGet("{id}", Name= "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            using(_log.BeginScope())
            {  
                _log.Write($"Retrieving user id {id}");
                var user = await _repo.GetUser(id);

                if (user == null)
                {
                    _log.Write($"No user was found with id {id}.");
                    return NotFound();
                }

                _log.Write($"{user.Username} was returned.");

                // Convert with AutoMapper, User object to Dto
                var userToMap = _mapper.Map<UserForDetailedDto>(user);

                return Ok(userToMap);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto updateDto)
        {
            using(_log.BeginScope())
            {
                _log.Write($"Update user id {id}.");//
 
                if (RoleType.Admin.ToString() != User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value)
                    return Unauthorized();

                var userFromRepo = await _repo.GetUser(id);
                userFromRepo.Modified = DateTime.Now;

                _mapper.Map(updateDto, userFromRepo);

                if (await _repo.SaveAync())
                {
                    _log.Write($"Update user {userFromRepo.Username} successfully.");
                    return NoContent();
                }

                throw new Exception($"Updating user {id} failed on save.");
            }
        }
     }
}