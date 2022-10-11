using SM.Entities;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace SM.Utils
{
    public static class AuthUtil
    {
        public static bool AuthorizePermissions(User requestUser, Blog? resource = null)
        {
            if (requestUser.Role != "admin" || (resource != null && resource!.User.UserId != requestUser.UserId))
            {
                return false;
            }

            return true;
        }

        public static string CreateToken(IConfiguration _config, string userEmail) {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.Email, userEmail)
            };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }

}