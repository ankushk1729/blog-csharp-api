using SM.Entities;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using SM.Data;

namespace SM.Utils
{
    public static class AuthUtil
    {
        public static bool AuthorizePermissions(ApiDBContext dBContext, User requestUser, Blog? resource = null)
        {
            if (UserUtil.GetUserRole(dBContext, requestUser.UserId) == "admin" || (resource != null && resource?.UserId == requestUser.UserId))
            {
                return true;
            }

            return false;
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

        public static User GetCurrentUser(ApiDBContext dBContext, HttpContext httpContext){
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string userEmail = "";
            if(identity != null) {
                userEmail = identity.Claims.FirstOrDefault()!.Value;    
            }
            var user = dBContext.Users.FirstOrDefault(user => user.Email == userEmail);

            return user;
        }

    }

}