using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Base;
using DatingApp.API.Common;
using DatingApp.API.Common.Paging;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Business
{
    public interface IRoleBus
    {
        Task<PagedList<Role>> GetRoles(Params param);
        Task<IEnumerable<Role>> GetActiveRoles();
         Task<PagedList<Role>> GetRolesByQuerySearch(Params param);
        Task<Role> GetRole(int id);
        Task<Role> AddRole (Role role);
        Task<Role> UpdateRole (Role role);
        Task<bool> IsSafeToDeleteRole(Role role);
    }
    public class RoleBus : IRoleBus
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public RoleBus(IRepositoryWrapper repository, IMapper mapper, ILog log)
        {
            _repository = repository;
            _mapper = mapper;
            _log = log;
        }

        public async Task<PagedList<Role>> GetRoles(Params param)
        {
            using (_log.BeginScope())
            {
                _log.Write($"Retrieving all roles with params pagenumber {param.PageNumber} and pagesize {param.PageSize}");

                // Add custom filters in this section
                var getRoles = await _repository.Role.SelectAsync(param);
           
                _log.Write($"{getRoles.TotalCount} roles found");

                return getRoles;
            }
        }

        public async Task<PagedList<Role>> GetRolesByQuerySearch(Params param)
        {
            using (_log.BeginScope())
            {
                _log.Write($"Retrieving all roles with params pagenumber {param.PageNumber} and pagesize {param.PageSize} and search by {param.Search}");

                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@PageSize", param.PageSize);
                parameters[1] = new SqlParameter("@PageNumber", param.PageNumber);
                parameters[2] = new SqlParameter("@Search", param.Search);
                parameters[3] = new SqlParameter("@SortOrder", param.SortOrder);
                parameters[4] = new SqlParameter("@SortColumn", param.SortColumn);

                var dt = await _repository.Role.ExecuteDataTableAsync("sp_RolesSelect", CommandType.StoredProcedure, parameters);

                var list = dt.DataTableToList<Role>();

                // Add custom filters in this section
                //var getRoles = await _repository.Role.SelectAsync(param);
           
                _log.Write($"{list.Count} roles found");

                return await PagedList<Role>.CreateSqlAsync(list, param.PageNumber, param.PageSize, (int) dt.Rows[0]["TotalRows"]);
            }
        }



        public async Task<IEnumerable<Role>> GetActiveRoles()
        {
            using (_log.BeginScope())
            {
                _log.Write("Retrieving all roles");
                var getRoles = await _repository.Role.SelectAsync(x => x.Active == true, 0);
                _log.Write($"{getRoles.Count()} roles found");
                return getRoles;
            }
        }

        public async Task<Role> GetRole(int id)
        {
            using (_log.BeginScope())
            {
                var getRole = await _repository.Role.SelectAsync(id);

                if (getRole == null)
                    _log.Write($"No match was found with RoleId={id}");
                else
                    _log.Write($"Returned role name {getRole.Name}");

                return getRole;
            }
        }

        public async Task<Role> AddRole (Role role)
        {
            using (_log.BeginScope())
            {
                _log.Write($"Added new role name {role.Name}");

                await _repository.Role.AddAsync(role);
                await _repository.Role.SaveAync();

                return role;
            }
        }

        public async Task<Role> UpdateRole (Role role)
        {
            using (_log.BeginScope())
            {
                _log.Write($"Updating role name {role.Name}");

                role.Modified = DateTime.Now;

                await _repository.Role.UpdateAsync(role);
                await _repository.Role.SaveAync();

                return role;
            }
        }

        public async Task<bool> IsSafeToDeleteRole(Role role)
        {
            var userHasRole = await _repository.Date.SelectAsync(x => x.RoleId == role.Id, 0);

            if (userHasRole.Count() > 0)
                return true;

            return false;
        }
    }
}