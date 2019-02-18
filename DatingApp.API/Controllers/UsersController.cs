using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Base;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
{
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public UsersController(IDatingRepository repo, IMapper mapper, ILog log)
        {
            _repo = repo;
            _mapper = mapper;
            _log = log;
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

                 _log.Write($"{await _repo.CountAsync(users)} user was return.");

                // Convert with AutoMapper User => UserForListDto
                var usersToMap = _mapper.Map<IEnumerable<UserForListDto>>(users);

                return Ok(usersToMap);
            }
        }

        [HttpGet("{id}")]
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
                _log.Write($"Update user id {id}.");

                if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
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