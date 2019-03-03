using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Common.Paging;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Business
{
    public class UserBus
    {
        private readonly ILog _log;
        private readonly IRepositoryWrapper _repository;

        public UserBus(ILog log, IRepositoryWrapper repository)
        {
            _log = log;
            _repository = repository;
        }
        
        public static bool ValidateUser(int userId)
        {
           //  if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
           return false;
        }

        
        public async Task<PagedList<User>> GetRolesByQuerySearch(UserParams param)
        {
            using (_log.BeginScope())
            {
                _log.Write($"Retrieving all users with params pagenumber {param.PageNumber} and pagesize {param.PageSize} and search by {param.Search}");

                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@PageSize", param.PageSize);
                parameters[1] = new SqlParameter("@PageNumber", param.PageNumber);
                parameters[2] = new SqlParameter("@Search", param.Search);
                parameters[3] = new SqlParameter("@SortOrder", param.SortOrder);
                parameters[4] = new SqlParameter("@SortColumn", param.SortColumn);

                var dt = await _repository.Date.ExecuteDataTableAsync("sp_UsersSelect", CommandType.StoredProcedure, parameters);

                var list = dt.DataTableToList<User>();

                // Add custom filters in this section
                //var getRoles = await _repository.Role.SelectAsync(param);
           
                _log.Write($"{list.Count} roles found");

                return await PagedList<User>.CreateSqlAsync(list, param.PageNumber, param.PageSize, (int) dt.Rows[0]["TotalRows"]);
            }
        }
    }
}