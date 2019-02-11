using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repository, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userDto)
        {
            //if (!ModelState.IsValid)
            // {

            // }

            // validation request
            userDto.Username = userDto.Username.ToLower();

            if (await _repository.UserExits(userDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userDto.Username
            };

            var createdUser = await _repository.Register(userToCreate, userDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto loginDto)
        {
            //throw new Exception("Just testing exception. ");

            var login = await _repository.Login(loginDto.Username.ToLower(), loginDto.Password);

            if (login == null)
                return NotFound("No match for username and password was found.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()),
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "Admin")//,
               // new Claim(ClaimTypes., login.Photos.FirstOrDefault(d => d.IsMain == true).Url )
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(50000),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDesciptor);

            var userDto = _mapper.Map<User, UserForListDto>(login);

            return Ok(new { 
                token = tokenHandler.WriteToken(token),
                user = userDto
            });
        }
    }
}