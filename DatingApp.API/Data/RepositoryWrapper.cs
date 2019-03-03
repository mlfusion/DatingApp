using DatingApp.API.Base;

namespace DatingApp.API.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DataContext _context;
        private readonly ILog _log;
        private IAuthRepository _authRespository;
        private IPhotoRepository _photoRespository;
        private IDatingRepository _datingRespository;
        private IRoleRepository _roleRespository;

        public RepositoryWrapper(DataContext context, ILog log)
        {
            _context = context;
            _log = log;
        }
        public IPhotoRepository Photo {
            get {
                if (_photoRespository == null)
                {
                    _photoRespository = new PhotoRepository(_log, _context);
                }
                return _photoRespository;
            }
        }

        public IDatingRepository Date {
            get {
                if (_datingRespository == null)
                {
                    _datingRespository = new DatingRepository(_log, _context);
                }
                return _datingRespository;
            }
        }

        public IAuthRepository Auth {
            get {
                if (_authRespository == null)
                {
                    _authRespository = new AuthRepository(_log, _context);
                }
                return _authRespository;
            }
        }

        public IRoleRepository Role {
            get {
                if (_roleRespository == null)
                {
                    _roleRespository = new RoleRepository(_log, _context);
                }
                return _roleRespository;
            }
        }
    }

    public interface IRepositoryWrapper
    {
        IPhotoRepository Photo {get;}
        IDatingRepository Date {get;}
        IAuthRepository Auth {get;}
        IRoleRepository Role {get;}
    }
}