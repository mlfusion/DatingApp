using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Common
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;

        public ConfigureJwtBearerOptions(IOptions<JwtAuthentication> jwtAuthentication)
        {
            _jwtAuthentication = jwtAuthentication ?? throw new System.ArgumentNullException(nameof(jwtAuthentication));
        }

        public void PostConfigure(string name, JwtBearerOptions options)
        {
            var jwtAuthentication = _jwtAuthentication.Value;

            options.ClaimsIssuer = jwtAuthentication.ValidIssuer;
            options.IncludeErrorDetails = true;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtAuthentication.ValidIssuer,
                ValidAudience = jwtAuthentication.ValidAudience
                //IssuerSigningKey = jwtAuthentication.SecurityKey,
                //NameClaimType = ClaimTypes.NameIdentifier
            };
        }
    }
}