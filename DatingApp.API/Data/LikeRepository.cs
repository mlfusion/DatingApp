using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Base;
using DatingApp.API.Common.Paging;
using DatingApp.API.Infrastructure;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data 
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly ILog _log;
        private readonly DataContext _context;

        public LikeRepository(ILog log, DataContext context) : base(log, context)
        {
            _log = log;
            _context = context; 
        }
    }

    public interface ILikeRepository : IRepository<Like>
    {

    }
}