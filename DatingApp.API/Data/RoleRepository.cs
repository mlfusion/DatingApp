using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Infrastructure;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ILog log, DataContext context) : base(log, context)
        {
            // DataTable ExecuteDataTable(string commandName, CommandType cmdType, SqlParameter[] sqlParameter)
        }
    }

    public interface IRoleRepository : IRepository<Role>
    {
        
    }
}