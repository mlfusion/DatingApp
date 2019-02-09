using System.Security.Claims;

namespace DatingApp.API.Business
{
    public class UserBus
    {        
        public UserBus()
        {
            
        }
        
        public static bool ValidateUser(int userId)
        {
           //  if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
           return false;
        }
    }
}