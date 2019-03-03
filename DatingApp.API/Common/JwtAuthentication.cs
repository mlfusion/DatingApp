using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Common
{
    public class JwtAuthentication
    {
    public string SecurityKey { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }

    private SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
    public SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
}
}