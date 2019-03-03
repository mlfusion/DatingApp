using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Common.ActionFilters
{
    public class UserFilterAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var _userId = int.Parse(resultContext.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var _datingRepository = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();

            var _user = await _datingRepository.SelectAsync(_userId);
            if (_user != null)
            {
                _user.LastAcitve = DateTime.Now;
                await _datingRepository.SaveAync();
            }
        }
    }
}