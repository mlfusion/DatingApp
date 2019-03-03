using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Base;
using DatingApp.API.Business;
using DatingApp.API.Common.ActionFilters;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
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
        //private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private IRoleBus _roleBus { get; set; }

        private string RoleName = RoleType.User.ToString();

        public AuthController(IConfiguration config,
        IMapper mapper, ILog log, IRepositoryWrapper repositoryWrapper, IRoleBus roleBus)
        {
            _roleBus = roleBus;
            _log = log;
            _repositoryWrapper = repositoryWrapper;
            _config = config;
            _mapper = mapper;
            // _repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userDto)
        {
            using (_log.BeginScope())
            {

                //throw new Exception("Just testing exception. ");

                // validation request
                userDto.Username = userDto.Username.ToLower();
                userDto.KnownAs = userDto.KnownAs.UppercaseFirst();

                _log.Write($"UserDto object: {userDto.Username}");

                if (await _repositoryWrapper.Auth.UserExits(userDto.Username))
                    return BadRequest("Username already exists");

                var userToCreate = _mapper.Map<User>(userDto);
                // var userToCreate = new User
                // {
                //     Username = userDto.Username
                // };

                var createdUser = _mapper.Map<UserForReturnRegisterDto>(await _repositoryWrapper.Auth.Register(userToCreate, userDto.Password));

                _log.Write($"UserId={createdUser.Id} has been created");

                return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id }, createdUser);
            }
        }

        [HttpPost("login")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login(UserForLoginDto loginDto)
        {
            #region Sample of ActionFilter
            // if (userDto == null)
            // {
            //     return BadRequest("User is object is null");
            // }

            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }     
            #endregion

            //throw new Exception("Just testing exception. ");

            var login = await _repositoryWrapper.Auth.Login(loginDto.Username.ToLower(), loginDto.Password);

            if (login == null)
                return NotFound("No match for username and password was found.");

            if (login.RoleId > 0)
            {
                var _roleName = await  _roleBus.GetRole(login.RoleId);
                RoleName = _roleName.Name;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()),
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, RoleName),
                new Claim("GangName", "FLEX"),
                new Claim(Common.ClaimTypes.ApplicationId, "123456")
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

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user = userDto
            });
        }
    }
}