using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Common.Paging;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Business
{
    public interface IUserBus
    {
        Task<PagedList<User>> GetUsersByQuerySearch(UserParams param);
    }

    public class UserBus : IUserBus
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

        public async Task<PagedList<User>> GetUsersByQuerySearch(UserParams param)
        {
            using (_log.BeginScope())
            {
                _log.Write($"Retrieving all users with params pagenumber {param.PageNumber} and pagesize {param.PageSize} and search by {param.Search}");

                //var list = await _repository.Date.GetUsersSqlAsync(param);

                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@PageSize", param.PageSize);
                parameters[1] = new SqlParameter("@PageNumber", param.PageNumber);
                parameters[2] = new SqlParameter("@Search", param.Search);
                parameters[3] = new SqlParameter("@SortOrder", param.SortOrder);
                parameters[4] = new SqlParameter("@SortColumn", param.SortColumn);

                var dt = await _repository.Date.ExecuteDataTableAsync("sp_UsersSelect", CommandType.StoredProcedure, parameters);

                // var list = dt.DataTableToList<User>();
                List<User> list = new List<User>();
                foreach(DataRow dr in dt.Rows)
                {
                    list.Add(new User { 
                            Id = (int) dr["Id"],
                            Username = (string) dr["Username"],
                            Gender = (string) dr["Gender"],
                            Created = (DateTime?) dr["Created"],
                            Modified = (DateTime?) (dr["Modified"] == DBNull.Value ? null : dr["Modified"]),
                            KnownAs = (string) dr["KnownAs"],
                            DateOfBirth = (DateTime?) dr["DateOfBirth"],
                            LastAcitve = (DateTime?) (dr["LastAcitve"] == DBNull.Value ? DateTime.MinValue : dr["LastAcitve"]),
                            City = (string) dr["City"],
                            Country = (string) dr["Country"],
                            RoleId = (int) dr["RoleId"],
                            Photos = new List<Photo>{
                                new Photo { Url = (string) dr["PhotoUrl"], IsMain = (bool) dr["IsMain"] }
                            }
                         });
                    };

                _log.Write($"{list.Count()} users found");

                return await PagedList<User>.CreateSqlAsync(list, param.PageNumber, param.PageSize, (int) dt.Rows[0]["TotalRows"]);

            }
    
        }
    }
}