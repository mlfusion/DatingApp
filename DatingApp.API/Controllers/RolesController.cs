using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Base;
using DatingApp.API.Business;
using DatingApp.API.Common;
using DatingApp.API.Common.Paging;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleBus _roleBus;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public RolesController(IRoleBus roleBus, IMapper mapper, ILog log)
        {
            _roleBus = roleBus;
            _mapper = mapper;
            _log = log;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ApiResult<IEnumerable<RoleForDto>>> GetRoles([FromQuery] Params param)
        {
            using(_log.BeginScope())
            {
                 _log.Write($"Retrieving all roles.");
                var roles = await _roleBus.GetRolesByQuerySearch(param);

                if (roles == null) {
                    _log.Write($"No roles was found");
                    return ApiResult<IEnumerable<RoleForDto>>.NotFound("You do not have permission to view all samples");
                }

                // Add pagination in response header
                Response.AddPagination(roles.CurrentPage, roles.PageSize, roles.TotalCount, roles.TotalPages);

                // Convert to Dto object
                var dtoRoles = _mapper.Map<IEnumerable<RoleForDto>>(roles);

                return ApiResult<IEnumerable<RoleForDto>>.Ok(dtoRoles);
            }
        }

        [HttpGet]
        public async Task<ApiResult<IEnumerable<RoleForDto>>> GetActiveRoles()
        {
            using(_log.BeginScope())
            {
                var roles = await _roleBus.GetActiveRoles();

                if (roles == null) {
                    _log.Write($"No roles was found");
                    return ApiResult<IEnumerable<RoleForDto>>.NotFound("You do not have permission to view all samples");
                }

                var dtoRoles = _mapper.Map<IEnumerable<RoleForDto>>(roles);

                return ApiResult<IEnumerable<RoleForDto>>.Ok(dtoRoles);

            }
        }

        [HttpGet("{id}", Name= "GetRole")]
        public async Task<ApiResult<RoleForDto>> GetRole(int id)
        {
            using(_log.BeginScope())
            {
                var role = await _roleBus.GetRole(id);

                if (role == null) {
                    _log.Write($"No role was found");
                    return ApiResult<RoleForDto>.NotFound("No role was found");
                }

                var dtoRole = _mapper.Map<RoleForDto>(role);

                return ApiResult<RoleForDto>.Ok(dtoRole);

            }       
        }

        [HttpPost]
        public async Task<ApiResult<RoleForDto>> AddRole(RoleForDto roleDto)
        {
            using(_log.BeginScope())
            {
                _log.Write($"Adding new role {roleDto.Name}");

                var role = _mapper.Map<RoleForDto, Role>(roleDto);

                await _roleBus.AddRole(role);

                _log.Write($"Successfully added new role id {role.Id}");

                var roleDtoReturn = _mapper.Map<Role, RoleForDto>(role);

                return ApiResult<RoleForDto>.Created(roleDtoReturn);
            }
        }

        [HttpPut]
        public async Task<ApiResult<RoleForDto>> UpdateRole(RoleForDto roleDto)
        {
            using(_log.BeginScope())
            {
                _log.Write($"Update role {roleDto.Name}");

                var role = _mapper.Map<RoleForDto, Role>(roleDto);

                // check if there are user with this role before delete
                // if there is send message that role can't be deleted
                if (!roleDto.Active)
                    if (await _roleBus.IsSafeToDeleteRole(role))
                    {
                        return ApiResult<RoleForDto>.Forbidden("There are active users with this role. Please remove all users to delete role.");
                    }

                await _roleBus.UpdateRole(role);

                _log.Write($"Successfully update role id {role.Id}");

                var roleDtoReturn = _mapper.Map<Role, RoleForDto>(role);

                return ApiResult<RoleForDto>.Created(roleDtoReturn);
            }
        }
    }
}