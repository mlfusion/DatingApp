using System.Text;
using Microsoft.Extensions.Configuration; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Common.Extensions
{
    public static class JwtExtensions
    {
        private static IConfiguration _configuration { get; set; }
        public static void ConfigureJwtBearerToken(this IServiceCollection services)
        {
          //IConfiguration configuration;
           // _configuration = configuration;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}